using DbmsWcfClient.DbmsServiceReference;
using DbmsWcfService.Models;
using System;
using System.Windows.Forms;

namespace DbmsWcfClient
{
    public partial class FormTable : Form
    {
        private readonly DbmsServiceClient client;
        private readonly string dbName;
        public readonly TableInfo TableInfo;

        public FormTable(DbmsServiceClient client, string dbName, TableInfo tableInfo)
        {
            InitializeComponent();
            this.client = client;
            this.dbName = dbName;
            TableInfo = tableInfo;
            Text = TableInfo.Name;
            RefreshRows();
            if (TableInfo is TableDifferenceInfo)
            {
                dataGridView.ReadOnly = true;
                dataGridView.AllowUserToDeleteRows = false;
                dataGridView.AllowUserToAddRows = false;
            }
        }

        public void RefreshRows()
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
            Table table = client.GetTable(dbName, TableInfo.Id);
            foreach (Column column in table.Columns)
                dataGridView.Columns.Add("", column.Name + "\n" + column.Type);
            foreach (DataGridViewColumn column in dataGridView.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            foreach (Row row in table.Rows.Values)
                dataGridView.Rows[dataGridView.Rows.Add(row.Cells)].Tag = row.Id;
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex == dataGridView.NewRowIndex || !dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].IsInEditMode)
                return;
            try
            {
                client.ValidateCell(dbName, TableInfo.Id, e.ColumnIndex, e.FormattedValue.ToString());
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show(ex.Message, "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == dataGridView.NewRowIndex)
                return;
            try
            {
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                    client.ValidateCell(dbName, TableInfo.Id, i, dataGridView.Rows[e.RowIndex].Cells[i].Value?.ToString() ?? "");
            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show("Fill all cells to create row (or remove the entire row)", "Cannot create row", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvRow = dataGridView.Rows[e.RowIndex];
            int? rowId = dgvRow.Tag as int?;
            if (rowId == null)
            {
                try
                {
                    string[] cells = new string[dataGridView.Columns.Count];
                    for (int i = 0; i < dataGridView.Columns.Count; i++)
                        cells[i] = dgvRow.Cells[i].Value?.ToString() ?? "";
                    dgvRow.Tag = client.AddRow(dbName, TableInfo.Id, cells);
                    FormDatabase parentForm = Parent.FindForm() as FormDatabase;
                    if (parentForm != null)
                        parentForm.RefreshDifferences(TableInfo.Id);
                }
                catch { }
            }
            else
            {
                DataGridViewCell dgvCell = dgvRow.Cells[e.ColumnIndex];
                client.UpdateCell(dbName, TableInfo.Id, rowId.Value, e.ColumnIndex, dgvCell.Value?.ToString() ?? "");
                FormDatabase parentForm = Parent.FindForm() as FormDatabase;
                parentForm?.RefreshDifferences(TableInfo.Id);
            }
        }

        private void dataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            int? dbRow = e.Row.Tag as int?;
            if (dbRow == null)
                return;
            client.RemoveRow(dbName, TableInfo.Id, dbRow.Value);
            FormDatabase parentForm = Parent.FindForm() as FormDatabase;
            if (parentForm != null)
                parentForm.RefreshDifferences(TableInfo.Id);
        }

        private void FormTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormDatabase parentForm = Parent.FindForm() as FormDatabase;
            if (parentForm != null)
                parentForm.ListTables(TableInfo.Id);
        }
    }
}

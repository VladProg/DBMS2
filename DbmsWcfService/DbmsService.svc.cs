using DbmsWcfService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace DbmsWcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DbmsService : IDbmsService
    {
        readonly Dictionary<string, Database> databases = new Dictionary<string, Database>();

        public int AddRow(string dbName, int tableId, string[] cells)
        {
            Table table = GetTable(dbName, tableId);
            if (cells.Length != table.Columns.Count)
                throw new ArgumentException("Row length must be the same as number of columns");
            Models.Values.Value[] values = new Models.Values.Value[cells.Length];
            for (int i = 0; i < cells.Length; i++)
                values[i] = table.Columns[i].Type.Parse(cells[i]);
            return table.AddRow(values).Id;
        }

        public int AddTable(string dbName, string tableName, Column[] columns)
        {
            Database database = GetFullDatabase(dbName);
            return database.AddTable(tableName, columns).Id;
        }

        public void RemoveTable(string dbName, int tableid)
        {
            Database database = GetFullDatabase(dbName);
            database.RemoveTable(tableid);
        }

        public DatabaseInfo GetDatabase(string dbName)
        {
            return GetFullDatabase(dbName).Info();
        }

        private Database GetFullDatabase(string dbName)
        {
            if (!databases.ContainsKey(dbName))
                throw new ArgumentException($"Cannot find database '{dbName}'");
            return databases[dbName];
        }

        public Table GetTable(string dbName, int tableId)
        {
            var ids = TableDifference.ParseId(tableId);
            if (ids.HasValue)
                return GetTableDifference(dbName, ids.Value.leftId, ids.Value.rightId);
            Database database = GetFullDatabase(dbName);
            if (!database.Tables.ContainsKey(tableId))
                throw new ArgumentException($"Database '{dbName}' doesn't contain table #{tableId}");
            return database.Tables[tableId];
        }

        public Table GetTableDifference(string dbName, int leftTableId, int rightTableId)
        {
            Table leftTable = GetTable(dbName, leftTableId);
            Table rightTable = GetTable(dbName, rightTableId);
            TableDifference difference = TableDifference.Create(leftTable, rightTable);
            difference.Calculate();
            return difference;
        }

        public void RemoveRow(string dbName, int tableId, int rowId)
        {
            Table table = GetTable(dbName, tableId);
            table.RemoveRow(rowId);
        }

        public void UpdateCell(string dbName, int tableId, int rowId, int columnId, string value)
        {
            Table table = GetTable(dbName, tableId);
            table.Rows[rowId].Cells[columnId] = table.Columns[columnId].Type.Parse(value);
        }

        public void ValidateCell(string dbName, int tableId, int columnId, string value)
        {
            Table table = GetTable(dbName, tableId);
            table.Columns[columnId].Type.Parse(value);
        }

        public void CreateDatabase(string dbName)
        {
            if (databases.ContainsKey(dbName))
                throw new ArgumentException($"Database '{dbName}' already exists");
            else
                databases[dbName] = new Database();
        }

        public void DeleteDatabase(string dbName)
        {
            if (databases.ContainsKey(dbName))
                databases.Remove(dbName);
            else
                throw new ArgumentException($"Cannot find database '{dbName}'");
        }

        public bool TableExists(string dbName, int tableId)
        {
            try
            {
                GetTable(dbName, tableId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

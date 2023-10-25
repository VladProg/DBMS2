using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models
{
    [DataContract]
    [KnownType(typeof(TableDifferenceInfo))]
    public class TableInfo
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Name { get; private set; }

        public TableInfo(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    [DataContract]
    [KnownType(typeof(TableDifference))]
    public class Table
    {
        [DataMember]
        public int Id;
        [DataMember]
        public string Name;
        [DataMember]
        private Column[] columns;
        [DataMember]
        private SortedDictionary<int, Row> rows = new SortedDictionary<int, Row>();
        private int nextId = 0;

        public ReadOnlyCollection<Column> Columns => Array.AsReadOnly(columns);
        public IReadOnlyDictionary<int, Row> Rows => rows;

        public Table(int id, string name, Column[] columns)
        {
            Id = id;
            Name = name;
            this.columns = (Column[])columns.Clone();
        }

        public virtual Row AddRow(Values.Value[] cells)
        {
            if (cells.Length != columns.Length)
                throw new ArgumentException("Row length must be the same as number of columns");
            for (int i = 0; i < columns.Length; i++)
                if (cells[i].Type != columns[i].Type)
                    throw new ArgumentException("Cell type doesn't match corresponding column type");
            Row row = new Row(nextId, cells);
            rows[nextId] = row;
            nextId++;
            return row;
        }

        public virtual void RemoveRow(int id)
        {
            rows.Remove(id);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Name);
            writer.Write(columns.Length);
            foreach (Column column in columns)
                column.Write(writer);
            writer.Write(rows.Count);
            foreach (KeyValuePair<int, Row> entry in rows)
                entry.Value.Write(writer);
            writer.Write(nextId);
        }

        public Table(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            Name = reader.ReadString();
            int columnCount = reader.ReadInt32();
            columns = new Column[columnCount];
            for (int i = 0; i < columnCount; i++)
                columns[i] = new Column(reader);
            int rowCount = reader.ReadInt32();
            for (int i = 0; i < rowCount; i++)
            {
                Row row = new Row(reader, columns);
                rows[row.Id] = row;
            }
            nextId = reader.ReadInt32();
        }

        public bool ContainsRow(Row row)
        {
            foreach (KeyValuePair<int, Row> entry in rows)
                if (row.Equals(entry.Value))
                    return true;
            return false;
        }

        public virtual void ClearRows()
        {
            rows.Clear();
        }

        public virtual TableInfo Info()
        {
            return new TableInfo(Id, Name);
        }
    }
}

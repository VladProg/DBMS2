using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models
{
    [DataContract]
    public class DatabaseInfo
    {
        [DataMember]
        public TableInfo[] Tables { get; private set; }
        [DataMember]
        public TableDifferenceInfo[] TableDifferences { get; private set; }

        public DatabaseInfo(TableInfo[] tables, TableDifferenceInfo[] tableDifferences)
        {
            Tables = tables;
            TableDifferences = tableDifferences;
        }
    }

    public class Database
    {
        private readonly SortedDictionary<int, Table> tables = new SortedDictionary<int, Table>();
        private int nextId = 0;

        public int TableCount
        {
            get { return tables.Count; }
        }

        public IReadOnlyDictionary<int, Table> Tables
        {
            get { return tables; }
        }

        public Database() { }

        public Table AddTable(string name, Column[] columns)
        {
            Table table = new Table(nextId, name, columns);
            tables[nextId] = table;
            nextId++;
            return table;
        }

        public void RemoveTable(int id)
        {
            tables.Remove(id);
        }

        public void Save(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                Write(writer);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(TableCount);
            foreach (KeyValuePair<int, Table> entry in tables)
                entry.Value.Write(writer);
            writer.Write(nextId);
        }

        public static Database Load(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                Database database = new Database(reader);
                if (fs.Position != fs.Length)
                    throw new InvalidOperationException("File has extra data");
                return database;
            }
        }

        public Database(BinaryReader reader)
        {
            int tableCount = reader.ReadInt32();
            for (int i = 0; i < tableCount; i++)
            {
                Table table = new Table(reader);
                tables[table.Id] = table;
            }
            nextId = reader.ReadInt32();
        }

        public DatabaseInfo Info()
        {
            TableInfo[] tables = (from table in Tables.Values select table.Info()).ToArray();
            TableDifferenceInfo[] tableDifferences =
                (from leftTable in Tables.Values
                 from rightTable in Tables.Values
                 where leftTable.Id != rightTable.Id
                 let difference = TableDifference.CreateOrNull(leftTable, rightTable)
                 where difference != null
                 select difference.Info() as TableDifferenceInfo).ToArray();
            return new DatabaseInfo(tables, tableDifferences);
        }
    }
}

using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models
{
    [DataContract]
    public class Row
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public Values.Value[] Cells { get; private set; }

        public Row(int id, Values.Value[] cells)
        {
            Id = id;
            Cells = cells;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Id);
            foreach (Values.Value cell in Cells)
                cell.Write(writer);
        }

        public Row(BinaryReader reader, Column[] columns)
        {
            Id = reader.ReadInt32();
            Cells = new Values.Value[columns.Length];
            for (int i = 0; i < columns.Length; i++)
                Cells[i] = columns[i].Type.ReadValue(reader);
        }

        public override bool Equals(object obj)
        {
            return obj is Row other && Cells.SequenceEqual(other.Cells);
        }
    }
}

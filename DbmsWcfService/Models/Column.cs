using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models
{
    [DataContract]
    public class Column
    {
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public Types.Type Type { get; private set; }

        public Column(string name, Types.Type type)
        {
            Name = name;
            Type = type;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Name);
            Type.Write(writer);
        }

        public Column(BinaryReader reader)
        {
            Name = reader.ReadString();
            Type = Types.Type.Read(reader);
        }
    }
}

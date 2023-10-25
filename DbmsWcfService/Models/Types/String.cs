using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Types
{
    [DataContract]
    public class String : Type
    {
        public const byte CODE = 4;

        public override Values.Value Parse(string s)
        {
            return new Values.String(this, s);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(CODE);
        }

        public override Values.Value ReadValue(BinaryReader reader)
        {
            return new Values.String(reader, this);
        }

        public override bool Equals(object obj)
        {
            return obj is String;
        }

        public override string ToString()
        {
            return "String";
        }
    }
}

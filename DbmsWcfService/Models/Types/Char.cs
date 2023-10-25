using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Types
{
    [DataContract]
    public class Char : Type
    {
        public const byte CODE = 3;

        public override Values.Value Parse(string s)
        {
            return new Values.Char(this, s);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(CODE);
        }

        public override Values.Value ReadValue(BinaryReader reader)
        {
            return new Values.Char(reader, this);
        }

        public override bool Equals(object obj)
        {
            return obj is Char;
        }

        public override string ToString()
        {
            return "Char";
        }
    }
}

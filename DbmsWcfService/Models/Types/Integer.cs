using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Types
{
    [DataContract]
    public class Integer : Type
    {
        public const byte CODE = 1;

        public override Values.Value Parse(string s)
        {
            return new Values.Integer(this, s);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(CODE);
        }

        public override Values.Value ReadValue(BinaryReader reader)
        {
            return new Values.Integer(reader, this);
        }

        public override bool Equals(object obj)
        {
            return obj is Integer;
        }

        public override string ToString()
        {
            return "Integer";
        }
    }
}

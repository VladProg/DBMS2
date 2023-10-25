using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Types
{
    [DataContract]
    public class Real : Type
    {
        public const byte CODE = 2;

        public override Values.Value Parse(string s)
        {
            return new Values.Real(this, s);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(CODE);
        }

        public override Values.Value ReadValue(BinaryReader reader)
        {
            return new Values.Real(reader, this);
        }

        public override bool Equals(object obj)
        {
            return obj is Real;
        }

        public override string ToString()
        {
            return "Real";
        }
    }
}

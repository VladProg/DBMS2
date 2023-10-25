using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Types
{
    [DataContract]
    public class Color : Type
    {
        public const byte CODE = 5;

        public override Values.Value Parse(string s)
        {
            return new Values.Color(this, s);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(CODE);
        }

        public override Values.Value ReadValue(BinaryReader reader)
        {
            return new Values.Color(reader, this);
        }

        public override bool Equals(object obj)
        {
            return obj is Color;
        }

        public override string ToString()
        {
            return "Color";
        }
    }
}

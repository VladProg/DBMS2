using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Values
{
    [DataContract]
    public class Real : Value
    {
        [DataMember]
        public decimal Val { get; private set; }

        public Real(Types.Real type, decimal val) : base(type)
        {
            Val = val;
        }

        public Real(Types.Real type, string s)
            : base(type)
        {
            try
            {
                Val = decimal.Parse(s.Replace(",", "."), CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new FormatException("Incorrect value for type Real");
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(Val);
        }

        public Real(BinaryReader reader, Types.Real type) : this(type, reader.ReadDecimal()) { }

        public override string ToString()
        {
            return Val.ToString().Replace(".", ",");
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && obj is Real other && Val == other.Val;
        }
    }
}

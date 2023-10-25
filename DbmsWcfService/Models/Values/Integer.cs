using System;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Values
{
    [DataContract]
    public class Integer : Value
    {
        [DataMember]
        public BigInteger Val { get; private set; }

        public Integer(Types.Integer type, BigInteger val) : base(type)
        {
            Val = val;
        }

        public Integer(Types.Integer type, string s) : base(type)
        {
            try
            {
                Val = BigInteger.Parse(s);
            }
            catch
            {
                throw new FormatException("Incorrect value for type Integer");
            }
        }

        public override void Write(BinaryWriter writer)
        {
            byte[] bytes = Val.ToByteArray();
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        public Integer(BinaryReader reader, Types.Integer type) : base(type)
        {
            int byteCount = reader.ReadInt32();
            byte[] bytes = reader.ReadBytes(byteCount);
            Val = new BigInteger(bytes);
        }

        public override string ToString() => Val.ToString();

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && obj is Integer other && Val == other.Val;
        }
    }
}

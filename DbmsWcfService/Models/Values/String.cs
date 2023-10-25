using System.Numerics;
using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Values
{
    [DataContract]
    public class String : Value
    {
        [DataMember]
        public string Val { get; private set; }

        public String(Types.String type, string val) : base(type)
        {
            Val = val;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(Val);
        }

        public String(BinaryReader reader, Types.String type) : this(type, reader.ReadString()) { }

        public override string ToString()
        {
            return Val;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            String other = obj as String;
            return other != null && Val == other.Val;
        }
    }
}

using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Values
{
    [DataContract]
    [KnownType(typeof(String))]
    [KnownType(typeof(Real))]
    [KnownType(typeof(Integer))]
    [KnownType(typeof(ColorInvl))]
    [KnownType(typeof(Color))]
    [KnownType(typeof(Char))]
    public abstract class Value
    {
        [DataMember]
        public Types.Type Type { get; private set; }

        public Value(Types.Type type)
        {
            Type = type;
        }

        public abstract void Write(BinaryWriter writer);

        public override bool Equals(object obj)
        {
            var other = obj as Value;
            return other != null && Type.Equals(other.Type);
        }
    }
}

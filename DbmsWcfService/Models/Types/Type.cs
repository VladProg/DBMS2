using System;
using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Types
{
    [DataContract]
    [KnownType(typeof(String))]
    [KnownType(typeof(Real))]
    [KnownType(typeof(Integer))]
    [KnownType(typeof(ColorInvl))]
    [KnownType(typeof(Color))]
    [KnownType(typeof(Char))]
    public abstract class Type
    {
        public abstract Values.Value Parse(string s);
        public abstract void Write(BinaryWriter writer);

        public static Type Read(BinaryReader reader)
        {
            byte code = reader.ReadByte();
            switch (code)
            {
                case Integer.CODE:
                    return new Integer();
                case Real.CODE:
                    return new Real();
                case Char.CODE:
                    return new Char();
                case String.CODE:
                    return new String();
                case Color.CODE:
                    return new Color();
                case ColorInvl.CODE:
                    return new ColorInvl(reader);
                default:
                    throw new NotImplementedException("Unknown type code");
            }
        }

        public abstract Values.Value ReadValue(BinaryReader reader);

        public static bool operator ==(Type left, Type right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Type left, Type right)
        {
            return !left.Equals(right);
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace DbmsWcfService.Models.Values
{
    [DataContract]
    public class ColorInvl : Value
    {
        [DataMember]
        public byte R { get; private set; }
        [DataMember]
        public byte G { get; private set; }
        [DataMember]
        public byte B { get; private set; }

        public new Types.ColorInvl Type
        {
            get
            {
                return (Types.ColorInvl)base.Type;
            }
        }

        private void ValidateRange(string name, byte val, byte min, byte max)
        {
            if (val < min || val > max)
                throw new ArgumentOutOfRangeException(string.Format("{0} = {1} but allowed range is [{2}..{3}]", name, val, min, max), (Exception)null);
        }

        private byte ParseByte(string name, string str, byte min, byte max)
        {
            try
            {
                byte val = byte.Parse(str);
                ValidateRange(name, val, min, max);
                return val;
            }
            catch
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} = {1} but allowed range is [{2}..{3}]", name, str, min, max), (Exception)null);
            }
        }

        private void Validate()
        {
            ValidateRange("R", R, Type.R1, Type.R2);
            ValidateRange("G", G, Type.G1, Type.G2);
            ValidateRange("B", B, Type.B1, Type.B2);
        }

        public ColorInvl(Types.ColorInvl type, byte r, byte g, byte b) : base(type)
        {
            R = r;
            G = g;
            B = b;
            Validate();
        }

        public ColorInvl(Types.ColorInvl type, string s) : base(type)
        {
            var numbers = Regex.Split(s, @"\D+").Where(n => !string.IsNullOrWhiteSpace(n)).ToArray();
            if (numbers.Length != 3)
                throw new FormatException("RGB-color should contain exactly 3 numbers");
            R = ParseByte("R", numbers[0], Type.R1, Type.R2);
            G = ParseByte("G", numbers[1], Type.G1, Type.G2);
            B = ParseByte("B", numbers[2], Type.B1, Type.B2);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
        }

        public ColorInvl(BinaryReader reader, Types.ColorInvl type)
            : this(type, reader.ReadByte(), reader.ReadByte(), reader.ReadByte()) { }

        public override string ToString()
        {
            return string.Format("(R={0}, G={1}, B={2})", R, G, B);
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) && obj is ColorInvl other)
            {
                return R == other.R && G == other.G && B == other.B;
            }
            return false;
        }
    }
}

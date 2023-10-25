using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace DbmsWcfService.Models.Values
{
    [DataContract]
    public class Color : Value
    {
        [DataMember]
        public byte R { get; private set; }
        [DataMember]
        public byte G { get; private set; }
        [DataMember]
        public byte B { get; private set; }

        public Color(Types.Color type, byte r, byte g, byte b) : base(type)
        {
            R = r;
            G = g;
            B = b;
        }

        private byte ParseByte(string name, string str)
        {
            try
            {
                byte val = byte.Parse(str);
                return val;
            }
            catch
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} = {1} but allowed range is [0..255]", name, str), (Exception)null);
            }
        }

        public Color(Types.Color type, string s) : base(type)
        {
            var numbers = Regex.Split(s, @"\D+").Where(n => !string.IsNullOrWhiteSpace(n)).ToArray();
            if (numbers.Length != 3)
                throw new FormatException("RGB-color must contain exactly 3 numbers (R, G, B)");
            R = ParseByte("R", numbers[0]);
            G = ParseByte("G", numbers[1]);
            B = ParseByte("B", numbers[2]);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
        }

        public Color(BinaryReader reader, Types.Color type)
            : this(type, reader.ReadByte(), reader.ReadByte(), reader.ReadByte()) { }

        public override string ToString() => $"(R={R}, G={G}, B={B})";

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && obj is Color other &&
                   R == other.R && G == other.G && B == other.B;
        }
    }
}

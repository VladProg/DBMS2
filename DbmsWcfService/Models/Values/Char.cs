﻿using System;
using System.IO;
using System.Runtime.Serialization;

namespace DbmsWcfService.Models.Values
{
    [DataContract]
    public class Char : Value
    {
        [DataMember]
        public char Val { get; private set; }

        public Char(Types.Char type, char val) : base(type)
        {
            Val = val;
        }

        public Char(Types.Char type, string s) : base(type)
        {
            try
            {
                Val = char.Parse(s);
            }
            catch
            {
                throw new FormatException("Char must contain a single character");
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(Val);
        }

        public Char(BinaryReader reader, Types.Char type) : this(type, reader.ReadChar()) { }

        public override string ToString()
        {
            return Val.ToString();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) && obj is Char)
            {
                Char other = (Char)obj;
                return Val == other.Val;
            }
            return false;
        }
    }
}

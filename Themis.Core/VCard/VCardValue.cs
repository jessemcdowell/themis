﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Themis.VCard
{
    public class VCardValue : VCardSimpleValue
    {
        public VCardValue()
        {
            Parameters = new VCardEntityList<VCardSimpleValue>();
        }

        public VCardValue(string name)
            : this()
        {
            Name = name;
        }

        public VCardValue(string name, string value)
            : this(name)
        {
            Value = value;
        }

        public VCardEntityList<VCardSimpleValue> Parameters { get; private set; }
    }
}

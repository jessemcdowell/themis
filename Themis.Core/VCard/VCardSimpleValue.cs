using System;
using System.Text;

namespace Themis.VCard
{
    public class VCardSimpleValue : VCardEntity
    {
        public VCardSimpleValue()
        { }

        public VCardSimpleValue(string name)
        {
            Name = name;
        }

        public VCardSimpleValue(string name, string value)
        {
            Name = name;
            Value = value;
        }


        public string Value { get; set; }


    }
}

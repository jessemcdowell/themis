using System;
using System.Text;

namespace Themis.Calendar.VCard
{
    public class VCardSimpleValue : VCardEntity
    {
        public VCardSimpleValue()
        { }

        public VCardSimpleValue(string name)
        {
            Name = name;
        }

        public VCardSimpleValue(string name, string escapedValue)
        {
            Name = name;
            EscapedValue = escapedValue;
        }

        /// <summary>
        /// The escaped and encoded form of the value.
        /// </summary>
        public string EscapedValue { get; set; }
    }
}

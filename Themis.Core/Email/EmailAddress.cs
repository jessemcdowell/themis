using System;
using System.Text.RegularExpressions;

namespace Themis.Email
{
    /// <summary>
    /// A simple representation of an email address
    /// </summary>
    public class EmailAddress
    {
        public EmailAddress(string address)
            : this(address, null)
        { }

        public EmailAddress(string address, string name)
        {
            if (String.IsNullOrEmpty(address))
                throw new ArgumentException("You must specify an email address", "address");

            Address = address;
            Name = String.IsNullOrWhiteSpace(name) ? null : name;
        }

        public string Address { get; private set; }

        public string Name { get; private set; }

        public override string ToString()
        {
            if (Name == null)
                return Address;
            else
                return String.Format("\"{0}\" <{1}>", Name.Replace('"', '\''), Address);
        }

    }
}

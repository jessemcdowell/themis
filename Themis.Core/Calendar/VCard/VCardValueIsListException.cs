using System;

namespace Themis.Calendar.VCard
{
    /// <summary>
    /// Thrown when encountering a list separator while parsing a value.
    /// </summary>
    public class VCardValueIsListException : Exception
    {
        private const string DefaultMessage = "A list separator was encountered";

        public VCardValueIsListException()
            : base(DefaultMessage)
        { }

    }
}

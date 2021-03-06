﻿using System;

namespace Themis.Calendar.VCard
{
    /// <summary>
    /// Indicates a problem parsing VCard data
    /// </summary>
    public class InvalidVCardFormatException : Exception
    {
        public InvalidVCardFormatException(string message, string content)
            : this(message, content, null)
        { }

        public InvalidVCardFormatException(string message, string content, Exception innerException)
            : base(message, innerException)
        {
            Content = content;
        }

        /// <summary>
        /// The content and it's surroundings where the problem appears.
        /// </summary>
        public string Content { get; private set; }

        public override string ToString()
        {
            string text = base.ToString();

            if (!String.IsNullOrEmpty(Content))
                text += "\r\nContent: " + Content;

            return text;
        }
    }
}

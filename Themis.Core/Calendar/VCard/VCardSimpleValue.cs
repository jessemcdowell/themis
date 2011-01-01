using System;
using System.Text;

namespace Themis.Calendar.VCard
{
    public class VCardSimpleValue : VCardEntity
    {
        private string _escapedValue;
        private bool? _isValueList = null;

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
        public string EscapedValue 
        {
            get { return _escapedValue; }
            set
            {
                _escapedValue = value;
                _isValueList = null;
            }
        }

        /// <summary>
        /// Indicates if EscapedValue is a value list
        /// </summary>
        public bool IsValueList
        {
            get
            {
                bool? value = _isValueList;
                if (value.HasValue)
                    return value.Value;

                _isValueList = value = GetIsValueList();
                return value.Value;
            }
        }

        private bool GetIsValueList()
        {
            string inputText = EscapedValue;
            if (String.IsNullOrEmpty(inputText))
                return false;

            int index = 0;
            while (index < inputText.Length)
            {
                char c = inputText[index];

                if (c == ',')
                    return true;
                if (c == '\\') // escape character
                {
                    // skip another character, and make sure there is more at the end
                    index++;
                    if (index >= inputText.Length)
                        throw new InvalidVCardFormatException("Line ends with escape character", inputText);
                }

                index++;
            }

            return false;
        }

                
        /// <summary>
        /// Parses a value list into a list of values
        /// </summary>
        /// <returns></returns>
        public VCardEntityList<VCardSimpleValue> GetListValues()
        {
            VCardEntityList<VCardSimpleValue> output = new VCardEntityList<VCardSimpleValue>();

            string inputText = EscapedValue;

            if (String.IsNullOrEmpty(inputText))
            {
                output.Add(new VCardSimpleValue("1", String.Empty));
                return output;
            }

            int count = 0;
            int lastIndex = -1;
            int index = 0;
            while (index < inputText.Length)
            {
                char c = inputText[index];

                if (c == ',')
                {
                    count++;
                    string content = inputText.Substring(lastIndex + 1, index - lastIndex - 1);
                    output.Add(new VCardSimpleValue(count.ToString(), content));

                    lastIndex = index;
                }
                if (c == '\\') // escape character
                {
                    // skip another character, and make sure there is more at the end
                    index++;
                    if (index >= inputText.Length)
                        throw new InvalidVCardFormatException("Line ends with escape character", inputText);
                }

                index++;
            }

            // add everything after the last separator
            count++;
            output.Add(new VCardSimpleValue(count.ToString(), inputText.Substring(lastIndex + 1)));

            // return the list
            return output;
        }

        /// <summary>
        /// Parses and returns an escaped text value as a string
        /// </summary>
        /// <returns>The contents of the value with no inline escaping</returns>
        public string GetText()
        {
            string inputText = EscapedValue;

            if (String.IsNullOrEmpty(inputText))
                return String.Empty;

            // process the text into a string builder. The result can't be bigger than the original, but it could be smaller
            StringBuilder output = new StringBuilder(inputText.Length);

            int index = 0;
            while (index < inputText.Length)
            {
                char c = inputText[index];

                if (c == '\\')
                {
                    if ((index + 1) >= inputText.Length)
                        throw new InvalidVCardFormatException("Text line ends with escape character", inputText);

                    c = inputText[index + 1];
                    if (c == 'n' || c == 'N')
                        output.AppendLine();
                    else
                        output.Append(c);

                    index += 2;
                }
                else if (c == ',')
                {
                    throw new VCardValueIsListException();
                }
                else
                {
                    output.Append(c);
                    index++;
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Parses the escaped value as a DateTime
        /// </summary>
        /// <returns>The value as a date time with a kind either in UTC (where time zone is specified) or unspecified (for local)</returns>
        public DateTime GetDateTime()
        {
            string inputText = EscapedValue;
            string text = inputText;

            if (String.IsNullOrEmpty(text))
                throw new InvalidVCardFormatException("No value to parse", inputText);


            // if it contains dashes for the date, remove them
            if ((text.Length > 4) && (text[4] == '-'))
                text = text.Substring(0, 4) + text.Substring(5);
            if ((text.Length > 6) && (text[6] == '-'))
                text = text.Substring(0, 6) + text.Substring(7);

            // if it contains time separators, remove them
            if ((text.Length > 11) && (text[11] == ':'))
                text = text.Substring(0, 11) + text.Substring(12);
            if ((text.Length > 13) && (text[13] == ':'))
                text = text.Substring(0, 13) + text.Substring(14);
            if ((text.Length > 15) && (text[15] == '.'))
                text = text.Substring(0, 15) + text.Substring(16);

            // make sure three isn't an offset
            if (text.Contains("-"))
                throw new InvalidVCardFormatException("DateTime with offset specified not supported", inputText);

            // make sure it isn't a list
            if (text.Contains(","))
                throw new VCardValueIsListException();


            // get the parts of the date
            if (text.Length < 8)
                throw new InvalidVCardFormatException("DateTime does not contain a full date", inputText);

            int year;
            int month;
            int day;
            try
            {
                year = Int32.Parse(text.Substring(0, 4), System.Globalization.CultureInfo.InvariantCulture);
                month = Int32.Parse(text.Substring(4, 2), System.Globalization.CultureInfo.InvariantCulture);
                day = Int32.Parse(text.Substring(6, 2), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                throw new InvalidVCardFormatException("Date value not numeric", inputText, ex);
            }

            // get the time part if it exists
            int hour = 0;
            int minute = 0;
            int second = 0;
            DateTimeKind kind = DateTimeKind.Unspecified;

            if ((text.Length > 8) && (text[8] == 'T'))
            {
                if (text.Length < 13)
                    throw new InvalidVCardFormatException("DateTime does has a time marker but not contain a hours and minutes", inputText);

                try
                {
                    hour = Int32.Parse(text.Substring(9, 2), System.Globalization.CultureInfo.InvariantCulture);
                    minute = Int32.Parse(text.Substring(11, 2), System.Globalization.CultureInfo.InvariantCulture);

                    if (text.Length >= 15)
                        second = Int32.Parse(text.Substring(13, 2), System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    throw new InvalidVCardFormatException("Time value not numeric", inputText, ex);
                }

                // see if it's specified in UTC
                if (text.EndsWith("Z", StringComparison.InvariantCultureIgnoreCase))
                    kind = DateTimeKind.Utc;
            }


            // return the value
            return new DateTime(year, month, day, hour, minute, second, kind);
        }
    }
}

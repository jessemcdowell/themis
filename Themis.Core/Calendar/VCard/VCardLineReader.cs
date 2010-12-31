using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Themis.Calendar.VCard
{
    /// <summary>
    /// Reads an unwrapped line of VCard data into it's separate parts
    /// </summary>
    public class VCardLineReader
    {
        private readonly string _line;
        private int _currentIndex = 0;

        const char ValueDelimeter = ':';
        const char ParameterDelimiter = ';';
        const char ParameterNameValueSeparator = '=';

        public VCardLineReader(string entireLine)
        {
            if (String.IsNullOrEmpty(entireLine))
                throw new ArgumentNullException("entireLine");
            _line = entireLine;

            ParseBeginning();
        }

        public string EntireLine 
        {
            get { return _line; }
        }

        public string Name { get; private set; }

        public VCardLineType Type { get; private set; }

        /// <summary>
        /// figure out the name and type of the entity the line represents.
        /// </summary>
        private void ParseBeginning()
        {
            _currentIndex = _line.IndexOfAny(new char[] {ParameterDelimiter, ValueDelimeter});
            if (_currentIndex == -1)
                throw new InvalidVCardFormatException("Line does not contain a name/value delimiter", EntireLine);
            if (_currentIndex == 0)
                throw new InvalidVCardFormatException("Name must be at least one character", EntireLine);
             
            Name = _line.Substring(0, _currentIndex);

            // figure out if it's a beginning or ending by the name
            if (String.Equals(Name, "BEGIN", StringComparison.InvariantCultureIgnoreCase))
                Type = VCardLineType.GroupBeginning;
            else if (String.Equals(Name, "End", StringComparison.InvariantCultureIgnoreCase))
                Type = VCardLineType.GroupEnding;
            else
            {
                Type = VCardLineType.Value;
                return;
            }

            // if it's a beginning or ending, the remainder should be a group name
            if (GetCurrentDelimiterType() == ParameterDelimiter)
                throw new InvalidVCardFormatException("Group " + (Type == VCardLineType.GroupBeginning ? "Beginning" : "Ending") + " cannot contain parameters", EntireLine);

            Name = _line.Substring(_currentIndex + 1);
        }

        private void AssertIsValueLine()
        {
            if (Type != VCardLineType.Value)
                throw new InvalidOperationException("You cannot read parameters or values of a group beginning or ending");
        }

        /// <summary>
        /// Attempts to read the next parameter from the value line. Returns true if one is present, or false if there are no more.
        /// </summary>
        /// <param name="name">Returns the name of the parameter</param>
        /// <param name="escapedValue">Returns the escaped value of the parameter</param>
        /// <returns>True if another parameter was found and read</returns>
        public bool ReadParameter(out string name, out string escapedValue)
        {
            AssertIsValueLine();

            // see if we're at the end of the parameters
            if (GetCurrentDelimiterType() == ValueDelimeter)
            {
                name = null;
                escapedValue = null;
                return false;
            }

            // find the boundaries of the parameter
            int parameterSeparatorIndex;
            int nextDelimeterIndex;
            FindParameterBoundaries(out parameterSeparatorIndex, out nextDelimeterIndex);

            // return the parts
            name = _line.Substring(_currentIndex + 1, parameterSeparatorIndex - _currentIndex - 1);
            escapedValue = _line.Substring(parameterSeparatorIndex + 1, nextDelimeterIndex - parameterSeparatorIndex - 1);

            // advance the current index to the next delimiter
            _currentIndex = nextDelimeterIndex;
            return true;
        }


        /// <summary>
        /// Retrieves the escaped value of the line once all parameters have been read.
        /// </summary>
        /// <returns>The escaped value from the line</returns>
        public string GetEscapedValue()
        {
            AssertIsValueLine();
            if (_line[_currentIndex] == ParameterDelimiter)
                throw new InvalidOperationException("You must read all parameters before you can read the value of a line");

            System.Diagnostics.Debug.Assert(GetCurrentDelimiterType() == ValueDelimeter, "Position not at value delimiter");

            return _line.Substring(_currentIndex + 1);
        }

        private char GetCurrentDelimiterType()
        {
            return _line[_currentIndex];
        }

        private void FindParameterBoundaries(out int parameterSeparatorIndex, out int nextDelimeterIndex)
        {
            // find the parameters name / value separator
            parameterSeparatorIndex = _currentIndex + 1;
            while (true)
            {
                // if we hit the end of the line, then there was no separator between this parameter and the end of the line
                if (parameterSeparatorIndex >= _line.Length)
                    throw new InvalidVCardFormatException("Line does not contain a value delimiter", EntireLine);

                // if we encounter an escape character, skip anything that follows it.
                // if we find a delimiter, then there is no name / value separator
                // if we find what we're looking for, then exit the loop
                char c = _line[parameterSeparatorIndex];
                if (c == '\\')
                    parameterSeparatorIndex++;
                else if ((c == ParameterDelimiter) || (c == ValueDelimeter))
                    throw new InvalidVCardFormatException("Paremeter does not contain a name / value separator", _line.Substring(_currentIndex + 1, parameterSeparatorIndex - _currentIndex - 1));
                else if (c == ParameterNameValueSeparator)
                    break;

                parameterSeparatorIndex++;
            }

            // find the next delimiter
            nextDelimeterIndex = parameterSeparatorIndex + 1;
            while (true)
            {
                // if we hit the end of the line, then there was no separator between this parameter and the end of the line
                if (nextDelimeterIndex >= _line.Length)
                    throw new InvalidVCardFormatException("Line does not contain a value delimiter", EntireLine);

                // if we encounter an escape character, skip anything that follows it.
                // if we find what we're looking for, then exit the loop
                char c = _line[nextDelimeterIndex];
                if (c == '\\')
                    nextDelimeterIndex++;
                else if ((c == ParameterDelimiter) || (c == ValueDelimeter))
                    break;

                nextDelimeterIndex++;
            }
        }

    }
}

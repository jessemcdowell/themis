using System;
using System.Text;
using System.IO;

namespace Themis.VCard
{
    public class VCardReader
    {
        /// <summary>
        /// Retrieves the first entity from the stream.
        /// If the entity is a group all contained children will be included.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>The first entity from the stream reader, or null if there are none remaining</returns>
        public VCardEntity ReadEntity(StreamReader reader)
        {
            string line = ReadUnfoldedLine(reader);
            if (line == null)
                return null;

            // see if the line begins a new group
            string name;
            if (GetIsGroupBeginning(line, out name))
            {
                VCardGroup group = new VCardGroup() { Name = name };



                return group;
            }



            throw new NotImplementedException(); 
        }

        /// <summary>
        /// Checks if the line is the beginning of a group, returning the name of the group if it is.
        /// </summary>
        internal bool GetIsGroupBeginning(string line, out string name)
        {
            if (line.StartsWith("BEGIN:", StringComparison.InvariantCultureIgnoreCase))
            {
                name = line.Substring(6);
                return true;
            }
            else if (line.StartsWith("BEGIN;", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidVCardFormatException("Group beginning with parameters are not supported", line);
            }
            else
            {
                name = null;
                return false;
            }
        }

        /// <summary>
        /// Checks if the line is the ending of a group, returning the name of the group if it is.
        /// </summary>
        internal bool GetIsGroupEnding(string line, out string name)
        {
            if (line.StartsWith("END:", StringComparison.InvariantCultureIgnoreCase))
            {
                name = line.Substring(4);
                return true;
            }
            else if (line.StartsWith("END;", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidVCardFormatException("Group ending with parameters are not supported", line);
            }
            else
            {
                name = null;
                return false;
            }
        }

        /// <summary>
        /// Reads the next complete line from the stream, returning null if the end is reached.
        /// </summary>
        /// <param name="reader">The stream of text to read.</param>
        /// <returns>The complete unfolded line with no trailing CrLf</returns>
        internal string ReadUnfoldedLine(StreamReader reader)
        {
            string text = reader.ReadLine();
            if (text == null)
                return null;

            // read any following lines. Folded lines will begin with a single tab character or a single space 
            int nextLineChar;
            while (((nextLineChar = reader.Peek()) == 9) || (nextLineChar == 32))
                text += reader.ReadLine().Substring(1);

            return text;
        }
    }
}

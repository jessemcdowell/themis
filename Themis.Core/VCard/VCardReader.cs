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
            string unfoldedLine = ReadUnfoldedLine(reader);
            if (unfoldedLine == null)
                return null;

            VCardLineReader line = new VCardLineReader(unfoldedLine);
            return GetEntityFromLine(reader, line);
        }

        private VCardEntity GetEntityFromLine(StreamReader reader, VCardLineReader line)
        {
            switch (line.Type)
            {
                case VCardLineType.Value:
                    return GetValueFromLine(line);

                case VCardLineType.GroupEnding:
                    throw new InvalidVCardFormatException("Encountered unexpected group ending " + line.Name, line.EntireLine);

                case VCardLineType.GroupBeginning:
                    VCardGroup group = new VCardGroup(line.Name);
                    ReadAllGroupChildren(reader, group);
                    return group;

                default:
                    throw new NotSupportedException("Unknown Line Type " + line.Type.ToString());
            }
        }

        private void ReadAllGroupChildren(StreamReader reader, VCardGroup group)
        {
            while (true)
            {
                string unfoldedLine = ReadUnfoldedLine(reader);
                if (unfoldedLine == null)
                    throw new InvalidVCardFormatException("Encountered end of VCard content before ending of group " + group.Name, null);

                VCardLineReader line = new VCardLineReader(unfoldedLine);

                if (line.Type == VCardLineType.GroupEnding)
                {
                    if (!String.Equals(group.Name, line.Name, StringComparison.InvariantCultureIgnoreCase))
                        throw new InvalidVCardFormatException("Encountered end of Group " + line.Name + " before current group " + group.Name, line.EntireLine);
                    break;
                }

                VCardEntity child = GetEntityFromLine(reader, line);
                group.Children.Add(child);
            }
        }

        private VCardValue GetValueFromLine(VCardLineReader line)
        {
            VCardValue value = new VCardValue(line.Name);

            string paramName;
            string paramEscapedValue;
            while (line.ReadParameter(out paramName, out paramEscapedValue))
                value.Parameters.Add(new VCardSimpleValue(paramName, paramEscapedValue)); 

            value.EscapedValue = line.GetEscapedValue();

            return value;
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

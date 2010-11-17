using System;
using System.Text;
using System.IO;

namespace Themis.VCard
{
    public class VCardReader
    {


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

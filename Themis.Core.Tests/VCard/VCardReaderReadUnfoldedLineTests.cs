using System;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace Themis.VCard
{
    [TestFixture]
    public class VCardReaderReadUnfoldedLineTests
    {
        private string CallReadUnfoldedLine(string input)
        {
            var encoding = Encoding.UTF8;
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(input), false))
            using (StreamReader sr = new StreamReader(ms, encoding))
            {
                VCardReader vcr = new VCardReader();
                return vcr.ReadUnfoldedLine(sr);
            }
        }

        [Test]
        public void Simple_Line()
        {
            const string input = "DTSTART:20101115T213000Z\r\n";
            const string expected = "DTSTART:20101115T213000Z";

            string actual = CallReadUnfoldedLine(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void First_Line_Only()
        {
            const string input = "DTSTART:20101115T213000Z\r\nDTEND:20101115T230000Z\r\n";
            const string expected = "DTSTART:20101115T213000Z";

            string actual = CallReadUnfoldedLine(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void First_Line_Only_With_Folded_Lines()
        {
            const string input = "DTSTART:20101\r\n 115T213000Z\r\nDTEND:20101115T230000Z\r\n";
            const string expected = "DTSTART:20101115T213000Z";

            string actual = CallReadUnfoldedLine(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Simple_Line_Without_CrLf()
        {
            const string input = "DTSTART:20101115T213000Z";
            const string expected = "DTSTART:20101115T213000Z";

            string actual = CallReadUnfoldedLine(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Line_Folded_With_Spaces()
        {
            const string input = "DTSTA\r\n RT:201\r\n 01115T2\r\n 13000Z\r\n";
            const string expected = "DTSTART:20101115T213000Z";

            string actual = CallReadUnfoldedLine(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Line_Folded_With_Tabs()
        {
            const string input = "DTSTA\r\n\tRT:201\r\n\t01115T2\r\n\t13000Z\r\n";
            const string expected = "DTSTART:20101115T213000Z";

            string actual = CallReadUnfoldedLine(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Line_Folded_With_Spaces_And_Inline_Space_At_Start_Of_Folded_Line()
        {
            const string input = "TEXT:Hello\r\n  There Bob\r\n";
            const string expected = "TEXT:Hello There Bob";

            string actual = CallReadUnfoldedLine(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Line_Folded_With_Tabs_And_Inline_Tab_At_Start_Of_Folded_Line()
        {
            const string input = "TEXT:Name\r\n\t\tValue\r\n";
            const string expected = "TEXT:Name\tValue";

            string actual = CallReadUnfoldedLine(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void End_Of_Stream_Returns_Null()
        {
            string actual;
            using (StreamReader sr = new StreamReader(new MemoryStream()))
            {
                VCardReader vcr = new VCardReader();
                actual = vcr.ReadUnfoldedLine(sr);
            }

            Assert.IsNull(actual);
        }
    }
}

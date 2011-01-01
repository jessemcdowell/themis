using System;
using NUnit.Framework;

namespace Themis.Calendar.VCard
{
    [TestFixture]
    public class TextValueEncodingTests
    {
        private const string Name = "N";

        [Test]
        public void Rfc2425_Text_Example_1()
        {
            const string input = @"this is a text value";
            const string expected = "this is a text value";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            string actual = sv.GetText();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Rfc2425_Text_Example_2_With_Multiple_Values()
        {
            const string input = @"this is one value,this is another";
            string[] expected = new string[] { "this is one value", "this is another" };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);

            var actual = sv.GetListValues();

            Assert.AreEqual(expected.GetLength(0), actual.Count, "Number of items");
            Assert.AreEqual(expected[0], actual[0].GetText());
            Assert.AreEqual(expected[1], actual[1].GetText());
        }

        [Test]
        public void Rfc2425_Text_Example_3()
        {
            const string input = @"this is a single value\, with a comma encoded";
            const string expected = "this is a single value, with a comma encoded";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            string actual = sv.GetText();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Rfc2425_Text_Example_4_With_Multiple_Lines()
        {
            const string input = @"Mythical Manager\nHyjinx Software Division\nBabsCo\, Inc.\n";
            const string expected = "Mythical Manager\r\nHyjinx Software Division\r\nBabsCo, Inc.\r\n";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            string actual = sv.GetText();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Rfc2445_Text_Example_1()
        {
            const string input = @"Project XYZ Final Review\nConference Room - 3B\nCome Prepared.";
            const string expected = "Project XYZ Final Review\r\nConference Room - 3B\r\nCome Prepared.";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            string actual = sv.GetText();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Text line ends with escape character")]
        public void Line_Ending_With_An_Escape_Character_Fails()
        {
            const string input = @"Hello\";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            sv.GetText();
        }

        [Test]
        public void Line_With_Escaped_Character_At_Beginning()
        {
            const string input = @"\,Hello";
            const string expected = ",Hello";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            string actual = sv.GetText();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Line_With_Escaped_Character_At_Ending_Fails()
        {
            const string input = @"Hello\,";
            const string expected = "Hello,";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            string actual = sv.GetText();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(VCardValueIsListException))]
        public void List_Separator_Causes_Exception()
        {
            const string input = @"Hello,Hi";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            sv.GetText();
        }
    }
}

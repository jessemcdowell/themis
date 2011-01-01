using System;
using NUnit.Framework;

namespace Themis.Calendar.VCard
{
    [TestFixture]
    public class SimpleValueEncodingTests
    {
        private const string Name = "N";

        [Test]
        [TestCase("TRUE")]
        [TestCase("true")]
        [TestCase("TrUe")]
        public void Parse_Boolean_True(string input)
        {
            const bool expected = true;

            VCardSimpleValue vs = new VCardSimpleValue(Name, input);
            bool actual = vs.GetBoolean();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("FALSE")]
        [TestCase("false")]
        [TestCase("fAlSe")]
        public void Parse_Boolean_False(string input)
        {
            const bool expected = false;

            VCardSimpleValue vs = new VCardSimpleValue(Name, input);
            bool actual = vs.GetBoolean();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Boolean is not an expected value")]
        public void Parse_Invalid_Boolean_Fails()
        {
            string input = "perhaps";

            VCardSimpleValue vs = new VCardSimpleValue(Name, input);
            vs.GetBoolean();
        }

        [Test]
        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("+1", 1)]
        [TestCase("-1", -1)]
        [TestCase("2147483647", 2147483647)]
        [TestCase("+2147483647", 2147483647)]
        [TestCase("-2147483648", -2147483648)]
        public void Parse_Integer(string input, int expected)
        {
            VCardSimpleValue vs = new VCardSimpleValue(Name, input);
            int actual = vs.GetInteger();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage="Integer is not a valid format")]
        [TestCase("lots")]
        [TestCase("+-5")]
        [TestCase("3.8")]
        [TestCase(@"4\,123")]
        public void Parse_Invalid_Integer_Fails(string input)
        {
            VCardSimpleValue vs = new VCardSimpleValue(Name, input);
            
            vs.GetInteger();
        }
    }
}

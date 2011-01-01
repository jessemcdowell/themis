using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Themis.Calendar.VCard
{
    [TestFixture]
    public class ValueListEncodingTests
    {
        public const string Name = "N";

        public void AssertEncodedValueList(IEnumerable<string> expected, IEnumerable<VCardSimpleValue> actual)
        {
            CollectionAssert.AreEqual(expected, from a in actual select a.EscapedValue, StringComparer.InvariantCulture);
        }

        [Test]
        public void List_With_Two_Items()
        {
            const string input = @"Hello,There";
            string[] expected = new[] { "Hello", "There" };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            var actual = sv.GetListValues();

            AssertEncodedValueList(expected, actual);
        }

        [Test]
        public void List_With_Five_Items()
        {
            const string input = @"One,Two,Three,Four,Five";
            string[] expected = new[] { "One", "Two", "Three", "Four", "Five" };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            var actual = sv.GetListValues();

            AssertEncodedValueList(expected, actual);
        }

        [Test]
        public void No_Separators_Should_Return_A_Single_Value()
        {
            const string input = @"Hello There";
            string[] expected = new[] { "Hello There" };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            var actual = sv.GetListValues();

            AssertEncodedValueList(expected, actual);
        }

        [Test]
        public void Escaped_Separators_Should_Not_Separate_Values()
        {
            const string input = @"Hello\,There";
            string[] expected = new[] { @"Hello\,There" };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            var actual = sv.GetListValues();

            AssertEncodedValueList(expected, actual);
        }

        [Test]
        public void A_Null_String_Should_Return_A_Single_Value()
        {
            const string input = null;
            string[] expected = new [] { "" };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            var actual = sv.GetListValues();

            AssertEncodedValueList(expected, actual);
        }

        [Test]
        public void Separators_At_Beginning_And_End_Should_Result_In_Empty_Items()
        {
            const string input = @",Hello,There,";
            string[] expected = new[] { "", "Hello", "There", "" };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            var actual = sv.GetListValues();

            AssertEncodedValueList(expected, actual);
        }

        [Test]
        public void The_Name_Of_Each_Returned_Value_Should_Be_The_Index_Starting_With_1()
        {
            const string input = @"Hello,There,Bob";
            string[] expected = new[] { "1", "2", "3" };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            var actual = sv.GetListValues();

            CollectionAssert.AreEqual(expected, from a in actual select a.Name);
        }


        [Test]
        [TestCase("Hello,There")]
        [TestCase("Hello,There,Bob")]
        [TestCase(",Hello")]
        [TestCase("Hello,")]
        public void Identify_Value_That_Is_A_List(string input)
        {
            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            bool actual = sv.IsValueList;

            Assert.IsTrue(actual, input);
        }

        [Test]
        [TestCase(@"Hello There")]
        [TestCase(@"Hello\,There")]
        [TestCase(@"Hello\,There\,Bob")]
        [TestCase(@"\,Hello")]
        [TestCase(@"Hello\,")]
        public void Identify_Value_That_Is_Not_A_List(string input)
        {
            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            bool actual = sv.IsValueList;

            Assert.IsFalse(actual, input);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Line ends with escape character")]
        public void Fail_To_Identify_Value_With_Escape_At_Ending()
        {
            const string input = @"Hello\";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            
            bool actual = sv.IsValueList;
        }


        [Test]
        public void Change_IsValueList_When_EscapedValue_Changes()
        {
            VCardSimpleValue sv = new VCardSimpleValue(Name, "One");
            Assert.IsFalse(sv.IsValueList, "1");

            sv.EscapedValue = "Two,Three,Four";
            Assert.IsTrue(sv.IsValueList, "2");

            sv.EscapedValue = "Five";
            Assert.IsFalse(sv.IsValueList, "3");
        }
    }
}

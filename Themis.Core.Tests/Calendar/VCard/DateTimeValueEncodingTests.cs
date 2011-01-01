using System;
using NUnit.Framework;

namespace Themis.Calendar.VCard
{
    [TestFixture]
    public class DateTimeValueEncodingTests
    {
        private const string Name = "N";

        [Test]
        [ExpectedException(typeof(AssertionException), MatchType=MessageMatch.Contains, ExpectedMessage="DateTime Value")]
        public void Assert_DateTime_With_Different_Time_Fails()
        {
            DateTime d1 = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Utc);
            DateTime d2 = new DateTime(1998, 01, 18, 23, 00, 01, DateTimeKind.Utc);

            AssertDateTime(d1, d2);
        }

        [Test]
        [ExpectedException(typeof(AssertionException), MatchType=MessageMatch.Contains, ExpectedMessage="DateTime Kind")]
        public void Assert_DateTime_With_Different_Kinds_Fails()
        {
            DateTime d1 = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Unspecified);
            DateTime d2 = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Utc);

            AssertDateTime(d1, d2);            
        }

        public static void AssertDateTime(DateTime expected, DateTime actual)
        {
            // it seems that this statement won't catch differences in kinds
            Assert.AreEqual(expected, actual, "DateTime Value");
            Assert.AreEqual(expected.Kind, actual.Kind, "DateTime Kind");
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage="DateTime with offset specified not supported")]
        public void Rfc2445_Invalid_Example_1()
        {
            const string input = "19980119T230000-0800";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            sv.GetDateTime();
        }

        [Test]
        public void Rfc2445_Local_Example_2()
        {
            const string input = "19980118T230000";
            DateTime expected = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Unspecified);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Rfc2445_UTC_Example_3()
        {
            const string input = "19980119T070000Z";
            DateTime expected = new DateTime(1998, 01, 19, 07, 00, 00, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        //[Test]
        //[Ignore("Not supported")]
        //public void Rfc2445_Local_With_Timezone_Example_4()
        //{
        //    const string input = "19980119T020000";
        //    VCardSimpleValue inputParameter = new VCardSimpleValue("TZID", "US-Eastern");
        //
        //    DateTime expected = new DateTime(1998, 01, 19, 02, 00, 00, DateTimeKind.Unspecified);
        //
        //    VCardValue v = new VCardValue(Name, input);
        //    v.Parameters.Add(inputParameter);
        //
        //    DateTime actual = v.GetDateTime();
        //
        //    AssertDateTime(expected, actual);
        //}

        [Test]
        public void Rfc2445_Local_Example_5()
        {
            const string input = "19970714T133000";
            DateTime expected = new DateTime(1997, 07, 14, 13, 30, 00, DateTimeKind.Unspecified);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Rfc2445_UTC_Example_6()
        {
            const string input = "19970714T173000Z";
            DateTime expected = new DateTime(1997, 07, 14, 17, 30, 00, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Rfc2425_DateTime_Example_1_With_Dashes()
        {
            const string input = "1996-10-22T14:00:00Z";
            DateTime expected = new DateTime(1996, 10, 22, 14, 00, 00, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Rfc2425_DateTime_Example_2_With_Dashes()
        {
            const string input = "1996-08-11T12:34:56Z";
            DateTime expected = new DateTime(1996, 08, 11, 12, 34, 56, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Rfc2425_DateTime_Example_3()
        {
            const string input = "19960811T123456Z";
            DateTime expected = new DateTime(1996, 08, 11, 12, 34, 56, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Rfc2425_Date_Example_1_With_Dashes()
        {
            const string input = "1985-04-12";
            DateTime expected = new DateTime(1985, 04, 12, 00, 00, 00, DateTimeKind.Unspecified);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Rfc2425_Date_Example_3()
        {
            const string input = "19850412";
            DateTime expected = new DateTime(1985, 04, 12, 00, 00, 00, DateTimeKind.Unspecified);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Outlook_Utc_DateTime()
        {
            const string input = "20101114T222754Z";
            DateTime expected = new DateTime(2010, 11, 14, 22, 27, 54, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void Rfc2425_Example_4_Multiple_Values()
        {
            const string input = "1996-10-22T14:00:00Z,1996-08-11T12:34:56Z";
            DateTime[] expected = new DateTime[] {
                new DateTime(1996, 10, 22, 14, 00, 00, DateTimeKind.Utc), 
                new DateTime(1996, 08, 11, 12, 34, 56, DateTimeKind.Utc),
            };

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);

            var actual = sv.GetListValues();

            Assert.AreEqual(expected.GetLength(0), actual.Count, "Number of items");
            AssertDateTime(expected[0], actual[0].GetDateTime());
            AssertDateTime(expected[1], actual[1].GetDateTime());
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage="DateTime does not contain a full date")]
        public void Incomplete_Date_Fails()
        {
            const string input = "1998010";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            sv.GetDateTime();
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Date value not numeric")]
        public void DateTime_With_Letters_In_Date_Fails()
        {
            const string input = "201A1114T222754Z";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            sv.GetDateTime();
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Time value not numeric")]
        public void DateTime_With_Letters_In_Time_Fails()
        {
            const string input = "20101114TA22754Z";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            sv.GetDateTime();
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "DateTime has a time marker but not contain a hours and minutes")]
        public void DateTime_With_Time_Marker_But_No_Time_Fails()
        {
            const string input = "20101114T";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            sv.GetDateTime();
        }

        [Test]
        public void DateTime_Hours_Minutes_Local()
        {
            const string input = "20101114T2227";
            DateTime expected = new DateTime(2010, 11, 14, 22, 27, 00, DateTimeKind.Unspecified);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void DateTime_Hours_Minutes_Utc()
        {
            const string input = "20101114T2227Z";
            DateTime expected = new DateTime(2010, 11, 14, 22, 27, 00, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void DateTime_With_Fraction_Of_Second_Local()
        {
            const string input = "20101114T222754123";
            DateTime expected = new DateTime(2010, 11, 14, 22, 27, 54, DateTimeKind.Unspecified);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void DateTime_With_Fraction_Of_Second_Utc()
        {
            const string input = "20101114T222754123Z";
            DateTime expected = new DateTime(2010, 11, 14, 22, 27, 54, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void DateTime_With_Fraction_Of_Second_And_Separators_Local()
        {
            const string input = "2010-11-14T22:27:54.123";
            DateTime expected = new DateTime(2010, 11, 14, 22, 27, 54, DateTimeKind.Unspecified);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        public void DateTime_With_Fraction_Of_Second_And_Separators_Utc()
        {
            const string input = "2010-11-14T22:27:54.123Z";
            DateTime expected = new DateTime(2010, 11, 14, 22, 27, 54, DateTimeKind.Utc);

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            DateTime actual = sv.GetDateTime();

            AssertDateTime(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(VCardValueIsListException))]
        public void List_Separator_Causes_Exception()
        {
            const string input = "19961022T140000Z,19960811T123456Z";

            VCardSimpleValue sv = new VCardSimpleValue(Name, input);
            sv.GetDateTime();
        }
    }
}

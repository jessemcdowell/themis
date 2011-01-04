using System;
using NUnit.Framework;

namespace Themis
{
    [TestFixture]
    public class DateTimeAssertTests
    {
        [Test]
        [ExpectedException(typeof(AssertionException), MatchType = MessageMatch.Contains, ExpectedMessage = "DateTime Value")]
        public void Assert_DateTime_With_Different_Time_Fails()
        {
            DateTime d1 = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Utc);
            DateTime d2 = new DateTime(1998, 01, 18, 23, 00, 01, DateTimeKind.Utc);

            DateTimeAssert.AreEqual(d1, d2);
        }

        [Test]
        [ExpectedException(typeof(AssertionException), MatchType = MessageMatch.Contains, ExpectedMessage = "DateTime Kind")]
        public void Assert_DateTime_With_Different_Kinds_Fails()
        {
            DateTime d1 = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Unspecified);
            DateTime d2 = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Utc);

            DateTimeAssert.AreEqual(d1, d2);
        }

        [Test]
        public void Assert_DateTime_With_Same_Value_And_Kind_Does_Not_Fail()
        {
            DateTime d1 = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Unspecified);
            DateTime d2 = new DateTime(1998, 01, 18, 23, 00, 00, DateTimeKind.Unspecified);

            DateTimeAssert.AreEqual(d1, d2);
        }

    }
}

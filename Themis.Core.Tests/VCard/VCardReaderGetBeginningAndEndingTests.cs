using System;
using NUnit.Framework;

namespace Themis.VCard
{
    [TestFixture]
    public class VCardReaderGetBeginningAndEndingTests
    {
        [Test]
        public void Get_Beginning_Lower_Case()
        {
            const string input = "begin:vcard";
            const string expectedName = "vcard";

            string actualName;

            VCardReader vcr = new VCardReader();
            bool result = vcr.GetIsGroupBeginning(input, out actualName);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedName, actualName, "Name");
        }

        [Test]
        public void Get_Beginning_Upper_Case()
        {
            const string input = "BEGIN:VEVENT";
            const string expectedName = "VEVENT";

            string actualName;

            VCardReader vcr = new VCardReader();
            bool result = vcr.GetIsGroupBeginning(input, out actualName);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedName, actualName, "Name");
        }

        [Test]
        public void Normal_Value_Is_Not_A_Beginning()
        {
            const string input = "CREATED:20101114T222754Z";

            string actualName;

            VCardReader vcr = new VCardReader();
            bool result = vcr.GetIsGroupBeginning(input, out actualName);

            Assert.IsFalse(result);
        }

        [Test]
        public void Get_Ending_Lower_Case()
        {
            const string input = "end:vcard";
            const string expectedName = "vcard";

            string actualName;

            VCardReader vcr = new VCardReader();
            bool result = vcr.GetIsGroupEnding(input, out actualName);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedName, actualName, "Name");
        }

        [Test]
        public void Get_Ending_Upper_Case()
        {
            const string input = "END:VEVENT";
            const string expectedName = "VEVENT";

            string actualName;

            VCardReader vcr = new VCardReader();
            bool result = vcr.GetIsGroupEnding(input, out actualName);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedName, actualName, "Name");
        }

        [Test]
        public void Normal_Value_Is_Not_An_Ending()
        {
            const string input = "CREATED:20101114T222754Z";

            string actualName;

            VCardReader vcr = new VCardReader();
            bool result = vcr.GetIsGroupEnding(input, out actualName);

            Assert.IsFalse(result);
        }
    }
}

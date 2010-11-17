using System;
using NUnit.Framework;

namespace Themis.VCard
{
    [TestFixture]
    public class VCardLineReaderTests
    {
        [Test]
        public void Detect_Beginning_Lower_Case()
        {
            const string input = "begin:vcard";
            const string expectedName = "vcard";

            VCardLineReader lr = new VCardLineReader(input);

            Assert.AreEqual(VCardLineType.GroupBeginning, lr.Type, "Type");
            Assert.AreEqual(expectedName, lr.Name, "Name");
        }

        [Test]
        public void Detect_Beginning_Upper_Case()
        {
            const string input = "BEGIN:VEVENT";
            const string expectedName = "VEVENT";

            VCardLineReader lr = new VCardLineReader(input);

            Assert.AreEqual(VCardLineType.GroupBeginning, lr.Type, "Type");
            Assert.AreEqual(expectedName, lr.Name, "Name");
        }

        [Test]
        public void Detect_Ending_Lower_Case()
        {
            const string input = "end:vcard";
            const string expectedName = "vcard";

            VCardLineReader lr = new VCardLineReader(input);

            Assert.AreEqual(VCardLineType.GroupEnding, lr.Type, "Type");
            Assert.AreEqual(expectedName, lr.Name, "Name");
        }

        [Test]
        public void Get_Ending_Upper_Case()
        {
            const string input = "END:VEVENT";
            const string expectedName = "VEVENT";

            VCardLineReader lr = new VCardLineReader(input);

            Assert.AreEqual(VCardLineType.GroupEnding, lr.Type, "Type");
            Assert.AreEqual(expectedName, lr.Name, "Name");
        }

        [Test]
        public void Detect_Normal_Value()
        {
            const string input = "CREATED:20101114T222754Z";
            const string expectedName = "CREATED";
            const string expectedValue = "20101114T222754Z";

            VCardLineReader lr = new VCardLineReader(input);

            Assert.AreEqual(VCardLineType.Value, lr.Type, "Type");
            Assert.AreEqual(expectedName, lr.Name, "Name");
            Assert.AreEqual(expectedValue, lr.GetEscapedValue(), "EscapedValue");
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Line does not contain a name/value delimiter")]
        public void Line_With_No_Separator_Fails()
        {
            const string input = "CREATED20101114T222754Z";

            VCardLineReader lr = new VCardLineReader(input);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Line does not contain a value delimiter")]
        public void Line_With_Param_Separator_But_No_Value_Separator_Fails()
        {
            const string input = "CREATED;P=20101114T222754Z";

            VCardLineReader lr = new VCardLineReader(input);

            string n; string v;
            lr.ReadParameter(out n, out v);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Name must be at least one character")]
        public void Line_With_Separator_As_First_Character_Fails()
        {
            const string input = ":20101114T222754Z";

            VCardLineReader lr = new VCardLineReader(input);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Group Beginning cannot contain parameters")]
        public void Beginning_With_Paramaters_Fails()
        {
            const string input = "BEGIN;Param=1:VCARD";

            VCardLineReader lr = new VCardLineReader(input);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Group Ending cannot contain parameters")]
        public void Ending_With_Paramaters_Fails()
        {
            const string input = "END;Param=1:VCARD";

            VCardLineReader lr = new VCardLineReader(input);
        }

        [Test]
        public void Read_Value_With_Parameter()
        {
            const string input = "ATTENDEE;CN=testuser@fake.foo:mailto:testuser@fake.foo";
            const string expectedName = "ATTENDEE";
            const string expectedParamName = "CN";
            const string expectedParamValue = "testuser@fake.foo";
            const string expectedValue = "mailto:testuser@fake.foo";

            VCardLineReader lr = new VCardLineReader(input);

            string actualParamName; string actualParamValue;
            bool gotParam = lr.ReadParameter(out actualParamName, out actualParamValue);

            string actualValue = lr.GetEscapedValue();

            Assert.AreEqual(VCardLineType.Value, lr.Type, "Type");
            Assert.AreEqual(expectedName, lr.Name, "Name");

            Assert.IsTrue(gotParam, "Retrieved Parameter");
            Assert.AreEqual(expectedParamName, actualParamName, "Parameter Name");
            Assert.AreEqual(expectedParamValue, actualParamValue, "Parameter Value");

            Assert.AreEqual(expectedValue, actualValue, "EscapedValue");
        }

        [Test]
        public void Read_Value_With_Two_Parameters()
        {
            const string input = "Name;P1=V1;P2=V2:Value";

            VCardLineReader lr = new VCardLineReader(input);
            Assert.AreEqual("Name", lr.Name, "Name");

            string name; string value; bool gotParam;
            

            gotParam = lr.ReadParameter(out name, out value);

            Assert.IsTrue(gotParam, "P1: Read Parameter");
            Assert.AreEqual("P1", name, "P1: Name");
            Assert.AreEqual("V1", value, "P1: Value");


            gotParam = lr.ReadParameter(out name, out value);

            Assert.IsTrue(gotParam, "P2: Read Parameter");
            Assert.AreEqual("P2", name, "P2: Name");
            Assert.AreEqual("V2", value, "P2: Value");


            gotParam = lr.ReadParameter(out name, out value);
            Assert.IsFalse(gotParam, "P3: Read Parameter");


            value = lr.GetEscapedValue();
            Assert.AreEqual("Value", value, "Value");
        }


        [Test]
        public void Read_Parameter_Returns_False_If_There_Are_No_Parameters()
        {
            const string input = "ATTENDEE:mailto:testuser@fake.foo";

            VCardLineReader lr = new VCardLineReader(input);

            string actualParamName; string actualParamValue;
            bool gotParam = lr.ReadParameter(out actualParamName, out actualParamValue);

            Assert.IsFalse(gotParam, "Retrieved Parameter");
        }

        [Test]
        public void Read_Parameter_Returns_True_Only_Once_For_One_Parameter()
        {
            const string input = "ATTENDEE;CN=testuser@fake.foo:mailto:testuser@fake.foo";

            VCardLineReader lr = new VCardLineReader(input);

            string n; string v;

            Assert.IsTrue(lr.ReadParameter(out n, out v), "1");

            Assert.IsFalse(lr.ReadParameter(out n, out v), "2"); 
        }

        [Test]
        public void Read_Value_With_Escaped_Semicolon()
        {
            const string input = @"RANDOMTEXT:before\;after";
            const string expectedValue = @"before\;after";

            VCardLineReader lr = new VCardLineReader(input);
            string actualValue = lr.GetEscapedValue();

            Assert.AreEqual(expectedValue, actualValue, "EscapedValue");
        }

        [Test]
        public void Read_Parameter_With_Escaped_Colon()
        {
            const string input = @"SOMEVAL;Param=My\:Value:MyText";
            const string expectedParamName = "Param";
            const string expectedParamValue = @"My\:Value";

            VCardLineReader lr = new VCardLineReader(input);

            string actualParamName; string actualParamValue;
            bool gotParam = lr.ReadParameter(out actualParamName, out actualParamValue);

            string actualValue = lr.GetEscapedValue();

            Assert.IsTrue(gotParam, "Retrieved Parameter");
            Assert.AreEqual(expectedParamName, actualParamName, "Parameter Name");
            Assert.AreEqual(expectedParamValue, actualParamValue, "Parameter Value");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage="You must read all parameters before you can read the value of a line")]
        public void Read_Value_Fails_If_You_Havent_Read_All_Parameters_First()
        {
            const string input = "ATTENDEE;CN=testuser@fake.foo:mailto:testuser@fake.foo";

            VCardLineReader lr = new VCardLineReader(input);

            lr.GetEscapedValue();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "You cannot read parameters or values of a group beginning or ending")]
        public void Read_Value_Fails_For_Group_Beginning()
        {
            const string input = "BEGIN:VCARD";

            VCardLineReader lr = new VCardLineReader(input);
            lr.GetEscapedValue();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "You cannot read parameters or values of a group beginning or ending")]
        public void Read_Parameter_Fails_For_Group_Beginning()
        {
            const string input = "BEGIN:VCARD";

            VCardLineReader lr = new VCardLineReader(input);
            
            string n; string v;
            lr.ReadParameter(out n, out v);
        }
    }
}

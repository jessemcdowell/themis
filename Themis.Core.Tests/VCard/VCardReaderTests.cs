using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Themis.VCard
{
    [TestFixture]
    public class VCardReaderTests
    {
        private VCardEntity ReadVCardEntity(string input)
        {
            var encoding = Encoding.UTF8;
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(input), false))
            using (StreamReader sr = new StreamReader(ms, encoding))
            {
                VCardReader vcr = new VCardReader();
                return vcr.ReadEntity(sr);
            }
        }

        [Test]
        public void Read_Simple_Value()
        {
            const string input = "Name:Value";

            VCardEntity entity = ReadVCardEntity(input);

            Assert.That(entity, Is.InstanceOf<VCardValue>());

            VCardValue value = (VCardValue)entity;
            Assert.That(value.Name, Is.EqualTo("Name"), "Name");
            Assert.That(value.EscapedValue, Is.EqualTo("Value"), "EscapedValue");
            Assert.That(value.Parameters.Count, Is.EqualTo(0), "Parameters.Count");
        }

        [Test]
        public void Read_Value_With_Parameter()
        {
            const string input = "Name;Param=PValue:Value";

            VCardEntity entity = ReadVCardEntity(input);

            Assert.That(entity, Is.InstanceOf<VCardValue>());

            VCardValue value = (VCardValue)entity;
            Assert.That(value.Name, Is.EqualTo("Name"), "Name");
            Assert.That(value.EscapedValue, Is.EqualTo("Value"), "EscapedValue");

            Assert.That(value.Parameters.Count, Is.EqualTo(1), "Parameters.Count");
            
            VCardSimpleValue parameter = value.Parameters[0];
            Assert.That(parameter.Name, Is.EqualTo("Param"), "Parameter[0].Name");
            Assert.That(parameter.EscapedValue, Is.EqualTo("PValue"), "Parameter[0].Value");
        }

        [Test]
        public void Read_Simple_Group_With_One_Value()
        {
            const string input = 
@"BEGIN:G1
Child:Value
END:G1
";

            VCardEntity entity = ReadVCardEntity(input);

            Assert.That(entity, Is.InstanceOf<VCardGroup>());
            VCardGroup group = (VCardGroup)entity;

            Assert.That(group.Name, Is.EqualTo("G1"), "Name");
            Assert.That(group.Children.Count, Is.EqualTo(1), "Children.Count");

            Assert.That(group.Children[0], Is.InstanceOf<VCardValue>(), "Children[0].GetType");
            Assert.That(group.Children[0].Name, Is.EqualTo("Child"), "Children[0].Name");
        }


        [Test]
        public void Read_Group_With_Child_Groups_And_Values()
        {
            const string input =
@"BEGIN:G1
G1V:Value
BEGIN:G2
G2V:ChildValue
END:G2
END:G1
";

            VCardEntity entity = ReadVCardEntity(input);

            Assert.That(entity, Is.InstanceOf<VCardGroup>());
            VCardGroup group = (VCardGroup)entity;

            Assert.That(group.Children.Count, Is.EqualTo(2), "Children.Count");

            Assert.That(group.Children[0], Is.InstanceOf<VCardValue>(), "Children[0].GetType");
            
            Assert.That(group.Children[1], Is.InstanceOf<VCardGroup>(), "Children[1].GetType");
            group = (VCardGroup)group.Children[1];

            Assert.That(group.Children[0], Is.InstanceOf<VCardValue>(), "Children[1].Children[0].GetType");
            Assert.That(group.Children[0].Name, Is.EqualTo("G2V"), "Children[1].Children[0].Name");
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage="Encountered end of VCard content before ending of group G1")]
        public void Group_Without_An_Ending_Fails()
        {
            const string input =
@"BEGIN:G1
Child:Value
";
            ReadVCardEntity(input);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Encountered end of Group G2 before current group G1")]
        public void Group_With_Different_Ending_Fails()
        {
            const string input =
@"BEGIN:G1
Child:Value
END:G2
";

            ReadVCardEntity(input);
        }

        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Encountered end of Group G1 before current group G2")]
        public void Group_With_Child_Group_Beginning_And_No_Ending_Fails()
        {
            const string input =
@"BEGIN:G1
BEGIN:G2
Child:Value
END:G1
";

            ReadVCardEntity(input);
        }

        [Test]
        public void Group_With_Ending_Of_Different_Case_Succeeds()
        {
            const string input =
@"BEGIN:THEGROUP
Child:Value
END:thegroup
";

            VCardEntity entity = ReadVCardEntity(input);

            Assert.That(entity, Is.InstanceOf<VCardGroup>());
            VCardGroup group = (VCardGroup)entity;
        }


        [Test]
        [ExpectedException(typeof(InvalidVCardFormatException), ExpectedMessage = "Encountered unexpected group ending G1")]
        public void Ending_Before_Group_Fails()
        {
            const string input =
@"END:G1
Child:Value
";
            ReadVCardEntity(input);
        }

        [Test]
        public void Outlook_Invitation_With_Multibyte_Characters_Reads_Without_Errors()
        {
            const string input =
@"BEGIN:VCALENDAR
PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN
VERSION:2.0
METHOD:REQUEST
X-MS-OLK-FORCEINSPECTOROPEN:TRUE
BEGIN:VEVENT
ATTENDEE;CN=testuser@fake.foo;RSVP=TRUE:mailto:testuser@fake.foo
CLASS:PUBLIC
CREATED:20101117T032844Z
DESCRIPTION:When: November-16-10 7:30 PM-8:00 PM (UTC-08:00) Pacific Time (
	US & Canada).\nWhere: city\, state\n\nNote: The GMT offset above does not 
	reflect daylight saving time adjustments.\n\n*~*~*~*~*~*~*~*~*~*\n\nCharac
	ters: \n& amp\n\\ backslash\n\, comma\nはい hai\n\n
DTEND:20101117T040000Z
DTSTAMP:20101117T032844Z
DTSTART:20101117T033000Z
LAST-MODIFIED:20101117T032844Z
LOCATION:city\, state
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
PRIORITY:5
SEQUENCE:0
SUMMARY;LANGUAGE=en-ca:subject \; semicolon
TRANSP:OPAQUE
UID:040000008200E00074C5B7101A82E0080000000040A95B7BC485CB01000000000000000
	0100000005C34DB25F0121A4FA7D4C3F9CF3F2B98
X-ALT-DESC;FMTTYPE=text/html:<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 3.2//E
	N"">\n<HTML>\n<HEAD>\n<META NAME=""Generator"" CONTENT=""MS Exchange Server ve
	rsion 08.00.0681.000"">\n<TITLE></TITLE>\n</HEAD>\n<BODY>\n<!-- Converted f
	rom text/rtf format -->\n\n<P DIR=LTR><SPAN LANG=""en-us""><FONT FACE=""Calib
	ri"">When: November-16-10 7:30 PM-8:00 PM (UTC-08:00) Pacific Time (US &amp
	\; Canada).</FONT></SPAN></P>\n\n<P DIR=LTR><SPAN LANG=""en-us""><FONT FACE=
	""Calibri"">Where: city\, state</FONT></SPAN></P>\n\n<P DIR=LTR><SPAN LANG=""
	en-us""><FONT FACE=""Calibri"">Note: The GMT offset above does not reflect da
	ylight saving time adjustments.</FONT></SPAN></P>\n\n<P DIR=LTR><SPAN LANG
	=""en-us""><FONT FACE=""Calibri"">*~*~*~*~*~*~*~*~*~*</FONT></SPAN></P>\n\n<P 
	DIR=LTR><SPAN LANG=""en-us""></SPAN><SPAN LANG=""en-ca""><FONT FACE=""Calibri"">
	Characters: </FONT></SPAN></P>\n\n<P DIR=LTR><SPAN LANG=""en-ca""><FONT FACE
	=""Calibri"">&amp\; amp</FONT></SPAN></P>\n\n<P DIR=LTR><SPAN LANG=""en-ca""><
	FONT FACE=""Calibri"">\\</FONT><FONT FACE=""Calibri""> backslash</FONT></SPAN>
	</P>\n\n<P DIR=LTR><SPAN LANG=""en-ca""><FONT FACE=""Calibri"">\, comma</FONT>
	</SPAN></P>\n\n<P DIR=LTR><SPAN LANG=""en-us""></SPAN><SPAN LANG=""en-ca""><FO
	NT FACE=""MS Mincho"">はい</FONT></SPAN><SPAN LANG=""en-us""></SPAN><SPAN LA
	NG=""en-ca""><FONT FACE=""Calibri""> hai</FONT></SPAN></P>\n\n<P DIR=LTR><SPAN
	 LANG=""en-us""></SPAN><SPAN LANG=""en-ca""></SPAN></P>\n\n</BODY>\n</HTML>
X-MICROSOFT-CDO-BUSYSTATUS:TENTATIVE
X-MICROSOFT-CDO-IMPORTANCE:1
X-MICROSOFT-CDO-INTENDEDSTATUS:BUSY
X-MICROSOFT-DISALLOW-COUNTER:FALSE
X-MS-OLK-ALLOWEXTERNCHECK:TRUE
X-MS-OLK-AUTOSTARTCHECK:FALSE
X-MS-OLK-CONFTYPE:0
X-MS-OLK-SENDER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
BEGIN:VALARM
TRIGGER:-PT15M
ACTION:DISPLAY
DESCRIPTION:Reminder
END:VALARM
END:VEVENT
END:VCALENDAR";

            VCardEntity entity = ReadVCardEntity(input);

            Assert.That(entity, Is.InstanceOf<VCardGroup>());

            VCardGroup vevent = (VCardGroup)((VCardGroup)entity).Children.GetFirstEntity("vevent");
            VCardValue description = (VCardValue)vevent.Children.GetFirstEntity("description");

            Assert.That(description.EscapedValue, Contains.Substring("はい"), "Japanese Characters");
        }

    }
}

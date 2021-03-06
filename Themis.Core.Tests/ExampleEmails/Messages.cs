﻿using System;
using System.IO;
using ActiveUp.Net.Mail;
using Themis.Email;

namespace Themis.ExampleEmails
{
    public static class Messages
    {
        private const string BasePath = @"ExampleEmails";

        private static Message GetMessageByFileName(string fileName)
        {
            string path = Path.Combine(BasePath, fileName);

            return Parser.ParseMessageFromFile(path);
        }

        public static IReceivedEmail GetRecievedEmail(Message message)
        {
            return new MailSystemReceivedEmail(message);
        }

        public static Message GetOutlook1NewEmail()
        {
            return GetMessageByFileName("Outlook-1-New.txt");
        }

        public static Message GetOutlook1NewWithSimbolsEmail()
        {
            return GetMessageByFileName("Outlook-1-New.WithSymbols.txt");
        }
    }
}

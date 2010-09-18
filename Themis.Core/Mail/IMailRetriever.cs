using System;

namespace Themis.Mail
{
    public interface IMailRetriever
    {

        /// <summary>
        /// Retrieve messages from the specified server, calling the callback function for each mail item
        /// </summary>
        /// <param name="server">The server to connect to.</param>
        /// <param name="messageRecievedCallback">The callback function to be called for each message. Callback should return true for any message that is processed successfully, and false for any that should be left on the server.</param>
        void GetMessages(MailboxConnectionInfo server, Func<IMailMessage, bool> messageRecievedCallback);

    }
}

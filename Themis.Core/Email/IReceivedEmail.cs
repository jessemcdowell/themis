using System;

namespace Themis.Email
{
    public interface IReceivedEmail
    {
        EmailAddress From { get; }

        string Subject { get; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace Themis.Email
{
    public interface IReceivedEmailSection
    {
        string ContentMimeType { get; }

        TextReader GetContentTextReader();

        IList<IReceivedEmailSection> ChildSections { get; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Themis.Email;

namespace Themis.EmailProcessing
{
    internal class FakeReceivedEmailSection : IReceivedEmailSection
    {
        public string ContentMimeType { get; set; }

        public List<IReceivedEmailSection> ChildSections { get; set; }

        public Func<TextReader> GetContentTextReaderGenerator { get; set; }

        /// <summary>
        /// A special value to help matching instances that have the same values
        /// </summary>
        public Guid UniqueId { get; private set; }

        public FakeReceivedEmailSection()
        {
            ChildSections = new List<IReceivedEmailSection>();
            ContentMimeType = "fake/type";
            UniqueId = Guid.NewGuid();
        }

        string IReceivedEmailSection.ContentMimeType
        {
            get { return ContentMimeType; }
        }

        TextReader IReceivedEmailSection.GetContentTextReader()
        {
            var generator = GetContentTextReaderGenerator;
            if (generator != null)
                return generator();

            throw new NotImplementedException();
        }

        IList<IReceivedEmailSection> IReceivedEmailSection.ChildSections
        {
            get { return ChildSections.AsReadOnly(); }
        }

        public override bool Equals(object obj)
        {
            FakeReceivedEmailSection other = obj as FakeReceivedEmailSection;
            if (other == null)
                return false;

            return (other.UniqueId == UniqueId);
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }
    }
}

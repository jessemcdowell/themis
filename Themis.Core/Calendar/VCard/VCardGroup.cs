using System;
using System.Text;

namespace Themis.Calendar.VCard
{
    public class VCardGroup : VCardEntity
    {
        public VCardGroup()
        {
            Children = new VCardEntityList<VCardEntity>();
        }

        public VCardGroup(string name)
            : this()
        {
            Name = name;
        }

        public VCardEntityList<VCardEntity> Children { get; private set; }

    }
}

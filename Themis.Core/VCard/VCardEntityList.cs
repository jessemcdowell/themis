using System;
using System.Collections.Generic;

namespace Themis.VCard
{
    public class VCardEntityList<T> : List<T> where T : VCardEntity
    {
        public VCardEntityList()
            : base(100)
        { }


        public bool TryGetFirstEntity(string name, out T entity)
        {
            foreach (T e in GetEntities(name))
            {
                entity = e;
                return true;
            }

            entity = null;
            return false;
        }

        public T GetFirstEntity(string name)
        {
            T e;
            if (!TryGetFirstEntity(name, out e))
                throw new KeyNotFoundException();
            return e;
        }

        public IEnumerable<T> GetEntities(string name)
        {
            foreach (T e in this)
            {
                if (String.Equals(e.Name, name, StringComparison.InvariantCultureIgnoreCase))
                    yield return e;
            }
        }
    }
}

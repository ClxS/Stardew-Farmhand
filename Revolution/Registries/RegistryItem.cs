using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revolution.Registries
{
    public class RegistryItem<T, TKey>
    {
        public RegistryItem(TKey key, T item)
        {
            UniqueId = key;
            Item = item;
        }

        public TKey UniqueId;
        public T Item;
    }
}

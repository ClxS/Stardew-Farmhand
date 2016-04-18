using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.Helpers
{
    public class UniqueId<T>
    {
        private static readonly List<T> RegisteredItems = new List<T>();

        public readonly T ThisId;

        public UniqueId(T id)
        {
            ThisId = id;

            if(RegisteredItems.Contains(id))
                throw new Exception($"{id} has been registered more than once!");

            RegisteredItems.Add(ThisId);
        }

        ~UniqueId()
        {
            if(!RegisteredItems.Contains(ThisId))
                Logging.Log.Warning($"Tried to remove unique id {ThisId} but it was not present");
            else
                RegisteredItems.Remove(ThisId);
        }

        public override bool Equals(object obj)
        {
            var uniqueObj = obj as UniqueId<T>;
            return Equals(uniqueObj);
        }

        public bool Equals(T obj)
        {
            return ThisId.Equals(obj);
        }

        public bool Equals(UniqueId<T> other)
        {
            return other != null && ThisId.Equals(other.ThisId);
        }

        public override int GetHashCode()
        {
            return ThisId.GetHashCode();
        }
        
    }
}

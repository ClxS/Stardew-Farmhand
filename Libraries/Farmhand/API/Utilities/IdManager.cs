using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmhand.API.Utilities
{
    internal static class IdManager
    {
        private static readonly List<int> AssignedIds = new List<int>();
        private static readonly Random Random = new Random();

        public static int AssignNewId(int min)
        {
            var id = Random.Next(min);
            while (AssignedIds.Any(n => n == id))
            {
                id = Random.Next(min);
            }
            AssignedIds.Add(id);
            return id;
        } 
    }
}

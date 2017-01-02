namespace Farmhand.Helpers
{
    using System;
    using System.Collections.Generic;

    using Farmhand.Logging;

    /// <summary>
    ///     Defines a unique ID and enforces uniqueness.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the ID
    /// </typeparam>
    public class UniqueId<T>
    {
        private static readonly List<T> RegisteredItems = new List<T>();

        /// <summary>
        ///     The ID for this instance.
        /// </summary>
        public readonly T ThisId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UniqueId{T}" /> class.
        /// </summary>
        /// <param name="id">
        ///     The ID of this instance..
        /// </param>
        /// <exception cref="Exception">
        ///     Throws an exception if this ID has already been registered.
        /// </exception>
        public UniqueId(T id)
        {
            this.ThisId = id;

            if (RegisteredItems.Contains(id))
            {
                throw new Exception($"{id} has been registered more than once!");
            }

            RegisteredItems.Add(this.ThisId);
        }

        ~UniqueId()
        {
            if (!RegisteredItems.Contains(this.ThisId))
            {
                Log.Warning($"Tried to remove unique id {this.ThisId} but it was not present");
            }
            else
            {
                RegisteredItems.Remove(this.ThisId);
            }
        }

        /// <summary>
        ///     Equality comparison.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="object" /> to compare against.
        /// </param>
        /// <returns>
        ///     Whether this is equal to the provided value.
        /// </returns>
        public override bool Equals(object obj)
        {
            var uniqueObj = obj as UniqueId<T>;
            return this.Equals(uniqueObj);
        }

        /// <summary>
        ///     Equality comparison.
        /// </summary>
        /// <param name="obj">
        ///     The value to compare against.
        /// </param>
        /// <returns>
        ///     Whether this is equal to the provided value.
        /// </returns>
        public bool Equals(T obj)
        {
            return this.ThisId.Equals(obj);
        }

        /// <summary>
        ///     Equality comparison.
        /// </summary>
        /// <param name="other">
        ///     The <see cref="UniqueId{T}" /> to compare against.
        /// </param>
        /// <returns>
        ///     Whether this is equal to the provided value.
        /// </returns>
        public bool Equals(UniqueId<T> other)
        {
            return other != null && this.ThisId.Equals(other.ThisId);
        }

        /// <summary>
        ///     Gets the hash code for this instance by returning the ThisId's hash code.
        /// </summary>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public override int GetHashCode()
        {
            return this.ThisId.GetHashCode();
        }
    }
}
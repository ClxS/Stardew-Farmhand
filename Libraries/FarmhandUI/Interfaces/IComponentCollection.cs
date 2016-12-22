namespace Farmhand.UI.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The ComponentCollection interface.
    /// </summary>
    public interface IComponentCollection : IComponentContainer
    {
        /// <summary>
        ///     Gets the interactive components.
        /// </summary>
        List<IInteractiveMenuComponent> InteractiveComponents { get; }

        /// <summary>
        ///     Gets the static components.
        /// </summary>
        List<IMenuComponent> StaticComponents { get; }

        /// <summary>
        ///     Gets whether this collection can accept the provided component.
        /// </summary>
        /// <param name="component">
        ///     The component to check.
        /// </param>
        /// <returns>
        ///     Whether this collection accepts the component.
        /// </returns>
        bool AcceptsComponent(IMenuComponent component);

        /// <summary>
        ///     Adds a component to this collection
        /// </summary>
        /// <param name="component">
        ///     The component to add.
        /// </param>
        void AddComponent(IMenuComponent component);

        /// <summary>
        ///     Removes a component from this collection
        /// </summary>
        /// <param name="component">
        ///     The component to remove.
        /// </param>
        void RemoveComponent(IMenuComponent component);

        /// <summary>
        ///     Removes all components of a specific type from this collection.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of components to remove.
        /// </typeparam>
        void RemoveComponents<T>() where T : IMenuComponent;

        /// <summary>
        ///     Removes all components matching a filter from this collection.
        /// </summary>
        /// <param name="filter">
        ///     The filter to use.
        /// </param>
        void RemoveComponents(Predicate<IMenuComponent> filter);

        /// <summary>
        ///     Removes all components from this collection
        /// </summary>
        void ClearComponents();
    }
}
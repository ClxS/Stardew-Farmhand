using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Farmhand.UI
{
    public interface IComponentCollection : IComponentContainer
    {
        bool AcceptsComponent(IMenuComponent component);
        void AddComponent(IMenuComponent component);
        void RemoveComponent(IMenuComponent component);
        void RemoveComponents<T>() where T : IMenuComponent;
        void RemoveComponents(Predicate<IMenuComponent> filter);
        void ClearComponents();
        List<IInteractiveMenuComponent> InteractiveComponents { get; }
        List<IMenuComponent> StaticComponents { get; }
    }
}

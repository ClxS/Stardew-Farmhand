namespace Farmhand.UI.Interfaces
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The ComponentContainer interface.
    /// </summary>
    public interface IComponentContainer
    {
        /// <summary>
        ///     Gets the area in which events (such as mouse click/hover events) are captured.
        /// </summary>
        Rectangle EventRegion { get; }

        /// <summary>
        ///     Gets the zoomed event region.
        /// </summary>
        Rectangle ZoomEventRegion { get; }

        /// <summary>
        ///     Resets the current focus for this container.
        /// </summary>
        void ResetFocus();

        /// <summary>
        ///     Gives focus to a specified component.
        /// </summary>
        /// <param name="component">
        ///     The component to give focus.
        /// </param>
        void GiveFocus(IInteractiveMenuComponent component);

        /// <summary>
        ///     Gets the menu attached to this container.
        /// </summary>
        /// <returns>
        ///     The attached menu <see cref="FrameworkMenu" />.
        /// </returns>
        FrameworkMenu GetAttachedMenu();
    }
}
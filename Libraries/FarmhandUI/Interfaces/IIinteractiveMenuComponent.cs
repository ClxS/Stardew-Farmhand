namespace Farmhand.UI.Interfaces
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The InteractiveMenuComponent interface.
    /// </summary>
    public interface IInteractiveMenuComponent : IMenuComponent
    {
        /// <summary>
        ///     Checks whether a point is within the event area of this component.
        /// </summary>
        /// <param name="position">
        ///     The position to check.
        /// </param>
        /// <param name="offset">
        ///     The offset of this component.
        /// </param>
        /// <returns>
        ///     Whether the point is within the component bounds.
        /// </returns>
        bool InBounds(Point position, Point offset);

        /// <summary>
        ///     Called when the left mouse button is clicked over this component
        /// </summary>
        /// <param name="position">
        ///     The mouse position
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void LeftClick(Point position, Point offset);

        /// <summary>
        ///     Called when the right mouse button is clicked over this component
        /// </summary>
        /// <param name="position">
        ///     The mouse position
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void RightClick(Point position, Point offset);

        /// <summary>
        ///     Called when the left mouse button is held over this component
        /// </summary>
        /// <param name="position">
        ///     The mouse position
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void LeftHeld(Point position, Point offset);

        /// <summary>
        ///     Called when the left mouse button is released over this component
        /// </summary>
        /// <param name="position">
        ///     The mouse position
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void LeftUp(Point position, Point offset);

        /// <summary>
        ///     Called when the mouse enters this component
        /// </summary>
        /// <param name="position">
        ///     The mouse position
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void HoverIn(Point position, Point offset);

        /// <summary>
        ///     Called when the mouse leaves this component
        /// </summary>
        /// <param name="position">
        ///     The mouse position
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void HoverOut(Point position, Point offset);

        /// <summary>
        ///     Called when the mouse moves over this component
        /// </summary>
        /// <param name="position">
        ///     The mouse position
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void HoverOver(Point position, Point offset);

        /// <summary>
        ///     Called when focus is lost
        /// </summary>
        void FocusLost();

        /// <summary>
        ///     Called when focus is gained
        /// </summary>
        void FocusGained();

        /// <summary>
        ///     Called when the scroll wheel is used over this component
        /// </summary>
        /// <param name="direction">
        ///     The delta value of the scroll
        /// </param>
        /// <param name="position">
        ///     The mouse position
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void Scroll(int direction, Point position, Point offset);
    }
}
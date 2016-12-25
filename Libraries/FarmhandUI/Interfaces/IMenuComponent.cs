namespace Farmhand.UI.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     The MenuComponent interface.
    /// </summary>
    public interface IMenuComponent
    {
        /// <summary>
        ///     Gets or sets a value indicating whether visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        ///     Gets or sets the layer.
        /// </summary>
        int Layer { get; set; }

        /// <summary>
        ///     Gets the parent container.
        /// </summary>
        IComponentContainer Parent { get; }

        /// <summary>
        ///     Called every frame on each component, responsible for updating behavior
        /// </summary>
        /// <param name="t">
        ///     The time elapsed since the previous frame
        /// </param>
        void Update(GameTime t);

        /// <summary>
        ///     Called each frame on each component, responsible for drawing it and it's children
        /// </summary>
        /// <param name="b">
        ///     The sprite batch to use for drawing
        /// </param>
        /// <param name="offset">
        ///     The offset of this component
        /// </param>
        void Draw(SpriteBatch b, Point offset);

        /// <summary>
        ///     Attaches this component to another
        /// </summary>
        /// <param name="collection">
        ///     The collection to attach to
        /// </param>
        void Attach(IComponentContainer collection);

        /// <summary>
        ///     Detaches this component from it's parent
        /// </summary>
        /// <param name="collection">
        ///     Collection to detach from
        /// </param>
        void Detach(IComponentContainer collection);

        /// <summary>
        ///     Gets the position of this component
        /// </summary>
        /// <returns>
        ///     The position of this component as a <see cref="Point" />.
        /// </returns>
        Point GetPosition();

        /// <summary>
        ///     Gets the bounding region of this component
        /// </summary>
        /// <returns>
        ///     The region of this component as a <see cref="Rectangle" />.
        /// </returns>
        Rectangle GetRegion();
    }
}
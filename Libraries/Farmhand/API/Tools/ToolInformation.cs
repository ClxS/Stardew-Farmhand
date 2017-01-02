namespace Farmhand.API.Tools
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     Contains custom tool information.
    /// </summary>
    public class ToolInformation
    {
        /// <summary>
        ///     Gets the tool ID.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     Gets or sets the tool name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether stackable.
        /// </summary>
        public bool Stackable { get; set; } = false;

        /// <summary>
        ///     Gets or sets the attachment slots.
        /// </summary>
        public int AttachmentSlots { get; set; } = 0;

        /// <summary>
        ///     Gets or sets the upgrade level.
        /// </summary>
        public int UpgradeLevel { get; set; } = -1;

        /// <summary>
        ///     Determine, based off the Id of the tool, where the InitialParentIndex is.
        /// </summary>
        /// <returns>
        ///     The initial parent index.
        /// </returns>
        public int GetInitialParentIndex()
        {
            var largeIndex = Tool.InitialTools + this.Id;

            var xLarge = largeIndex % 3;
            var yLarge = largeIndex / 3;

            var xSmall = xLarge * 7;
            var ySmall = yLarge * 2;

            var smallIndex = xSmall + ySmall * 21;

            return smallIndex;
        }

        /// <summary>
        ///     Determine, based off the Id of the tool, where the IndexOfMenuItem is
        /// </summary>
        /// <returns>
        ///     The menu item index.
        /// </returns>
        public int GetIndexOfMenuItem()
        {
            return this.GetInitialParentIndex() + 26;
        }
    }
}
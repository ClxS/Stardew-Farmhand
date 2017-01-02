namespace Farmhand.API.Tools
{
    using System;
    using System.Collections.Generic;

    using Farmhand.API.Utilities;

    using StardewValley;

    /// <summary>
    ///     Tool-related API functionality.
    /// </summary>
    public static class Tool
    {
        /// <summary>
        ///     The initial count of tools.
        /// </summary>
        public const int InitialTools = 36;

        /// <summary>
        ///     Gets the next available index.
        /// </summary>
        public static int NextAvailableIndex { get; private set; }

        internal static Dictionary<string, ToolInformation> Tools { get; } = new Dictionary<string, ToolInformation>();

        /// <summary>
        ///     Registers a new tool
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the tool.
        /// </typeparam>
        /// <param name="tool">
        ///     Information of tool to register
        /// </param>
        public static void RegisterTool<T>(ToolInformation tool)
        {
            if (Game1.toolSpriteSheet == null)
            {
                throw new Exception(
                    "objectInformation is null! This likely occurs if you try to register an item before AfterContentLoaded");
            }

            tool.Id = NextAvailableIndex;
            NextAvailableIndex++;

            Tools.Add(tool.Name, tool);
            TextureUtility.AddSpriteToSpritesheet(
                ref Game1.toolSpriteSheet,
                tool.Texture,
                InitialTools + tool.Id,
                112,
                32);
        }
    }
}
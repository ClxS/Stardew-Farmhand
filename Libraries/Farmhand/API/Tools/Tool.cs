using Farmhand.API.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Tools
{
    public class Tool
    {
        public static Dictionary<string, ToolInformation> Tools { get; } = new Dictionary<string, ToolInformation>();

        public static int NextAvailableIndex = 0;

        public const int InitialTools = 36;

        /// <summary>
        /// Registers a new tool
        /// </summary>
        /// <param name="tool">Information of tool to register</param>
        public static void RegisterTool<T>(ToolInformation tool)
        {
            if (Game1.toolSpriteSheet == null)
            {
                throw new Exception("objectInformation is null! This likely occurs if you try to register an item before AfterContentLoaded");
            }

            tool.Id = NextAvailableIndex;
            NextAvailableIndex++;

            Tools.Add(tool.Name, tool);
            TextureUtility.AddSpriteToSpritesheet(ref Game1.toolSpriteSheet, tool.Texture, InitialTools + tool.Id, 112, 32);
        }
    }
}

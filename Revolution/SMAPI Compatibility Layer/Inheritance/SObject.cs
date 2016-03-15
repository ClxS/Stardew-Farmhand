using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StardewModdingAPI.Inheritance
{
    public class SObject : StardewValley.Object
    {
        public override String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Description { get; set; }
        public Texture2D Texture { get; set; }
        public String CategoryName { get; set; }
        public Color CategoryColour { get; set; }
        public Boolean IsPassable { get; set; }
        public Boolean IsPlaceable { get; set; }
        public Boolean HasBeenRegistered { get; set; }
        public Int32 RegisteredId { get; set; }

        public Int32 MaxStackSize { get; set; }

        public Boolean WallMounted { get; set; }
        public Vector2 DrawPosition { get; set; }

        public Boolean FlaggedForPickup { get; set; }

        [XmlIgnore]
        public Vector2 CurrentMouse { get; protected set; }
        [XmlIgnore]
        public Vector2 PlacedAt { get; protected set; }

        public override int Stack
        {
            get { return stack; }
            set { stack = value; }
        }

        public SObject()
        {
            name = "Modded Item Name";
            Description = "Modded Item Description";
            CategoryName = "Modded Item Category";
            Category = 4163;
            CategoryColour = Color.White;
            IsPassable = false;
            IsPlaceable = false;
            boundingBox = new Rectangle(0, 0, 64, 64);
            MaxStackSize = 999;

            type = "interactive";
        }

        public override string getDescription()
        {
            return Description;
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(((x * Game1.tileSize) + (Game1.tileSize / 2)) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(((y * Game1.tileSize) + (Game1.tileSize / 2)) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex)), (Color)(Color.White * alpha), 0f, new Vector2(8f, 8f), (this.scale.Y > 1f) ? this.getScale().Y : ((float)Game1.pixelZoom), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.isPassable() ? ((float)this.getBoundingBox(new Vector2((float)x, (float)y)).Top) : ((float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom)) / 10000f);
            }
        }

        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
        {
            Log.Debug("THIS DRAW FUNCTION IS NOT IMPLEMENTED I WANT TO KNOW WHERE IT IS CALLED");
            return;
            try
            {
                if (Texture != null)
                {
                    int targSize = Game1.tileSize;
                    int midX = (int)((xNonTile) + 32);
                    int midY = (int)((yNonTile) + 32);

                    int targX = midX - targSize / 2;
                    int targY = midY - targSize / 2;

                    Rectangle targ = new Rectangle(targX, targY, targSize, targSize);
                    spriteBatch.Draw(Texture, targ, null, new Color(255, 255, 255, 255f * alpha), 0, Vector2.Zero, SpriteEffects.None, layerDepth);
                    //spriteBatch.Draw(Program.DebugPixel, targ, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
                    /*
                    spriteBatch.DrawString(Game1.dialogueFont, "TARG: " + targ, new Vector2(128, 0), Color.Red);
                    spriteBatch.DrawString(Game1.dialogueFont, ".", new Vector2(targX * 0.5f, targY), Color.Orange);
                    spriteBatch.DrawString(Game1.dialogueFont, ".", new Vector2(targX, targY), Color.Red);
                    spriteBatch.DrawString(Game1.dialogueFont, ".", new Vector2(targX * 1.5f, targY), Color.Yellow);
                    spriteBatch.DrawString(Game1.dialogueFont, ".", new Vector2(targX * 2f, targY), Color.Green);
                    */
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                Console.ReadKey();
            }
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {
            if (this.isRecipe)
            {
                transparency = 0.5f;
                scaleSize *= 0.75f;
            }

            if (Texture != null)
            {
                int targSize = (int)(64 * scaleSize * 0.9f);
                int midX = (int)((location.X) + 32);
                int midY = (int)((location.Y) + 32);

                int targX = midX - targSize / 2;
                int targY = midY - targSize / 2;

                spriteBatch.Draw(Texture, new Rectangle(targX, targY, targSize, targSize), null, new Color(255, 255, 255, transparency), 0, Vector2.Zero, SpriteEffects.None, layerDepth);
            }
            if (drawStackNumber)
            {
                float scale = 0.5f + scaleSize;
                Game1.drawWithBorder(string.Concat(this.stack.ToString()), Color.Black, Color.White, location + new Vector2((float)Game1.tileSize - Game1.tinyFont.MeasureString(string.Concat(this.stack.ToString())).X * scale, (float)Game1.tileSize - (float)((double)Game1.tinyFont.MeasureString(string.Concat(this.stack.ToString())).Y * 3.0f / 4.0f) * scale), 0.0f, scale, 1f, true);
            }
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (Texture != null)
            {
                int targSize = 64;
                int midX = (int)((objectPosition.X) + 32);
                int midY = (int)((objectPosition.Y) + 32);

                int targX = midX - targSize / 2;
                int targY = midY - targSize / 2;

                spriteBatch.Draw(Texture, new Rectangle(targX, targY, targSize, targSize), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, (f.getStandingY() + 2) / 10000f);
            }
        }

        public override Color getCategoryColor()
        {
            return CategoryColour;
        }

        public override string getCategoryName()
        {
            if (string.IsNullOrEmpty(CategoryName))
                return "Modded Item";
            return CategoryName;
        }

        public override bool isPassable()
        {
            return IsPassable;
        }

        public override bool isPlaceable()
        {
            return IsPlaceable;
        }

        public override int maximumStackSize()
        {
            return MaxStackSize;
        }

        public SObject Clone()
        {
            SObject toRet = new SObject();

            toRet.Name = this.Name;
            toRet.CategoryName = this.CategoryName;
            toRet.Description = this.Description;
            toRet.Texture = this.Texture;
            toRet.IsPassable = this.IsPassable;
            toRet.IsPlaceable = this.IsPlaceable;
            toRet.quality = this.quality;
            toRet.scale = this.scale;
            toRet.isSpawnedObject = this.isSpawnedObject;
            toRet.isRecipe = this.isRecipe;
            toRet.questItem = this.questItem;
            toRet.stack = 1;
            toRet.HasBeenRegistered = this.HasBeenRegistered;
            toRet.RegisteredId = this.RegisteredId;

            return toRet;
        }

        public override Item getOne()
        {
            return this.Clone();
        }

        public override void actionWhenBeingHeld(Farmer who)
        {
            int x = Game1.oldMouseState.X + Game1.viewport.X;
            int y = Game1.oldMouseState.Y + Game1.viewport.Y;

            x = x / Game1.tileSize;
            y = y / Game1.tileSize;

            CurrentMouse = new Vector2(x, y);
            //Program.LogDebug(canBePlacedHere(Game1.currentLocation, CurrentMouse));
            base.actionWhenBeingHeld(who);
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            //Program.LogDebug(CurrentMouse.ToString().Replace("{", "").Replace("}", ""));
            if (!l.objects.ContainsKey(tile))
                return true;

            return false;
        }

        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            if (Game1.didPlayerJustRightClick())
                return false;

            x = (x / Game1.tileSize);
            y = (y / Game1.tileSize);

            //Program.LogDebug(x + " - " + y);
            //Console.ReadKey();

            Vector2 key = new Vector2(x, y);

            if (!canBePlacedHere(location, key))
                return false;

            SObject s = Clone();

            s.PlacedAt = key;
            s.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);

            location.objects.Add(key, s);
            Log.Verbose("{0} - {1}", this.GetHashCode(), s.GetHashCode());

            return true;
        }

        public override void actionOnPlayerEntry()
        {
            //base.actionOnPlayerEntry();
        }

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            if (canBePlacedHere(location, CurrentMouse))
            {
                int targSize = Game1.tileSize;

                int x = Game1.oldMouseState.X + Game1.viewport.X;
                int y = Game1.oldMouseState.Y + Game1.viewport.Y;
                spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(x / Game1.tileSize * Game1.tileSize - Game1.viewport.X), (float)(y / Game1.tileSize * Game1.tileSize - Game1.viewport.Y)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(Utility.playerCanPlaceItemHere(location, (Item)this, x, y, Game1.player) ? 194 : 210, 388, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.01f);
            }
        }
    }
}

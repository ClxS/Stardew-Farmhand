using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Xml.Serialization;
// ReSharper disable ArrangeThisQualifier
// ReSharper disable ArrangeRedundantParentheses

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
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
                // ReSharper disable once ArrangeRedundantParentheses
                spriteBatch.Draw(Texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(((x * Game1.tileSize) + (Game1.tileSize / 2)) + ((shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), ((y * Game1.tileSize) + (Game1.tileSize / 2)) + ((shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0))), Game1.currentLocation.getSourceRectForObject(ParentSheetIndex), Color.White * alpha, 0f, new Vector2(8f, 8f), (scale.Y > 1f) ? getScale().Y : (Game1.pixelZoom), flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (isPassable() ? (getBoundingBox(new Vector2(x, y)).Top) : (getBoundingBox(new Vector2(x, y)).Bottom)) / 10000f);
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
                var targSize = (int)(64 * scaleSize * 0.9f);
                var midX = (int)((location.X) + 32);
                var midY = (int)((location.Y) + 32);

                var targX = midX - targSize / 2;
                var targY = midY - targSize / 2;

                spriteBatch.Draw(Texture, new Rectangle(targX, targY, targSize, targSize), null, new Color(255, 255, 255, transparency), 0, Vector2.Zero, SpriteEffects.None, layerDepth);
            }
            if (!drawStackNumber) return;

            float ownScale = 0.5f + scaleSize;
            Game1.drawWithBorder(stack.ToString(), Color.Black, Color.White, location + new Vector2(Game1.tileSize - Game1.tinyFont.MeasureString(stack.ToString()).X * ownScale, Game1.tileSize - (float)((double)Game1.tinyFont.MeasureString(stack.ToString()).Y * 3.0f / 4.0f) * ownScale), 0.0f, ownScale, 1f, true);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (Texture == null) return;

            var targSize = 64;
            var midX = (int)((objectPosition.X) + 32);
            var midY = (int)((objectPosition.Y) + 32);

            var targX = midX - targSize / 2;
            var targY = midY - targSize / 2;

            spriteBatch.Draw(Texture, new Rectangle(targX, targY, targSize, targSize), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, (f.getStandingY() + 2) / 10000f);
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
            var toRet = new SObject
            {
                Name = this.Name,
                CategoryName = this.CategoryName,
                Description = this.Description,
                Texture = this.Texture,
                IsPassable = this.IsPassable,
                IsPlaceable = this.IsPlaceable,
                quality = this.quality,
                scale = this.scale,
                isSpawnedObject = this.isSpawnedObject,
                isRecipe = this.isRecipe,
                questItem = this.questItem,
                stack = 1,
                HasBeenRegistered = this.HasBeenRegistered,
                RegisteredId = this.RegisteredId
            };


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
            
            Vector2 key = new Vector2(x, y);

            if (!canBePlacedHere(location, key))
                return false;

            SObject s = Clone();

            s.PlacedAt = key;
            s.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);

            location.objects.Add(key, s);
            Log.Verbose($"{this.GetHashCode()} - {s.GetHashCode()}");

            return true;
        }

        public override void actionOnPlayerEntry()
        {
            //base.actionOnPlayerEntry();
        }

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            if (!canBePlacedHere(location, CurrentMouse)) return;

            var x = Game1.oldMouseState.X + Game1.viewport.X;
            var y = Game1.oldMouseState.Y + Game1.viewport.Y;
            spriteBatch.Draw(Game1.mouseCursors, new Vector2(x / Game1.tileSize * Game1.tileSize - Game1.viewport.X, y / Game1.tileSize * Game1.tileSize - Game1.viewport.Y), new Rectangle(Utility.playerCanPlaceItemHere(location, this, x, y, Game1.player) ? 194 : 210, 388, 16, 16), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.01f);
        }
    }
}

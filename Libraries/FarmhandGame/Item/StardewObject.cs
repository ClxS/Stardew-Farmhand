using Farmhand.API.Items;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using xTile.Dimensions;
using System.Runtime.Serialization;
using Farmhand.Logging;
using Farmhand.Attributes;

namespace Farmhand.Overrides.Game.Item
{
    public class StardewObject : StardewValley.Object
    {

        // All the constructors offered by StardewValley.Object
        public StardewObject() :
            base()
        {
        }

        public StardewObject(Vector2 vector, int Id, bool isRecipe = false) :
            base(vector, Id, isRecipe)
        {
        }

        public StardewObject(int parentSheetIndex, int initialStack, bool isRecipe = false, int price = -1, int quality = 0) :
            base(parentSheetIndex, initialStack, isRecipe, price, quality)
        {
        }

        public StardewObject(Vector2 tileLocation, int parentSheetIndex, int initialStack) :
            base(tileLocation, parentSheetIndex, initialStack)
        {
        }

        public StardewObject(Vector2 tileLocation, int parentSheetIndex, string name, bool canBeSetDown, bool canBeGrabbed, bool isHoedirt, bool isSpawnedObject) :
            base(tileLocation, parentSheetIndex, name, canBeSetDown, canBeGrabbed, isHoedirt, isSpawnedObject)
        {
        }

        // Overriden methods

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void actionOnPlayerEntry()
        {
            base.actionOnPlayerEntry();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void actionWhenBeingHeld(Farmer who)
        {
            base.actionWhenBeingHeld(who);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool actionWhenPurchased()
        {
            return base.actionWhenPurchased();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void actionWhenStopBeingHeld(Farmer who)
        {
            base.actionWhenStopBeingHeld(who);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override int addToStack(int amount)
        {
            return base.addToStack(amount);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void addWorkingAnimation(GameLocation environment)
        {
            base.addWorkingAnimation(environment);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool canBeGivenAsGift()
        {
            return base.canBeGivenAsGift();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            return base.canBePlacedHere(l, tile);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool canBePlacedInWater()
        {
            return base.canBePlacedInWater();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool canBeShipped()
        {
            return base.canBeShipped();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool canBeTrashed()
        {
            return base.canBeTrashed();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            return base.checkForAction(who, justCheckingForActivity);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override string checkForSpecialItemHoldUpMeessage()
        {
            return base.checkForSpecialItemHoldUpMeessage();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool clicked(Farmer who)
        {
            return base.clicked(who);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void consumeRecipe(Farmer who)
        {
            base.consumeRecipe(who);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool countsForShippedCollection()
        {
            return base.countsForShippedCollection();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void DayUpdate(GameLocation location)
        {
            base.DayUpdate(location);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            base.draw(spriteBatch, x, y, alpha);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
        {
            base.draw(spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void drawAsProp(SpriteBatch b)
        {
            base.drawAsProp(b);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            base.drawPlacementBounds(spriteBatch, location);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            base.drawWhenHeld(spriteBatch, objectPosition, f);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void farmerAdjacentAction()
        {
            base.farmerAdjacentAction();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override Microsoft.Xna.Framework.Rectangle getBoundingBox(Vector2 tileLocation)
        {
            return base.getBoundingBox(tileLocation);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override Color getCategoryColor()
        {
            return base.getCategoryColor();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override string getCategoryName()
        {
            return base.getCategoryName();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override string getDescription()
        {
            return base.getDescription();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override int getHealth()
        {
            return base.getHealth();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override Vector2 getLocalPosition(xTile.Dimensions.Rectangle viewport)
        {
            return base.getLocalPosition(viewport);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override StardewValley.Item getOne()
        {
            return base.getOne();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override Vector2 getScale()
        {
            return base.getScale();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override int getStack()
        {
            return base.getStack();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void hoverAction()
        {
            base.hoverAction();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void initializeLightSource(Vector2 tileLocation)
        {
            base.initializeLightSource(tileLocation);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool isActionable(Farmer who)
        {
            return base.isActionable(who);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool isAnimalProduct()
        {
            return base.isAnimalProduct();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool isForage(GameLocation location)
        {
            return base.isForage(location);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool isPassable()
        {
            return base.isPassable();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool isPlaceable()
        {
            return base.isPlaceable();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override int maximumStackSize()
        {
            return base.maximumStackSize();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            return base.minutesElapsed(minutes, environment);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool onExplosion(Farmer who, GameLocation location)
        {
            return base.onExplosion(who, location);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool performDropDownAction(Farmer who)
        {
            return base.performDropDownAction(who);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
        {
            return base.performObjectDropInAction(dropIn, probe, who);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            base.performRemoveAction(tileLocation, environment);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool performToolAction(Tool t)
        {
            return base.performToolAction(t);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool performUseAction()
        {
            return base.performUseAction();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            return base.placementAction(location, x, y, who);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void reloadSprite()
        {
            base.reloadSprite();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void rot()
        {
            base.rot();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override int salePrice()
        {
            return base.salePrice();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override int sellToStorePrice()
        {
            return base.sellToStorePrice();
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void setHealth(int health)
        {
            base.setHealth(health);
        }

        /// <summary>
        /// Calling conditions, usage, and return value significance unkown
        /// </summary>
        public override void updateWhenCurrentLocation(GameTime time)
        {
            base.updateWhenCurrentLocation(time);
        }
    }
}
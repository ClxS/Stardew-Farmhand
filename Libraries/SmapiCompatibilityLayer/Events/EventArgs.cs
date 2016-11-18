using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Inheritance;
using StardewValley;
using StardewValley.Menus;
using Object = StardewValley.Object;

namespace StardewModdingAPI.Events
{
    public class EventArgsKeyboardStateChanged : EventArgs
    {
        public EventArgsKeyboardStateChanged(KeyboardState priorState, KeyboardState newState)
        {
            NewState = newState;
            PriorState = priorState;
        }

        public KeyboardState NewState { get; private set; }
        public KeyboardState PriorState { get; private set; }
    }

    public class EventArgsKeyPressed : EventArgs
    {
        public EventArgsKeyPressed(Keys keyPressed)
        {
            KeyPressed = keyPressed;
        }

        public Keys KeyPressed { get; private set; }
    }

    public class EventArgsControllerButtonPressed : EventArgs
    {
        public EventArgsControllerButtonPressed(PlayerIndex playerIndex, Buttons buttonPressed)
        {
            PlayerIndex = playerIndex;
            ButtonPressed = buttonPressed;
        }

        public PlayerIndex PlayerIndex { get; private set; }
        public Buttons ButtonPressed { get; private set; }
    }

    public class EventArgsControllerButtonReleased : EventArgs
    {
        public EventArgsControllerButtonReleased(PlayerIndex playerIndex, Buttons buttonReleased)
        {
            PlayerIndex = playerIndex;
            ButtonReleased = buttonReleased;
        }

        public PlayerIndex PlayerIndex { get; private set; }
        public Buttons ButtonReleased { get; private set; }
    }

    public class EventArgsControllerTriggerPressed : EventArgs
    {
        public EventArgsControllerTriggerPressed(PlayerIndex playerIndex, Buttons buttonPressed, float value)
        {
            PlayerIndex = playerIndex;
            ButtonPressed = buttonPressed;
            Value = value;
        }

        public PlayerIndex PlayerIndex { get; private set; }
        public Buttons ButtonPressed { get; private set; }
        public float Value { get; private set; }
    }

    public class EventArgsControllerTriggerReleased : EventArgs
    {
        public EventArgsControllerTriggerReleased(PlayerIndex playerIndex, Buttons buttonReleased, float value)
        {
            PlayerIndex = playerIndex;
            ButtonReleased = buttonReleased;
            Value = value;
        }

        public PlayerIndex PlayerIndex { get; private set; }
        public Buttons ButtonReleased { get; private set; }
        public float Value { get; private set; }
    }

    public class EventArgsMouseStateChanged : EventArgs
    {
        public EventArgsMouseStateChanged(MouseState priorState, MouseState newState)
        {
            NewState = newState;
            PriorState = priorState;
        }

        public MouseState NewState { get; private set; }
        public MouseState PriorState { get; private set; }
    }

    public class EventArgsClickableMenuChanged : EventArgs
    {
        public EventArgsClickableMenuChanged(IClickableMenu priorMenu, IClickableMenu newMenu)
        {
            NewMenu = newMenu;
            PriorMenu = priorMenu;
        }

        public IClickableMenu NewMenu { get; private set; }
        public IClickableMenu PriorMenu { get; private set; }
    }

    public class EventArgsClickableMenuClosed : EventArgs
    {
        public EventArgsClickableMenuClosed(IClickableMenu priorMenu)
        {
            PriorMenu = priorMenu;
        }
        
        public IClickableMenu PriorMenu { get; private set; }
    }

    public class EventArgsGameLocationsChanged : EventArgs
    {
        public EventArgsGameLocationsChanged(List<GameLocation> newLocations)
        {
            NewLocations = newLocations;
        }

        public List<GameLocation> NewLocations { get; private set; }
    }

    public class EventArgsMineLevelChanged : EventArgs
    {
        public EventArgsMineLevelChanged(int previousMineLevel, int currentMineLevel)
        {
            PreviousMineLevel = previousMineLevel;
            CurrentMineLevel = currentMineLevel;
        }

        public int PreviousMineLevel { get; private set; }
        public int CurrentMineLevel { get; private set; }
    }

    public class EventArgsLocationObjectsChanged : EventArgs
    {
        public EventArgsLocationObjectsChanged(SerializableDictionary<Vector2, Object> newObjects)
        {
            NewObjects = newObjects;
        }

        public SerializableDictionary<Vector2, Object> NewObjects { get; private set; }
    }

    public class EventArgsCurrentLocationChanged : EventArgs
    {
        public EventArgsCurrentLocationChanged(GameLocation priorLocation, GameLocation newLocation)
        {
            NewLocation = newLocation;
            PriorLocation = priorLocation;
        }

        public GameLocation NewLocation { get; private set; }
        public GameLocation PriorLocation { get; private set; }
    }

    public class EventArgsFarmerChanged : EventArgs
    {
        public EventArgsFarmerChanged(Farmer priorFarmer, Farmer newFarmer)
        {
            NewFarmer = NewFarmer;
            PriorFarmer = PriorFarmer;
        }

        public Farmer NewFarmer { get; }
        public Farmer PriorFarmer { get; }
    }

    public class EventArgsInventoryChanged : EventArgs
    {
        public EventArgsInventoryChanged(List<Item> inventory, List<ItemStackChange> changedItems)
        {
            Inventory = inventory;
            Added = changedItems?.Where(n => n.ChangeType == ChangeType.Added).ToList() ?? new List<ItemStackChange>();
            Removed = changedItems?.Where(n => n.ChangeType == ChangeType.Removed).ToList() ?? new List<ItemStackChange>();
            QuantityChanged = changedItems?.Where(n => n.ChangeType == ChangeType.StackChange).ToList() ?? new List<ItemStackChange>();
        }

        public List<Item> Inventory { get; private set; }
        public List<ItemStackChange> Added { get; private set; }
        public List<ItemStackChange> Removed { get; private set; }
        public List<ItemStackChange> QuantityChanged { get; private set; }
    }

    public class EventArgsLevelUp : EventArgs
    {
        public enum LevelType
        {
            Combat,
            Farming,
            Fishing,
            Foraging,
            Mining,
            Luck
        }

        public EventArgsLevelUp(LevelType type, int newLevel)
        {
            Type = type;
            NewLevel = newLevel;
        }

        public EventArgsLevelUp(int type, int newLevel)
        {
            Type = LevelType.Luck;
            NewLevel = newLevel;
        }

        public LevelType Type { get; private set; }
        public int NewLevel { get; private set; }
    }

    public class EventArgsIntChanged : EventArgs
    {
        public EventArgsIntChanged(int priorInt, int newInt)
        {
            NewInt = newInt;
            PriorInt = priorInt;
        }

        public int NewInt { get; }
        public int PriorInt { get; }
    }

    public class EventArgsStringChanged : EventArgs
    {
        public EventArgsStringChanged(string priorString, string newString)
        {
            NewString = newString;
            PriorString = priorString;
        }

        public string NewString { get; private set; }
        public string PriorString { get; private set; }
    }

    public class EventArgsLoadedGameChanged : EventArgs
    {
        public EventArgsLoadedGameChanged(bool loadedGame)
        {
            LoadedGame = loadedGame;
        }

        public bool LoadedGame { get; private set; }
    }

    public class EventArgsNewDay : EventArgs
    {
        public EventArgsNewDay(int prevDay, int curDay, bool newDay)
        {
            PreviousDay = prevDay;
            CurrentDay = curDay;
            IsNewDay = newDay;
        }

        public int PreviousDay { get; private set; }
        public int CurrentDay { get; private set; }
        public bool IsNewDay { get; private set; }
    }

    public class EventArgsCommand : EventArgs
    {
        public EventArgsCommand(Command command)
        {
            Command = command;
        }

        public Command Command { get; private set; }
    }
}
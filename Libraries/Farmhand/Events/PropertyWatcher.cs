﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using Farmhand.Logging;

namespace Farmhand.Events
{
    public class PropertyWatcher
    {
        private static bool HasLoadFired { get; set; } = false;
        private KeyboardState KStateNow { get; set; }
        private KeyboardState KStatePrior { get; set; }
        private MouseState MStateNow { get; set; }
        private MouseState MStatePrior { get; set; }
        private Keys[] CurrentlyPressedKeys { get; set; }
        private Keys[] PreviouslyPressedKeys { get; set; } = new Keys[0];
        private Keys[] FramePressedKeys
        {
            get { return CurrentlyPressedKeys.Where(x => !PreviouslyPressedKeys.Contains(x)).ToArray(); }
        }
        private Keys[] FrameReleasedKeys
        {
            get { return PreviouslyPressedKeys.Where(x => !CurrentlyPressedKeys.Contains(x)).ToArray(); }
        }
        private Buttons[][] PreviouslyPressedButtons { get; set; } = new Buttons[4][];
        private IClickableMenu PreviousActiveMenu { get; set; }
        private GameLocation PreviousGameLocation { get; set; }
        private Farmer PreviousFarmer { get; set; }

        private int LastSaveProgress { get; set; }
        private int LastLoadProgress { get; set; }
        
        private bool WasButtonJustPressed(Buttons button, ButtonState buttonState, PlayerIndex stateIndex)
        {
            return buttonState == ButtonState.Pressed && !PreviouslyPressedButtons[(int)stateIndex].Contains(button);
        }

        private bool WasButtonJustReleased(Buttons button, ButtonState buttonState, PlayerIndex stateIndex)
        {
            return buttonState == ButtonState.Released && PreviouslyPressedButtons[(int)stateIndex].Contains(button);
        }

        private bool WasButtonJustPressed(Buttons button, float value, PlayerIndex stateIndex)
        {
            return WasButtonJustPressed(button, value > 0.2f ? ButtonState.Pressed : ButtonState.Released, stateIndex);
        }

        private bool WasButtonJustReleased(Buttons button, float value, PlayerIndex stateIndex)
        {
            return WasButtonJustReleased(button, value > 0.2f ? ButtonState.Pressed : ButtonState.Released, stateIndex);
        }

        public Buttons[] GetButtonsDown(PlayerIndex index)
        {
            var state = GamePad.GetState(index);
            var buttons = new List<Buttons>();
            if (!state.IsConnected) return buttons.ToArray();

            if (state.Buttons.A == ButtonState.Pressed) buttons.Add(Buttons.A);
            if (state.Buttons.B == ButtonState.Pressed) buttons.Add(Buttons.B);
            if (state.Buttons.Back == ButtonState.Pressed) buttons.Add(Buttons.Back);
            if (state.Buttons.BigButton == ButtonState.Pressed) buttons.Add(Buttons.BigButton);
            if (state.Buttons.LeftShoulder == ButtonState.Pressed) buttons.Add(Buttons.LeftShoulder);
            if (state.Buttons.LeftStick == ButtonState.Pressed) buttons.Add(Buttons.LeftStick);
            if (state.Buttons.RightShoulder == ButtonState.Pressed) buttons.Add(Buttons.RightShoulder);
            if (state.Buttons.RightStick == ButtonState.Pressed) buttons.Add(Buttons.RightStick);
            if (state.Buttons.Start == ButtonState.Pressed) buttons.Add(Buttons.Start);
            if (state.Buttons.X == ButtonState.Pressed) buttons.Add(Buttons.X);
            if (state.Buttons.Y == ButtonState.Pressed) buttons.Add(Buttons.Y);
            if (state.DPad.Up == ButtonState.Pressed) buttons.Add(Buttons.DPadUp);
            if (state.DPad.Down == ButtonState.Pressed) buttons.Add(Buttons.DPadDown);
            if (state.DPad.Left == ButtonState.Pressed) buttons.Add(Buttons.DPadLeft);
            if (state.DPad.Right == ButtonState.Pressed) buttons.Add(Buttons.DPadRight);
            if (state.Triggers.Left > 0.2f) buttons.Add(Buttons.LeftTrigger);
            if (state.Triggers.Right > 0.2f) buttons.Add(Buttons.RightTrigger);
            return buttons.ToArray();
        }

        public Buttons[] GetFramePressedButtons(PlayerIndex index)
        {
            GamePadState state = GamePad.GetState(index);
            List<Buttons> buttons = new List<Buttons>();
            if (state.IsConnected)
            {
                if (WasButtonJustPressed(Buttons.A, state.Buttons.A, index)) buttons.Add(Buttons.A);
                if (WasButtonJustPressed(Buttons.B, state.Buttons.B, index)) buttons.Add(Buttons.B);
                if (WasButtonJustPressed(Buttons.Back, state.Buttons.Back, index)) buttons.Add(Buttons.Back);
                if (WasButtonJustPressed(Buttons.BigButton, state.Buttons.BigButton, index)) buttons.Add(Buttons.BigButton);
                if (WasButtonJustPressed(Buttons.LeftShoulder, state.Buttons.LeftShoulder, index)) buttons.Add(Buttons.LeftShoulder);
                if (WasButtonJustPressed(Buttons.LeftStick, state.Buttons.LeftStick, index)) buttons.Add(Buttons.LeftStick);
                if (WasButtonJustPressed(Buttons.RightShoulder, state.Buttons.RightShoulder, index)) buttons.Add(Buttons.RightShoulder);
                if (WasButtonJustPressed(Buttons.RightStick, state.Buttons.RightStick, index)) buttons.Add(Buttons.RightStick);
                if (WasButtonJustPressed(Buttons.Start, state.Buttons.Start, index)) buttons.Add(Buttons.Start);
                if (WasButtonJustPressed(Buttons.X, state.Buttons.X, index)) buttons.Add(Buttons.X);
                if (WasButtonJustPressed(Buttons.Y, state.Buttons.Y, index)) buttons.Add(Buttons.Y);
                if (WasButtonJustPressed(Buttons.DPadUp, state.DPad.Up, index)) buttons.Add(Buttons.DPadUp);
                if (WasButtonJustPressed(Buttons.DPadDown, state.DPad.Down, index)) buttons.Add(Buttons.DPadDown);
                if (WasButtonJustPressed(Buttons.DPadLeft, state.DPad.Left, index)) buttons.Add(Buttons.DPadLeft);
                if (WasButtonJustPressed(Buttons.DPadRight, state.DPad.Right, index)) buttons.Add(Buttons.DPadRight);
                if (WasButtonJustPressed(Buttons.LeftTrigger, state.Triggers.Left, index)) buttons.Add(Buttons.LeftTrigger);
                if (WasButtonJustPressed(Buttons.RightTrigger, state.Triggers.Right, index)) buttons.Add(Buttons.RightTrigger);
            }
            return buttons.ToArray();
        }

        public Buttons[] GetFrameReleasedButtons(PlayerIndex index)
        {
            GamePadState state = GamePad.GetState(index);
            List<Buttons> buttons = new List<Buttons>();
            if (state.IsConnected)
            {
                if (WasButtonJustReleased(Buttons.A, state.Buttons.A, index)) buttons.Add(Buttons.A);
                if (WasButtonJustReleased(Buttons.B, state.Buttons.B, index)) buttons.Add(Buttons.B);
                if (WasButtonJustReleased(Buttons.Back, state.Buttons.Back, index)) buttons.Add(Buttons.Back);
                if (WasButtonJustReleased(Buttons.BigButton, state.Buttons.BigButton, index)) buttons.Add(Buttons.BigButton);
                if (WasButtonJustReleased(Buttons.LeftShoulder, state.Buttons.LeftShoulder, index)) buttons.Add(Buttons.LeftShoulder);
                if (WasButtonJustReleased(Buttons.LeftStick, state.Buttons.LeftStick, index)) buttons.Add(Buttons.LeftStick);
                if (WasButtonJustReleased(Buttons.RightShoulder, state.Buttons.RightShoulder, index)) buttons.Add(Buttons.RightShoulder);
                if (WasButtonJustReleased(Buttons.RightStick, state.Buttons.RightStick, index)) buttons.Add(Buttons.RightStick);
                if (WasButtonJustReleased(Buttons.Start, state.Buttons.Start, index)) buttons.Add(Buttons.Start);
                if (WasButtonJustReleased(Buttons.X, state.Buttons.X, index)) buttons.Add(Buttons.X);
                if (WasButtonJustReleased(Buttons.Y, state.Buttons.Y, index)) buttons.Add(Buttons.Y);
                if (WasButtonJustReleased(Buttons.DPadUp, state.DPad.Up, index)) buttons.Add(Buttons.DPadUp);
                if (WasButtonJustReleased(Buttons.DPadDown, state.DPad.Down, index)) buttons.Add(Buttons.DPadDown);
                if (WasButtonJustReleased(Buttons.DPadLeft, state.DPad.Left, index)) buttons.Add(Buttons.DPadLeft);
                if (WasButtonJustReleased(Buttons.DPadRight, state.DPad.Right, index)) buttons.Add(Buttons.DPadRight);
                if (WasButtonJustReleased(Buttons.LeftTrigger, state.Triggers.Left, index)) buttons.Add(Buttons.LeftTrigger);
                if (WasButtonJustReleased(Buttons.RightTrigger, state.Triggers.Right, index)) buttons.Add(Buttons.RightTrigger);
            }
            return buttons.ToArray();
        }
        
        internal void CheckForChanges()
        {
            try
            {
                CheckControlChanges();
                CheckPropertyChanges();
                CheckSaveEvent();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void CheckSaveEvent()
        {
            if (LastLoadProgress < 100)
            {
                if (Game1.currentLoader != null)
                {
                    var currentLoadProgress = Game1.currentLoader.Current;
                    if (currentLoadProgress != LastLoadProgress)
                    {
                        LastLoadProgress = currentLoadProgress;
                        SaveEvents.InvokeOnAfterSaveProgress(LastLoadProgress);
                    }

                    if (LastLoadProgress >= 100)
                    {
                        SaveEvents.InvokeOnAfterLoad();
                        HasLoadFired = true;
                    }
                }
            }

            var saveEnumerator = SaveGame.getSaveEnumerator();
            if (saveEnumerator != null)
            {
                var saveProgress = saveEnumerator.Current;
                if (saveProgress > 0)
                {
                    if (LastSaveProgress < 100)
                    {
                        if (saveProgress != LastSaveProgress)
                        {
                            LastSaveProgress = saveProgress;
                            SaveEvents.InvokeOnAfterSaveProgress(LastLoadProgress);
                        }

                        if (LastSaveProgress >= 100)
                        {
                            SaveEvents.InvokeOnAfterSave();
                        }
                    }
                }
            }
        }

        private void CheckPropertyChanges()
        {
            if (Game1.activeClickableMenu != null && Game1.activeClickableMenu != PreviousActiveMenu)
            {
                MenuEvents.InvokeMenuChanged(PreviousActiveMenu, Game1.activeClickableMenu);
                PreviousActiveMenu = Game1.activeClickableMenu;
            }
            
            if (Game1.currentLocation != PreviousGameLocation)
            {
                Farmhand.Events.LocationEvents.InvokeCurrentLocationChanged(PreviousGameLocation, Game1.currentLocation);
                PreviousGameLocation = Game1.currentLocation;
            }
                        
            if (PropertyWatcher.HasLoadFired && Game1.player != PreviousFarmer)
            {
                var previous = PreviousFarmer;
                PreviousFarmer = Game1.player;
                Log.Success($"Farmer Changed: {Game1.player?.name} - previous: {PreviousFarmer?.name}");
                Farmhand.Events.PlayerEvents.InvokeFarmerChanged(previous, Game1.player);                
            }
        }


        private void CheckControlChanges()
        {
            KStateNow = Keyboard.GetState();
            CurrentlyPressedKeys = KStateNow.GetPressedKeys();

            MStateNow = Mouse.GetState();

            foreach (var k in FramePressedKeys)
                ControlEvents.InvokeKeyPressed(k);

            foreach (var k in FrameReleasedKeys)
                ControlEvents.InvokeKeyReleased(k);

            for (var i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                var buttons = GetFramePressedButtons(i);
                foreach (var b in buttons)
                {
                    if (b == Buttons.LeftTrigger || b == Buttons.RightTrigger)
                    {
                        ControlEvents.InvokeTriggerPressed(i, b, b == Buttons.LeftTrigger ? GamePad.GetState(i).Triggers.Left : GamePad.GetState(i).Triggers.Right);
                    }
                    else
                    {
                        ControlEvents.InvokeButtonPressed(i, b);
                    }
                }
            }

            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                foreach (Buttons b in GetFrameReleasedButtons(i))
                {
                    if (b == Buttons.LeftTrigger || b == Buttons.RightTrigger)
                    {
                        ControlEvents.InvokeTriggerReleased(i, b, b == Buttons.LeftTrigger ? GamePad.GetState(i).Triggers.Left : GamePad.GetState(i).Triggers.Right);
                    }
                    else
                    {
                        ControlEvents.InvokeButtonReleased(i, b);
                    }
                }
            }


            if (KStateNow != KStatePrior)
            {
                ControlEvents.InvokeKeyboardChanged(KStatePrior, KStateNow);
                KStatePrior = KStateNow;
            }

            if (MStateNow != MStatePrior)
            {
                ControlEvents.InvokeMouseChanged(MStatePrior, MStateNow);
                MStatePrior = MStateNow;
            }

            PreviouslyPressedKeys = CurrentlyPressedKeys;
            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                PreviouslyPressedButtons[(int)i] = GetButtonsDown(i);
            }
        }
    }
}

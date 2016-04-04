using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace Farmhand.Events
{
    internal class PropertyWatcher
    {
        internal KeyboardState KStateNow { get; private set; }
        internal KeyboardState KStatePrior { get; private set; }
        
        internal MouseState MStateNow { get; private set; }
        internal MouseState MStatePrior { get; private set; }

        internal Keys[] CurrentlyPressedKeys { get; private set; }
        internal Keys[] PreviouslyPressedKeys { get; private set; } = new Keys[0];

        internal Keys[] FramePressedKeys
        {
            get { return CurrentlyPressedKeys.Where(x => !PreviouslyPressedKeys.Contains(x)).ToArray(); }
        }
        internal Keys[] FrameReleasedKeys
        {
            get { return PreviouslyPressedKeys.Where(x => !CurrentlyPressedKeys.Contains(x)).ToArray(); }
        }

        internal Buttons[][] PreviouslyPressedButtons { get; set; } = new Buttons[4][];

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

        private Buttons[] GetButtonsDown(PlayerIndex index)
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

        private Buttons[] GetFramePressedButtons(PlayerIndex index)
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

        private Buttons[] GetFrameReleasedButtons(PlayerIndex index)
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

        private IClickableMenu PreviousActiveMenu { get; set; } = null;
        public GameLocation PreviousGameLocation { get; private set; }
        public Farmer PreviousFarmer { get; private set; }

        internal void CheckForChanges()
        {
            try
            {
                CheckControlChanges();
                CheckPropertyChanges();
            }
            catch (Exception)
            {
                // ignored
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

            if (Game1.player != null && Game1.player != PreviousFarmer)
            {
                Farmhand.Events.PlayerEvents.InvokeFarmerChanged(PreviousFarmer, Game1.player);
                PreviousFarmer = Game1.player;
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

namespace Farmhand.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Logging;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using StardewValley;
    using StardewValley.Menus;

    internal class PropertyWatcher
    {
        private static bool HasLoadFired { get; set; }

        private KeyboardState KStateNow { get; set; }

        private KeyboardState KStatePrior { get; set; }

        private MouseState MStateNow { get; set; }

        private MouseState MStatePrior { get; set; }

        private Keys[] CurrentlyPressedKeys { get; set; }

        private Keys[] PreviouslyPressedKeys { get; set; } = new Keys[0];

        private Keys[] FramePressedKeys
        {
            get
            {
                return this.CurrentlyPressedKeys.Where(x => !this.PreviouslyPressedKeys.Contains(x)).ToArray();
            }
        }

        private Keys[] FrameReleasedKeys
        {
            get
            {
                return this.PreviouslyPressedKeys.Where(x => !this.CurrentlyPressedKeys.Contains(x)).ToArray();
            }
        }

        private Buttons[][] PreviouslyPressedButtons { get; } = new Buttons[4][];

        private IClickableMenu PreviousActiveMenu { get; set; }

        private GameLocation PreviousGameLocation { get; set; }

        private Farmer PreviousFarmer { get; set; }

        private int LastSaveProgress { get; set; }

        private int LastLoadProgress { get; set; }

        private int HalfSecondPoll { get; set; }

        private bool WasButtonJustPressed(Buttons button, ButtonState buttonState, PlayerIndex stateIndex)
        {
            return buttonState == ButtonState.Pressed
                   && !this.PreviouslyPressedButtons[(int)stateIndex].Contains(button);
        }

        private bool WasButtonJustReleased(Buttons button, ButtonState buttonState, PlayerIndex stateIndex)
        {
            return buttonState == ButtonState.Released
                   && this.PreviouslyPressedButtons[(int)stateIndex].Contains(button);
        }

        private bool WasButtonJustPressed(Buttons button, float value, PlayerIndex stateIndex)
        {
            return this.WasButtonJustPressed(
                button,
                value > 0.2f ? ButtonState.Pressed : ButtonState.Released,
                stateIndex);
        }

        private bool WasButtonJustReleased(Buttons button, float value, PlayerIndex stateIndex)
        {
            return this.WasButtonJustReleased(
                button,
                value > 0.2f ? ButtonState.Pressed : ButtonState.Released,
                stateIndex);
        }

        public Buttons[] GetButtonsDown(PlayerIndex index)
        {
            var state = GamePad.GetState(index);
            var buttons = new List<Buttons>();
            if (!state.IsConnected)
            {
                return buttons.ToArray();
            }

            if (state.Buttons.A == ButtonState.Pressed)
            {
                buttons.Add(Buttons.A);
            }

            if (state.Buttons.B == ButtonState.Pressed)
            {
                buttons.Add(Buttons.B);
            }

            if (state.Buttons.Back == ButtonState.Pressed)
            {
                buttons.Add(Buttons.Back);
            }

            if (state.Buttons.BigButton == ButtonState.Pressed)
            {
                buttons.Add(Buttons.BigButton);
            }

            if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                buttons.Add(Buttons.LeftShoulder);
            }

            if (state.Buttons.LeftStick == ButtonState.Pressed)
            {
                buttons.Add(Buttons.LeftStick);
            }

            if (state.Buttons.RightShoulder == ButtonState.Pressed)
            {
                buttons.Add(Buttons.RightShoulder);
            }

            if (state.Buttons.RightStick == ButtonState.Pressed)
            {
                buttons.Add(Buttons.RightStick);
            }

            if (state.Buttons.Start == ButtonState.Pressed)
            {
                buttons.Add(Buttons.Start);
            }

            if (state.Buttons.X == ButtonState.Pressed)
            {
                buttons.Add(Buttons.X);
            }

            if (state.Buttons.Y == ButtonState.Pressed)
            {
                buttons.Add(Buttons.Y);
            }

            if (state.DPad.Up == ButtonState.Pressed)
            {
                buttons.Add(Buttons.DPadUp);
            }

            if (state.DPad.Down == ButtonState.Pressed)
            {
                buttons.Add(Buttons.DPadDown);
            }

            if (state.DPad.Left == ButtonState.Pressed)
            {
                buttons.Add(Buttons.DPadLeft);
            }

            if (state.DPad.Right == ButtonState.Pressed)
            {
                buttons.Add(Buttons.DPadRight);
            }

            if (state.Triggers.Left > 0.2f)
            {
                buttons.Add(Buttons.LeftTrigger);
            }

            if (state.Triggers.Right > 0.2f)
            {
                buttons.Add(Buttons.RightTrigger);
            }

            return buttons.ToArray();
        }

        public Buttons[] GetFramePressedButtons(PlayerIndex index)
        {
            var state = GamePad.GetState(index);
            var buttons = new List<Buttons>();
            if (state.IsConnected)
            {
                if (this.WasButtonJustPressed(Buttons.A, state.Buttons.A, index))
                {
                    buttons.Add(Buttons.A);
                }

                if (this.WasButtonJustPressed(Buttons.B, state.Buttons.B, index))
                {
                    buttons.Add(Buttons.B);
                }

                if (this.WasButtonJustPressed(Buttons.Back, state.Buttons.Back, index))
                {
                    buttons.Add(Buttons.Back);
                }

                if (this.WasButtonJustPressed(Buttons.BigButton, state.Buttons.BigButton, index))
                {
                    buttons.Add(Buttons.BigButton);
                }

                if (this.WasButtonJustPressed(Buttons.LeftShoulder, state.Buttons.LeftShoulder, index))
                {
                    buttons.Add(Buttons.LeftShoulder);
                }

                if (this.WasButtonJustPressed(Buttons.LeftStick, state.Buttons.LeftStick, index))
                {
                    buttons.Add(Buttons.LeftStick);
                }

                if (this.WasButtonJustPressed(Buttons.RightShoulder, state.Buttons.RightShoulder, index))
                {
                    buttons.Add(Buttons.RightShoulder);
                }

                if (this.WasButtonJustPressed(Buttons.RightStick, state.Buttons.RightStick, index))
                {
                    buttons.Add(Buttons.RightStick);
                }

                if (this.WasButtonJustPressed(Buttons.Start, state.Buttons.Start, index))
                {
                    buttons.Add(Buttons.Start);
                }

                if (this.WasButtonJustPressed(Buttons.X, state.Buttons.X, index))
                {
                    buttons.Add(Buttons.X);
                }

                if (this.WasButtonJustPressed(Buttons.Y, state.Buttons.Y, index))
                {
                    buttons.Add(Buttons.Y);
                }

                if (this.WasButtonJustPressed(Buttons.DPadUp, state.DPad.Up, index))
                {
                    buttons.Add(Buttons.DPadUp);
                }

                if (this.WasButtonJustPressed(Buttons.DPadDown, state.DPad.Down, index))
                {
                    buttons.Add(Buttons.DPadDown);
                }

                if (this.WasButtonJustPressed(Buttons.DPadLeft, state.DPad.Left, index))
                {
                    buttons.Add(Buttons.DPadLeft);
                }

                if (this.WasButtonJustPressed(Buttons.DPadRight, state.DPad.Right, index))
                {
                    buttons.Add(Buttons.DPadRight);
                }

                if (this.WasButtonJustPressed(Buttons.LeftTrigger, state.Triggers.Left, index))
                {
                    buttons.Add(Buttons.LeftTrigger);
                }

                if (this.WasButtonJustPressed(Buttons.RightTrigger, state.Triggers.Right, index))
                {
                    buttons.Add(Buttons.RightTrigger);
                }
            }

            return buttons.ToArray();
        }

        public Buttons[] GetFrameReleasedButtons(PlayerIndex index)
        {
            var state = GamePad.GetState(index);
            var buttons = new List<Buttons>();
            if (state.IsConnected)
            {
                if (this.WasButtonJustReleased(Buttons.A, state.Buttons.A, index))
                {
                    buttons.Add(Buttons.A);
                }

                if (this.WasButtonJustReleased(Buttons.B, state.Buttons.B, index))
                {
                    buttons.Add(Buttons.B);
                }

                if (this.WasButtonJustReleased(Buttons.Back, state.Buttons.Back, index))
                {
                    buttons.Add(Buttons.Back);
                }

                if (this.WasButtonJustReleased(Buttons.BigButton, state.Buttons.BigButton, index))
                {
                    buttons.Add(Buttons.BigButton);
                }

                if (this.WasButtonJustReleased(Buttons.LeftShoulder, state.Buttons.LeftShoulder, index))
                {
                    buttons.Add(Buttons.LeftShoulder);
                }

                if (this.WasButtonJustReleased(Buttons.LeftStick, state.Buttons.LeftStick, index))
                {
                    buttons.Add(Buttons.LeftStick);
                }

                if (this.WasButtonJustReleased(Buttons.RightShoulder, state.Buttons.RightShoulder, index))
                {
                    buttons.Add(Buttons.RightShoulder);
                }

                if (this.WasButtonJustReleased(Buttons.RightStick, state.Buttons.RightStick, index))
                {
                    buttons.Add(Buttons.RightStick);
                }

                if (this.WasButtonJustReleased(Buttons.Start, state.Buttons.Start, index))
                {
                    buttons.Add(Buttons.Start);
                }

                if (this.WasButtonJustReleased(Buttons.X, state.Buttons.X, index))
                {
                    buttons.Add(Buttons.X);
                }

                if (this.WasButtonJustReleased(Buttons.Y, state.Buttons.Y, index))
                {
                    buttons.Add(Buttons.Y);
                }

                if (this.WasButtonJustReleased(Buttons.DPadUp, state.DPad.Up, index))
                {
                    buttons.Add(Buttons.DPadUp);
                }

                if (this.WasButtonJustReleased(Buttons.DPadDown, state.DPad.Down, index))
                {
                    buttons.Add(Buttons.DPadDown);
                }

                if (this.WasButtonJustReleased(Buttons.DPadLeft, state.DPad.Left, index))
                {
                    buttons.Add(Buttons.DPadLeft);
                }

                if (this.WasButtonJustReleased(Buttons.DPadRight, state.DPad.Right, index))
                {
                    buttons.Add(Buttons.DPadRight);
                }

                if (this.WasButtonJustReleased(Buttons.LeftTrigger, state.Triggers.Left, index))
                {
                    buttons.Add(Buttons.LeftTrigger);
                }

                if (this.WasButtonJustReleased(Buttons.RightTrigger, state.Triggers.Right, index))
                {
                    buttons.Add(Buttons.RightTrigger);
                }
            }

            return buttons.ToArray();
        }

        internal void CheckForChanges(GameTime gameTime)
        {
            try
            {
                this.CheckControlChanges();
                this.CheckPropertyChanges();
                this.CheckSaveEvent();
                this.CheckTimeEvents(gameTime);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void CheckTimeEvents(GameTime gameTime)
        {
            this.HalfSecondPoll += gameTime.ElapsedGameTime.Milliseconds;
            if (this.HalfSecondPoll > 500)
            {
                this.HalfSecondPoll = this.HalfSecondPoll % 500;
                GameEvents.OnHalfSecondTick();
            }
        }

        private void CheckSaveEvent()
        {
            if (this.LastLoadProgress < 100)
            {
                if (Game1.currentLoader != null)
                {
                    var currentLoadProgress = Game1.currentLoader.Current;
                    if (currentLoadProgress != this.LastLoadProgress)
                    {
                        this.LastLoadProgress = currentLoadProgress;
                        SaveEvents.OnAfterSaveProgress(this.LastLoadProgress);
                    }

                    if (this.LastLoadProgress >= 100)
                    {
                        SaveEvents.OnAfterLoad();
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
                    if (this.LastSaveProgress < 100)
                    {
                        if (saveProgress != this.LastSaveProgress)
                        {
                            this.LastSaveProgress = saveProgress;
                            SaveEvents.OnAfterSaveProgress(this.LastLoadProgress);
                        }

                        if (this.LastSaveProgress >= 100)
                        {
                            SaveEvents.OnAfterSave();
                        }
                    }
                }
            }
        }

        private void CheckPropertyChanges()
        {
            if (Game1.activeClickableMenu != null && Game1.activeClickableMenu != this.PreviousActiveMenu)
            {
                MenuEvents.OnMenuChanged(this.PreviousActiveMenu, Game1.activeClickableMenu);
                this.PreviousActiveMenu = Game1.activeClickableMenu;
            }

            if (Game1.currentLocation != this.PreviousGameLocation)
            {
                LocationEvents.OnCurrentLocationChanged(this.PreviousGameLocation, Game1.currentLocation);
                this.PreviousGameLocation = Game1.currentLocation;
            }

            if (HasLoadFired && Game1.player != this.PreviousFarmer)
            {
                var previous = this.PreviousFarmer;
                this.PreviousFarmer = Game1.player;
                Log.Success($"Farmer Changed: {Game1.player?.name} - previous: {this.PreviousFarmer?.name}");
                PlayerEvents.OnFarmerChanged(previous, Game1.player);
            }
        }

        private void CheckControlChanges()
        {
            this.KStateNow = Keyboard.GetState();
            this.CurrentlyPressedKeys = this.KStateNow.GetPressedKeys();

            this.MStateNow = Mouse.GetState();

            foreach (var k in this.FramePressedKeys)
            {
                ControlEvents.OnKeyPressed(k);
            }

            foreach (var k in this.FrameReleasedKeys)
            {
                ControlEvents.OnKeyReleased(k);
            }

            for (var i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                var buttons = this.GetFramePressedButtons(i);
                foreach (var b in buttons)
                {
                    if (b == Buttons.LeftTrigger || b == Buttons.RightTrigger)
                    {
                        ControlEvents.OnTriggerPressed(
                            i,
                            b,
                            b == Buttons.LeftTrigger ? GamePad.GetState(i).Triggers.Left : GamePad.GetState(i).Triggers.Right);
                    }
                    else
                    {
                        ControlEvents.OnButtonPressed(i, b);
                    }
                }
            }

            for (var i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                foreach (var b in this.GetFrameReleasedButtons(i))
                {
                    if (b == Buttons.LeftTrigger || b == Buttons.RightTrigger)
                    {
                        ControlEvents.OnTriggerReleased(
                            i,
                            b,
                            b == Buttons.LeftTrigger ? GamePad.GetState(i).Triggers.Left : GamePad.GetState(i).Triggers.Right);
                    }
                    else
                    {
                        ControlEvents.OnButtonReleased(i, b);
                    }
                }
            }

            if (this.KStateNow != this.KStatePrior)
            {
                ControlEvents.OnKeyboardChanged(this.KStatePrior, this.KStateNow);
                this.KStatePrior = this.KStateNow;
            }

            if (this.MStateNow != this.MStatePrior)
            {
                ControlEvents.OnMouseChanged(this.MStatePrior, this.MStateNow);
                this.MStatePrior = this.MStateNow;
            }

            this.PreviouslyPressedKeys = this.CurrentlyPressedKeys;
            for (var i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                this.PreviouslyPressedButtons[(int)i] = this.GetButtonsDown(i);
            }
        }
    }
}
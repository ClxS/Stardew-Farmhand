namespace ModTemplate.Pages
{
    // ReSharper disable StyleCop.SA1300
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.Logging;
    using Farmhand.Registries;
    using Farmhand.Registries.Containers;
    using Farmhand.UI;
    using Farmhand.UI.Containers;
    using Farmhand.UI.Generic;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using ModTemplate.Controls;

    using StardewValley;

    internal sealed class ModConfig : FrameworkMenu
    {
        private readonly Dictionary<SelectableTextComponent, string> optionSettings =
            new Dictionary<SelectableTextComponent, string>();

        private ConfirmationDialog confirmationDialog;

        private FrameworkMenu controlContainer;

        private ModConfigFieldsComponent fieldsComponent;

        private ScrollableCollectionComponent modList;

        public ModConfig()
            : base(FrameDimensions, false, false)
        {
            this.ConstructForm();
        }

        private static Rectangle FrameDimensions
        {
            get
            {
                var bounds = Game1.game1.Window.ClientBounds;
                return new Rectangle(0, 0, bounds.Width / Game1.pixelZoom, bounds.Height / Game1.pixelZoom);
            }
        }

        private void ConstructForm()
        {
            this.confirmationDialog = new ConfirmationDialog(this.ZoomEventRegion) { Visible = false };

            // TODO: Find a better way of assuming the default resolution.
            this.controlContainer =
                new FrameworkMenu(
                    new Point((int)(1280 * 0.90 / Game1.pixelZoom), (int)(720 * 0.90 / Game1.pixelZoom)),
                    false);
            var tab =
                new FormCollectionComponent(
                    new Rectangle(0, 0, this.ZoomEventRegion.Width, this.ZoomEventRegion.Height));

            tab.AddComponent(new TextComponent(new Point(10, 0), "Available Mods"));
            tab.AddComponent(new TextComponent(new Point(550 / Game1.pixelZoom, 0), "Mod Settings"));

            this.modList =
                new ScrollableCollectionComponent(
                    new Rectangle(0, 10, 300 / Game1.pixelZoom, this.controlContainer.ZoomEventRegion.Height - 10));

            this.fieldsComponent =
                new ModConfigFieldsComponent(
                    new Rectangle(
                        this.modList.ZoomEventRegion.Width,
                        10,
                        this.controlContainer.ZoomEventRegion.Width - this.modList.ZoomEventRegion.Width,
                        this.controlContainer.ZoomEventRegion.Height - 10));

            tab.AddComponent(this.modList);
            tab.AddComponent(this.fieldsComponent);

            this.controlContainer.AddComponent(tab);
            this.FloatingComponent = this.confirmationDialog;
            this.AddComponent(this.confirmationDialog);

            this.confirmationDialog.Close += this.ConfirmationDialog_Close;

            var closeButton =
                new ClickableTextureComponent(
                    new Rectangle(this.controlContainer.ZoomEventRegion.Width, -12, 12, 12),
                    Game1.mouseCursors,
                    null,
                    new Rectangle(0x151, 0x1ee, 12, 12));
            closeButton.Handler += this.CloseButton_Handler;

            this.controlContainer.AddComponent(closeButton);
        }

        private void PopulateModList()
        {
            var mods =
                ModRegistry.GetRegisteredItems()
                    .Where(m => m is ModManifest)
                    .Cast<ModManifest>()
                    .Where(m => m.Instance?.ConfigurationSettings != null)
                    .ToArray();
            var anySelected = 0;

            this.optionSettings.Clear();
            this.modList.ClearComponents();
            for (var i = 0; i < mods.Length; ++i)
            {
                var text = new SelectableTextComponent(new Point(0, 10 * i), mods[i].Name);
                text.Handler += this.ModSelected_Handler;
                text.Selected += this.Mod_Selected;
                this.modList.AddComponent(text);

                this.optionSettings[text] = mods[i].UniqueId.ThisId;

                if (anySelected == 2)
                {
                    text.IsSelected = true;
                }

                anySelected++;
            }
        }

        private void CloseButton_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            Action<bool> closeForm = (saveChanges) =>
            {
                if (saveChanges)
                {
                    this.fieldsComponent.SaveChanges();
                    this.fieldsComponent.Cleanup();
                }

                this.OnClose();
            };

            if (this.fieldsComponent.HaveFieldsChanged)
            {
                this.confirmationDialog.ConfirmHandler = closeForm;
                this.confirmationDialog.Visible = true;
            }
            else
            {
                closeForm(false);
            }
        }

        private void ConfirmationDialog_Close(object sender, EventArgs e)
        {
            this.confirmationDialog.ConfirmHandler = null;
            this.confirmationDialog.Visible = false;
        }

        private void Mod_Selected(object sender, EventArgs e)
        {
            var component = sender as SelectableTextComponent;
            if (component == null)
            {
                return;
            }

            if (!this.optionSettings.ContainsKey(component))
            {
                Log.Error("Mod Id could not be found for option " + component.Text);
                return;
            }

            var modId = this.optionSettings[component];
            var mod = ModRegistry.GetItem(modId) as ModManifest;
            if (mod != null)
            {
                this.fieldsComponent.SetMod(mod.Instance?.ConfigurationSettings);
            }
            else
            {
                Log.Error("Failed to find mod with id " + modId);
            }
        }

        private void ModSelected_Handler(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu)
        {
            var textComponent = component as SelectableTextComponent;
            if (textComponent != null && !textComponent.IsSelected)
            {
                Action<bool> changeField = (saveChanges) =>
                {
                    if (saveChanges)
                    {
                        this.fieldsComponent.SaveChanges();
                        this.fieldsComponent.Cleanup();
                    }

                    var otherItems =
                        this.modList.InteractiveComponents.Where(c => c is SelectableTextComponent)
                            .Cast<SelectableTextComponent>();
                    foreach (var item in otherItems)
                    {
                        item.IsSelected = false;
                    }

                    textComponent.IsSelected = true;
                };

                if (this.fieldsComponent.HaveFieldsChanged)
                {
                    this.confirmationDialog.ConfirmHandler = changeField;
                    this.confirmationDialog.Visible = true;
                }
                else
                {
                    changeField(false);
                }
            }
        }

        public void OnOpen()
        {
            this.PopulateModList();
        }

        public event EventHandler Close = delegate { };

        public override void draw(SpriteBatch b)
        {
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);

            DrawMenuRect(
                b,
                this.controlContainer.Area.X,
                this.controlContainer.Area.Y,
                this.controlContainer.Area.Width,
                this.controlContainer.Area.Height);

            var o = new Point(this.Area.X + Zoom10, this.Area.Y + Zoom10);
            foreach (var el in this.DrawOrder)
            {
                el.Draw(b, o);
            }

            this.controlContainer.draw(b);

            this.confirmationDialog.Draw(b, o);

            this.drawMouse(b);
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            if (!this.confirmationDialog.Visible)
            {
                this.controlContainer.performHoverAction(x, y);
            }
        }

        public override void update(GameTime time)
        {
            base.update(time);
            if (!this.confirmationDialog.Visible)
            {
                this.controlContainer.update(time);
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            if (!this.confirmationDialog.Visible)
            {
                this.controlContainer.receiveLeftClick(x, y, playSound);
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            if (!this.confirmationDialog.Visible)
            {
                this.controlContainer.receiveScrollWheelAction(direction);
            }
        }

        private void OnClose()
        {
            // We can't use the built-in exit menu method, because it messes up in the TitleMenu
            this.Close(this, EventArgs.Empty);
        }
    }
}
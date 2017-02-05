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

    using ModTemplate.Controls;

    using StardewValley;

    internal sealed class ModConfig : FrameworkMenu
    {
        private readonly Dictionary<SelectableTextComponent, string> optionSettings =
            new Dictionary<SelectableTextComponent, string>();

        private ColoredRectangleComponent background;

        private ConfirmationDialog confirmationDialog;

        private ModConfigFieldsComponent fieldsComponent;

        private ScrollableCollectionComponent modList;

        public ModConfig()
            : base(FrameDimensions, false, false)
        {
        }

        private static Rectangle FrameDimensions
        {
            get
            {
                var bounds = Game1.game1.Window.ClientBounds;
                return new Rectangle(
                    0,
                    0,
                    (int)((bounds.Width / Game1.pixelZoom) / Game1.options.zoomLevel),
                    (int)((bounds.Height / Game1.pixelZoom) / Game1.options.zoomLevel));
            }
        }

        private static Rectangle FrameDimensionsZoomed
        {
            get
            {
                var bounds = Game1.game1.Window.ClientBounds;
                return new Rectangle(
                    0,
                    0,
                    (int)(bounds.Width / Game1.options.zoomLevel),
                    (int)(bounds.Height / Game1.options.zoomLevel));
            }
        }

        private void ConstructForm()
        {
            this.Centered = true;
            var controlArea = new Rectangle(
                this.ZoomEventRegion.X,
                this.ZoomEventRegion.Y - 10,
                this.ZoomEventRegion.Width,
                this.ZoomEventRegion.Height);

            var tab = new FormCollectionComponent(controlArea);

            this.AddControlBackgroundRect(tab);
            this.AddControlModList(tab);
            this.AddControlModFields(tab);
            this.AddControlConfirmationDialog();
            this.AddControlButtons(tab);
            this.AddComponent(tab);
        }

        private void AddControlButtons(IComponentCollection tab)
        {
            var closeButton =
                new ClickableTextureComponent(
                    new Rectangle(tab.ZoomEventRegion.Width - 20, 0, 12, 12),
                    Game1.mouseCursors,
                    null,
                    new Rectangle(0x151, 0x1ee, 12, 12)) {
                                                            Layer = 1 
                                                         };
            closeButton.Handler += this.CloseButton_Handler;

            tab.AddComponent(closeButton);
        }

        private void AddControlConfirmationDialog()
        {
            this.confirmationDialog = new ConfirmationDialog(this.ZoomEventRegion) { Visible = false, Layer = 2 };
            this.confirmationDialog.Close += this.ConfirmationDialog_Close;
            this.AddComponent(this.confirmationDialog);
        }

        private void AddControlModFields(IComponentCollection tab)
        {
            tab.AddComponent(new LabelComponent(new Point(10, -6), "Available Mods"));

            this.fieldsComponent =
                new ModConfigFieldsComponent(
                    new Rectangle(
                        this.modList.ZoomEventRegion.Width,
                        10,
                        tab.ZoomEventRegion.Width - this.modList.ZoomEventRegion.Width - 20,
                        tab.ZoomEventRegion.Height - 30));
            tab.AddComponent(this.fieldsComponent);
        }

        private void AddControlModList(IComponentCollection tab)
        {
            tab.AddComponent(new LabelComponent(new Point(550 / Game1.pixelZoom, -6), "Mod Settings"));
            this.modList =
                new ScrollableCollectionComponent(
                    new Rectangle(0, 20, 300 / Game1.pixelZoom, tab.ZoomEventRegion.Height - 30));
            tab.AddComponent(this.modList);
        }

        private void AddControlBackgroundRect(FormCollectionComponent tab)
        {
            var backgroundRect = this.ZoomEventRegion;
            this.background =
                new ColoredRectangleComponent(
                    new Rectangle(
                        backgroundRect.X - 20,
                        backgroundRect.Y - 20,
                        backgroundRect.Width + 20,
                        backgroundRect.Height + 20),
                    Color.Black * 0.75f) {
                                            Layer = -2 
                                         };
            this.AddComponent(this.background);

            var frameRect = tab.ZoomEventRegion;

            var frame =
                new FrameComponent(new Rectangle(frameRect.X - 10, frameRect.Y, frameRect.Width, frameRect.Height))
                {
                    Layer
                        =
                        -1
                };
            this.AddComponent(frame);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);

            var xDiff = newBounds.Width - oldBounds.Width;
            var yDiff = newBounds.Height - oldBounds.Height;

            this.background.InflateRegion(xDiff, yDiff);

            this.confirmationDialog?.OnWindowResize(oldBounds, newBounds);
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

                if (anySelected == 0)
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
            this.SetArea();
            this.ConstructForm();
            this.PopulateModList();
        }

        private void SetArea()
        {
            this.Area = FrameDimensionsZoomed;
            this.initialize(this.Area.X, this.Area.Y, this.Area.Width, this.Area.Height, false);
        }

        public event EventHandler Close = delegate { };

        private void OnClose()
        {
            // We can't use the built-in exit menu method, because it messes up in the TitleMenu
            this.Close(this, EventArgs.Empty);
        }
    }
}
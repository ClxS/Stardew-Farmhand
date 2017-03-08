namespace Farmhand.UI
{
    using Farmhand.UI.Components.Interfaces;

    using Microsoft.Xna.Framework.Input;

    using StardewValley;

    /// <summary>
    /// A keyboard subscriber proxy.
    /// </summary>
    public class KeyboardSubscriberProxy : IKeyboardSubscriber
    {
        /// <summary>
        /// Gets or sets the component to pass events to.
        /// </summary>
        protected IKeyboardComponent Component { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardSubscriberProxy"/> class.
        /// </summary>
        /// <param name="component">
        /// The component to pass events to.
        /// </param>
        public KeyboardSubscriberProxy(IKeyboardComponent component)
        {
            this.Component = component;
        }

        #region IKeyboardSubscriber Members

        /// <summary>
        /// Gets or sets a value indicating whether the component is selected.
        /// </summary>
        public bool Selected
        {
            get
            {
                return this.Component.Selected;
            }

            set
            {
                this.Component.Selected = value;
            }
        }

        /// <summary>
        /// Called on receiving character input.
        /// </summary>
        /// <param name="chr">
        /// The received character.
        /// </param>
        public void RecieveTextInput(char chr)
        {
            if (this.Component.Selected)
            {
                this.Component.TextReceived(chr);
            }
        }

        /// <summary>
        /// Called on receiving text input.
        /// </summary>
        /// <param name="str">
        /// The received text.
        /// </param>
        public void RecieveTextInput(string str)
        {
            if (this.Component.Selected)
            {
                this.Component.TextReceived(str);
            }
        }

        /// <summary>
        /// Called on receiving a command.
        /// </summary>
        /// <param name="cmd">
        /// The received command.
        /// </param>
        public void RecieveCommandInput(char cmd)
        {
            if (this.Component.Selected)
            {
                this.Component.CommandReceived(cmd);
            }
        }

        /// <summary>
        /// Called on receiving a special key.
        /// </summary>
        /// <param name="key">
        /// The received key.
        /// </param>
        public void RecieveSpecialInput(Keys key)
        {
            if (this.Component.Selected)
            {
                this.Component.SpecialReceived(key);
            }
        }

        #endregion
    }
}
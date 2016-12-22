namespace Farmhand.UI.Base
{
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework.Input;

    /// <summary>
    ///     The base keyboard form component.
    /// </summary>
    public abstract class BaseKeyboardFormComponent : BaseFormComponent, IKeyboardComponent
    {
        #region IKeyboardComponent Members

        /// <summary>
        ///     Gets or sets a value indicating whether selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        ///     Called when a character is entered
        /// </summary>
        /// <param name="chr">
        ///     The character received
        /// </param>
        public virtual void TextReceived(char chr)
        {
        }

        /// <summary>
        ///     Called when a string is entered
        /// </summary>
        /// <param name="str">
        ///     The string received
        /// </param>
        public virtual void TextReceived(string str)
        {
        }

        /// <summary>
        ///     Called when a command is received
        /// </summary>
        /// <param name="cmd">
        ///     The command received
        /// </param>
        public virtual void CommandReceived(char cmd)
        {
        }

        /// <summary>
        ///     Called when a special key is received
        /// </summary>
        /// <param name="key">
        ///     The key received
        /// </param>
        public virtual void SpecialReceived(Keys key)
        {
        }

        #endregion
    }
}
namespace Farmhand.UI.Interfaces
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    ///     The KeyboardComponent interface.
    /// </summary>
    public interface IKeyboardComponent
    {
        /// <summary>
        ///     Gets or sets a value indicating whether selected.
        /// </summary>
        bool Selected { get; set; }

        /// <summary>
        ///     Called when a character is entered
        /// </summary>
        /// <param name="chr">
        ///     The character received
        /// </param>
        void TextReceived(char chr);

        /// <summary>
        ///     Called when a string is entered
        /// </summary>
        /// <param name="str">
        ///     The string received
        /// </param>
        void TextReceived(string str);

        /// <summary>
        ///     Called when a command is received
        /// </summary>
        /// <param name="cmd">
        ///     The command received
        /// </param>
        void CommandReceived(char cmd);

        /// <summary>
        ///     Called when a special key is received
        /// </summary>
        /// <param name="key">
        ///     The key received
        /// </param>
        void SpecialReceived(Keys key);
    }
}
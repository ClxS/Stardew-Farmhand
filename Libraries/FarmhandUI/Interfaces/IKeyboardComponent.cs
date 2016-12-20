namespace Farmhand.UI
{
    public interface IKeyboardComponent
    {
        bool Selected { get; set; }
        void TextReceived(char chr);
        void TextReceived(string str);
        void CommandReceived(char cmd);
        void SpecialReceived(Microsoft.Xna.Framework.Input.Keys key);
    }
}

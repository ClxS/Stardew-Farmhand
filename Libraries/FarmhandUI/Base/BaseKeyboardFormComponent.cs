using Farmhand.UI.Interfaces;

namespace Farmhand.UI.Base
{
    public abstract class BaseKeyboardFormComponent : BaseFormComponent, IKeyboardComponent
    {
        public bool Selected {get; set; }
        public virtual void TextReceived(char chr)
        {

        }
        public virtual void TextReceived(string str)
        {

        }
        public virtual void CommandReceived(char cmd)
        {

        }
        public virtual void SpecialReceived(Microsoft.Xna.Framework.Input.Keys key)
        {

        }
    }
}

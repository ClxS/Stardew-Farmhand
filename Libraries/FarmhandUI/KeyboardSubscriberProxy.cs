namespace Farmhand.UI
{
    public class KeyboardSubscriberProxy : StardewValley.IKeyboardSubscriber
    {
        protected IKeyboardComponent Component;
        public KeyboardSubscriberProxy(IKeyboardComponent component)
        {
            Component = component;
        }
        public bool Selected
        {
            get
            {
                return Component.Selected;
            }
            set
            {
                Component.Selected = value;
            }
        }
        public void RecieveTextInput(char chr)
        {
            if (Component.Selected)
                Component.TextReceived(chr);
        }
        public void RecieveTextInput(string str)
        {
            if (Component.Selected)
                Component.TextReceived(str);
        }
        public void RecieveCommandInput(char cmd)
        {
            if (Component.Selected)
                Component.CommandReceived(cmd);
        }
        public void RecieveSpecialInput(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (Component.Selected)
                Component.SpecialReceived(key);
        }
    }
}

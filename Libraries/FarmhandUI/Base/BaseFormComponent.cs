namespace Farmhand.UI.Base
{
    public abstract class BaseFormComponent : BaseInteractiveMenuComponent
    {
        public virtual bool Disabled { get; set;} = false;
    }
}
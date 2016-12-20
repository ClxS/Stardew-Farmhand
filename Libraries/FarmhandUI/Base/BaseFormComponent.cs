namespace Farmhand.UI
{
    abstract public class BaseFormComponent : BaseInteractiveMenuComponent
    {
        public virtual bool Disabled { get; set;} = false;
    }
}
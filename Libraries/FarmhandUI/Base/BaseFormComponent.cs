namespace Farmhand.UI.Base
{
    /// <summary>
    /// The base class for all components
    /// </summary>
    public abstract class BaseFormComponent : BaseInteractiveMenuComponent
    {
        /// <summary>
        /// Gets or sets a value indicating whether this component is disabled.
        /// </summary>
        public virtual bool Disabled { get; set; } = false;
    }
}
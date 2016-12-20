using Microsoft.Xna.Framework;

namespace Farmhand.UI
{
    public interface IComponentContainer
    {
        void ResetFocus();
        void GiveFocus(IInteractiveMenuComponent component);
        Rectangle EventRegion { get; }
        Rectangle ZoomEventRegion { get; }
        FrameworkMenu GetAttachedMenu();
    }
}

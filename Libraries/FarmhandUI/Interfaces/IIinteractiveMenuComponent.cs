using Microsoft.Xna.Framework;

namespace Farmhand.UI.Interfaces
{
    public interface IInteractiveMenuComponent : IMenuComponent
    {
        bool InBounds(Point position, Point offset);
        void LeftClick(Point position, Point offset);
        void RightClick(Point position, Point offset);
        void LeftHeld(Point position, Point offset);
        void LeftUp(Point position, Point offset);
        void HoverIn(Point position, Point offset);
        void HoverOut(Point position, Point offset);
        void HoverOver(Point position, Point offset);
        void FocusLost();
        void FocusGained();
        void Scroll(int direction, Point position, Point offset);
    }
}

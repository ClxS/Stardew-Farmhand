using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmhand.UI.Interfaces
{
    public interface IMenuComponent
    {
        void Update(GameTime t);
        void Draw(SpriteBatch b, Point offset);
        void Attach(IComponentContainer collection);
        void Detach(IComponentContainer collection);
        Point GetPosition();
        Rectangle GetRegion();
        bool Visible { get; set; }
        int Layer { get; set; }
        IComponentContainer Parent { get; }
    }
}

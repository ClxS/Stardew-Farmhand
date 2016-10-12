using Farmhand.API.Monsters;
using Microsoft.Xna.Framework;

namespace Farmhand.Overrides.Character
{
    public class Monster : StardewValley.Monsters.Monster
    {
        protected MonsterInformation Information = null;

        public Monster(MonsterInformation Information, Vector2 Position)
            : base(Information.Name, Position)
        {
            this.Information = Information;
        }
    }
}

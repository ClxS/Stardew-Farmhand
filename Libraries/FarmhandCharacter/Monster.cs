using Farmhand.API.Monsters;
using Farmhand.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

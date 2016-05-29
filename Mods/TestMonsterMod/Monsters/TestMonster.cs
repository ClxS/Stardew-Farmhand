using Farmhand.API.Monsters;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonsterMod.Monsters
{
    class TestMonster : Farmhand.Overrides.Character.Monster
    {

        public TestMonster(MonsterInformation Information, Vector2 Position)
            : base(Information, Position)
        {

        }
    }
}

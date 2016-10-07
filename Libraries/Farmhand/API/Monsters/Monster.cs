using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmhand.API.Monsters
{
    public class Monster
    {
        public static Dictionary<string, MonsterInformation> Monsters { get; } = new Dictionary<string, MonsterInformation>();

        public static void RegisterMonster(MonsterInformation monsterInformation)
        {
            if (Monsters.ContainsKey(monsterInformation.Name) && Monsters[monsterInformation.Name] != monsterInformation)
            {
                Logging.Log.Warning($"Potential conflict registering new monster. Monster {monsterInformation.Name} has been registered by two separate mods." +
                                    $"Only the last registered one will be used.");
            }
            Monsters[monsterInformation.Name] = monsterInformation;
        }
    }
}

namespace Farmhand.API.Monsters
{
    using System.Collections.Generic;

    using Farmhand.Logging;

    /// <summary>
    ///     Monster-related API functionality.
    /// </summary>
    public static class Monster
    {
        /// <summary>
        ///     Gets the registered monsters.
        /// </summary>
        public static Dictionary<string, MonsterInformation> Monsters { get; } =
            new Dictionary<string, MonsterInformation>();

        /// <summary>
        ///     Registers a monster to the API.
        /// </summary>
        /// <param name="monsterInformation">
        ///     The monster's information.
        /// </param>
        public static void RegisterMonster(MonsterInformation monsterInformation)
        {
            if (Monsters.ContainsKey(monsterInformation.Name) && Monsters[monsterInformation.Name] != monsterInformation)
            {
                Log.Warning(
                    $"Potential conflict registering new monster. Monster {monsterInformation.Name} has been registered by two separate mods."
                    + "Only the last registered one will be used.");
            }

            Monsters[monsterInformation.Name] = monsterInformation;
        }
    }
}
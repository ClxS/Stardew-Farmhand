namespace Farmhand.API.NPCs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Farmhand.API.Locations;
    using Farmhand.Attributes;
    using Farmhand.Events;
    using Farmhand.Events.Arguments.GameEvents;
    using Farmhand.Events.Arguments.SaveEvents;
    using Farmhand.Logging;

    using StardewValley;

    /// <summary>
    ///     NPC-related API functionality.
    /// </summary>
    public static class Npc
    {
        private static bool afterGameLoadedFired;

        /// <summary>
        ///     Gets the list of registered NPC types.
        /// </summary>
        public static Dictionary<string, Type> NpcTypes { get; } = new Dictionary<string, Type>();

        /// <summary>
        ///     Gets the a dictionary of registered NPC information-types.
        /// </summary>
        public static Dictionary<string, Tuple<NpcInformation, Type>> Npcs { get; } =
            new Dictionary<string, Tuple<NpcInformation, Type>>();

        /// <summary>
        ///     Registers an NPC for us in the API.
        /// </summary>
        /// <param name="npcInformation">
        ///     The NPC information.
        /// </param>
        /// <typeparam name="T">
        ///     The type used to instantiate this NPC.
        /// </typeparam>
        public static void RegisterNpc<T>(NpcInformation npcInformation) where T : NPC, new()
        {
            if (Npcs.ContainsKey(npcInformation.Name) && Npcs[npcInformation.Name].Item1 != npcInformation)
            {
                Log.Warning(
                    $"Potential conflict registering new NPC. NPC {npcInformation.Name} has been registered by two seperate mods. Only the last registered one will be used.");
            }

            if (afterGameLoadedFired)
            {
                Log.Warning(
                    $"{npcInformation.Name} was registered after OnAfterGameLoaded. NPCs registered after OnAfterGameLoaded has fired will have to be manually added to their "
                    + "default locations.");
            }

            Npcs[npcInformation.Name] = new Tuple<NpcInformation, Type>(npcInformation, typeof(T));
        }

        [Hook(HookType.Entry, "StardewValley.Game1", ".ctor")]
        internal static void AttachListeners()
        {
            GameEvents.OnAfterGameLoaded += GameEvents_OnAfterGameLoaded;
            SaveEvents.OnAfterLoad += SaveEvents_OnAfterLoad;
        }

        private static void SaveEvents_OnAfterLoad(object sender, EventArgsOnAfterLoad e)
        {
            foreach (var npc in Npcs.Values)
            {
                var expectedLocation = Location.AllLocations.FirstOrDefault(l => l.name == npc.Item1.DefaultMap);
                var presentLocations = Location.AllLocations.Where(n => n.characters.Any(c => c.GetType() == npc.Item2));

                foreach (var location in presentLocations)
                {
                    location.characters.RemoveAll(c => c.GetType() == npc.Item2);
                }

                if (expectedLocation != null)
                {
                    var obj = (NPC)Activator.CreateInstance(npc.Item2);
                    expectedLocation.AddCharacter(obj);
                }
            }
        }

        private static void GameEvents_OnAfterGameLoaded(object sender, EventArgsOnAfterGameLoaded e)
        {
            afterGameLoadedFired = true;
            foreach (var npc in Npcs.Values)
            {
                var location = Location.AllLocations.FirstOrDefault(l => l.name == npc.Item1.DefaultMap);
                if (location != null)
                {
                    var obj = (NPC)Activator.CreateInstance(npc.Item2);
                    location.AddCharacter(obj);
                }
            }
        }
    }
}
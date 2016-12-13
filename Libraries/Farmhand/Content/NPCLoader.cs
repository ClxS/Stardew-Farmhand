using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Farmhand.Helpers;
using StardewValley;

namespace Farmhand.Content
{
    public class NPCLoader : IContentInjector
    {
        public bool IsInjector => false;
        public bool IsLoader => true;

        public List<string> NPCExceptions
            => Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters")
                .Select(file => file?.Replace("Content\\", "").Replace(".xnb", ""))
                .ToList();
        
        public bool HandlesAsset(Type type, string asset)
        {
            if (asset.StartsWith("Characters\\"))
                Logging.Log.Info($"Texture: {asset} => {asset.ContainsAny("\\Farmer", "\\Monsters", "\\Dialogue", "\\schedules")}");
            return !NPCExceptions.Any(_ => _.Equals(asset)) && (asset.StartsWith("Characters\\") && !asset.ContainsAny("\\Farmer", "\\Monsters", "\\Dialogue", "\\schedules"));
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\", "");
            var sprite = Farmhand.API.NPCs.NPC.NPCs[baseName].Texture;

            return (T) Convert.ChangeType(sprite, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Logging.Log.Error("You shouldn't be here!");
        }
    }

    public class NPCSchedulesLoader : IContentInjector
    {
        public bool IsLoader => true;
        public bool IsInjector => false;

        public List<string> SchedulesExceptions
            => Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters\\schedules")
                .Select(file => file?.Replace("Content\\", "").Replace(".xnb", ""))
                .ToList();

        public bool HandlesAsset(Type type, string asset)
        {
            if (asset.StartsWith("Characters\\"))
                Logging.Log.Info($"Schedules: {asset} => {asset.ContainsAny("\\Farmer", "\\Monsters", "\\Dialogue")}");
            return !SchedulesExceptions.Any(_ => _.Equals(asset)) && asset.StartsWith("Characters\\schedules\\");
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\schedules\\", "");
            var schedule = Farmhand.API.NPCs.NPC.NPCs[baseName].Schedules.BuildSchedule();

            return (T) Convert.ChangeType(schedule, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            throw new NotImplementedException();
        }
    }

    public class NPCDialogueLoader : IContentInjector
    {
        public bool IsLoader => true;
        public bool IsInjector => true;

        public List<string> DialoguesExceptions
            => Directory.GetFiles($"{Game1.content.RootDirectory}\\Characters\\Dialogue")
                .Select(file => file?.Replace("Content\\", "").Replace(".xnb", ""))
                .ToList();

        public bool HandlesAsset(Type type, string asset)
        {
            if (asset.StartsWith("Characters\\"))
                Logging.Log.Info($"Dialogue: {asset} => {asset.ContainsAny("\\Farmer", "\\Monsters", "\\schedules")}");
            return !DialoguesExceptions.Any(_ => _.Equals(asset)) && asset.StartsWith("Characters\\Dialogue\\");
        }

        public T Load<T>(ContentManager contentManager, string assetName)
        {
            var baseName = assetName.Replace("Characters\\Dialogue\\", "");
            var dialogues = Farmhand.API.NPCs.NPC.NPCs[baseName].Dialogues.BuildBaseDialogues();

            return (T) Convert.ChangeType(dialogues, typeof(T));
        }

        public void Inject<T>(T obj, string assetName, ref object output)
        {
            Logging.Log.Error("You shouldn't be here!");
        }
    }
}
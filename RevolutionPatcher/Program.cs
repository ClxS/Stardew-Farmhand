using ILRepacking;
using Revolution.Cecil;
using Revolution.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution
{
    class Program
    {
        static void Main(string[] args)
        {
            InjectRevolutionCoreClasses();
            CecilContext cecilContext = new CecilContext(Constants.IntermediateRevolutionExe);

            HookGameEvents(cecilContext);
            HookPlayerEvents(cecilContext);
            HookAnimalEvents(cecilContext);

            HookAPIEvents(cecilContext);
            
            cecilContext.WriteAssembly(Constants.RevolutionExe);          
        }

        static void HookGameEvents(CecilContext cecilContext)
        {
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.Game1", "Initialize", "Revolution.Events.GameEvents", "InvokeBeforeGameInitialise");
            CecilHelper.InjectExitMethod(cecilContext, "StardewValley.Game1", "Initialize", "Revolution.Events.GameEvents", "InvokeAfterGameInitialise");
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.Game1", "LoadContent", "Revolution.Events.GameEvents", "InvokeBeforeLoadContent");
            CecilHelper.InjectExitMethod(cecilContext, "StardewValley.Game1", "LoadContent", "Revolution.Events.GameEvents", "InvokeAfterLoadedContent");
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.Game1", "UnloadContent", "Revolution.Events.GameEvents", "InvokeBeforeUnloadContent");
            CecilHelper.InjectExitMethod(cecilContext, "StardewValley.Game1", "UnloadContent", "Revolution.Events.GameEvents", "InvokeAfterUnloadedContent");
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.Game1", "Update", "Revolution.Events.GameEvents", "InvokeBeforeUpdate");
            CecilHelper.InjectExitMethod(cecilContext, "StardewValley.Game1", "Update", "Revolution.Events.GameEvents", "InvokeAfterUpdate");
            
        }

        static void HookAnimalEvents(CecilContext cecilContext)
        {
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.FarmAnimal", "eatGrass", "Revolution.Events.FarmAnimalEvents", "InvokeOnEatGrass");
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.FarmAnimal", "makeSound", "Revolution.Events.FarmAnimalEvents", "InvokeOnMakeSound");
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.FarmAnimal", "farmerPushing", "Revolution.Events.FarmAnimalEvents", "InvokeOnFarmerPushing");
        }

        static void HookPlayerEvents(CecilContext cecilContext)
        {
            CecilHelper.InjectExitMethod(cecilContext, "StardewValley.Game1", "farmerTakeDamage", "Revolution.Events.PlayerEvents", "InvokeOnPlayerTakesDamage");
            CecilHelper.InjectExitMethod(cecilContext, "StardewValley.Game1", "doneEating", "Revolution.Events.PlayerEvents", "InvokeOnPlayerDoneEating");
        }

        static void HookAPIEvents(CecilContext cecilContext)
        {
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.Game1", ".ctor", "Revolution.ModLoader", "LoadMods");
        }

        static void InjectRevolutionCoreClasses()
        {
            RepackOptions options = new RepackOptions();
            ILogger logger = new RepackLogger();
            try
            {
                options.InputAssemblies = new string[] 
                {
                    Constants.StardewExe,
                    Constants.RevolutionDll
                };
                options.OutputFile = Constants.IntermediateRevolutionExe;
                options.SearchDirectories = new string[] { System.IO.Path.GetDirectoryName(Constants.CurrentAssemblyPath) };

                ILRepack repack = new ILRepack(options, logger);
                repack.Repack();
            }            
            catch (Exception e)
            {
                
            }
            finally
            {                
                if (options.PauseBeforeExit)
                {
                    Console.WriteLine("Press Any Key To Continue");
                    Console.ReadKey(true);
                }
            }
        }

        public class RepackLogger : ILogger
        {
            public bool ShouldLogVerbose
            {
                get
                {
                    return false;
                }

                set
                {
                    
                }
            }

            public void DuplicateIgnored(string ignoredType, object ignoredObject)
            {
               
            }

            public void Error(string msg)
            {

            }

            public void Info(string msg)
            {

            }

            public void Log(object str)
            {

            }

            public void Verbose(string msg)
            {

            }

            public void Warn(string msg)
            {

            }
        }

    }
}

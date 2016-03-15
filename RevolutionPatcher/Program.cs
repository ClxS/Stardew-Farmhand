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

            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.Game1", "Initialize", "Revolution.Events.GameEvents", "InvokeBeforeGameInitialise");
            CecilHelper.InjectExitMethod(cecilContext, "StardewValley.Game1", "Initialize", "Revolution.Events.GameEvents", "InvokeAfterGameInitialise");
            CecilHelper.InjectEntryMethod(cecilContext, "StardewValley.Game1", ".ctor", "Revolution.ModLoader", "LoadMods");

            cecilContext.WriteAssembly(Constants.RevolutionExe);          
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

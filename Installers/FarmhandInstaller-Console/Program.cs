namespace Farmhand.Installers
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Installers.Patcher;

    internal class Program
    {
        #region Pass enum

        public enum Pass
        {
            /// <summary>
            ///     First Pass
            /// </summary>
            PassOne,

            /// <summary>
            ///     Second Pass
            /// </summary>
            PassTwo
        }

        #endregion

        internal static void Main(string[] args)
        {
            Patcher.Patcher patcher;
            var grmDisabled = args.Any(a => a.Equals("-disablegrm"));
            var noObsolete = args.Any(a => a.Equals("-noobsolete"));

            if (args.Any(a => a.Equals("-pass1")))
            {
                patcher = CreatePatcher(Pass.PassOne);
                PatcherOptions.DisableGrm = grmDisabled;
                PatcherOptions.NoObsolete = noObsolete;
                if (noObsolete)
                {
                    PatcherOptions.OutputOverride = PatcherConstants.PassOneFarmhandExeNoObsolete;
                }

                patcher.PatchStardew();
            }
            else if (args.Any(a => a.Equals("-pass2")))
            {
                patcher = CreatePatcher(Pass.PassTwo);
                PatcherOptions.DisableGrm = grmDisabled;
                PatcherOptions.NoObsolete = noObsolete;
                if (noObsolete)
                {
                    PatcherOptions.OutputOverride = "Stardew Farmhand No-Obsolete.exe";
                }

                patcher.PatchStardew();
            }
            else
            {
                Console.WriteLine("Invalid build pass");
            }
        }

        internal static Patcher.Patcher CreatePatcher(Pass pass)
        {
            var patcherAssembly =
                Assembly.LoadFrom(
                    pass == Pass.PassOne ? "FarmhandPatcherFirstPass.dll" : "FarmhandPatcherSecondPass.dll");
            var patcherType =
                patcherAssembly.GetType(
                    pass == Pass.PassOne
                        ? "Farmhand.Installers.Patcher.PatcherFirstPass"
                        : "Farmhand.Installers.Patcher.PatcherSecondPass");
            var patcher = Activator.CreateInstance(patcherType);
            return patcher as Patcher.Patcher;
        }
    }
}
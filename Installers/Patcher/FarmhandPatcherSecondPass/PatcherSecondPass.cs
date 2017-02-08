namespace Farmhand.Installers.Patcher
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;

    using Farmhand.Attributes;
    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Injection;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers;
    using Farmhand.Installers.Patcher.Injection.Components.Parameters;
    using Farmhand.Installers.Patcher.PropertyConverters;

    /// <summary>
    ///     Performs the first-pass alterations to the game.
    /// </summary>
    public class PatcherSecondPass : Patcher
    {
        [Import]
        private IInjectionContext injectionContext;

        [Import]
        private IInjectionProcessor injector;

        [Import]
        private IAssemblyMerger merger;

        /// <summary>
        ///     Patches the game's executable.
        /// </summary>
        /// <param name="path">The final executable output path</param>
        public override void PatchStardew(string path = null)
        {
            this.merger = null;
            this.injector = null;
            this.injectionContext = null;

            var catalog =
                new AggregateCatalog(
                    new TypeCatalog(
                        typeof(RepackAssemblyMerger),
                        typeof(CecilInjectionProcessor<FarmhandHook>),
                        typeof(CecilContext)),
                    new TypeCatalog(
                        typeof(InputHandler<InputBindAttribute>),
                        typeof(LocalHandler<LocalBindAttribute>),
                        typeof(MethodOutputBindHandler<MethodOutputBindAttribute>),
                        typeof(ThisHandler<ThisBindAttribute>),
                        typeof(UseOutputHandler<UseOutputBindAttribute>)),
                    new TypeCatalog(
                        typeof(HookHandler<ParameterBindAttribute>),
                        typeof(HookHandlerAttributeConverter),
                        typeof(ReturnableHookHandler<ParameterBindAttribute>),
                        typeof(ReturnableHookHandlerAttributeConverter),
                        typeof(AlterBaseProtectionHandler),
                        typeof(AlterBaseFieldProtectionAttributeConverter),
                        typeof(AlterProtectionHandler),
                        typeof(AlterProtectionAttributeConverter),
                        typeof(ForceVirtualBaseHandler),
                        typeof(ForceVirtualBaseAttributeConverter),
                        typeof(RedirectConstructorFromBaseHandler),
                        typeof(RedirectConstructorFromBaseAttributeConverter),
                        typeof(RedirectConstructorToMethodHandler),
                        typeof(RedirectConstructorToMethodAttributeConverter),
                        typeof(MakeVirtualBaseCallHandler),
                        typeof(MakeVirtualBaseCallAttributeConverter),
                        typeof(MakeObsoleteHandler),
                        typeof(MakeObsoleteAttributeConverter)));
            this.InitialiseContainer(catalog);

            var repackOutput = this.GetAssemblyPath(PatcherConstants.PassTwoPackageResult);

            this.merger.Merge(
                repackOutput,
                PatcherConstants.PassOneFarmhandExe,
                PatcherConstants.FarmhandUiDll,
                PatcherConstants.FarmhandGameDll,
                PatcherConstants.FarmhandCharacterDll);

            this.injectionContext.SetPrimaryAssembly(repackOutput, true);
            this.injectionContext.LoadAssembly(this.GetAssemblyPath(PatcherConstants.FarmhandUiDll));
            this.injectionContext.LoadAssembly(
                this.GetAssemblyPath(PatcherConstants.FarmhandGameDll));
            this.injectionContext.LoadAssembly(
                this.GetAssemblyPath(PatcherConstants.FarmhandCharacterDll));

            this.injector.Inject();

            Console.WriteLine("Second Pass Installation Completed");

            path = path ?? PatcherConstants.FarmhandExe;
            var directory = Path.GetDirectoryName(path);

            if (directory == null)
            {
                throw new Exception("Path.GetDirectoryName(path) returned null");
            }

            Directory.CreateDirectory(directory);

            this.injectionContext.WriteAssembly(path, true);
        }
    }
}
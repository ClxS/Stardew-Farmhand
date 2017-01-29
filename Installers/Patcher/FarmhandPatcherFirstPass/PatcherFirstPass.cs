namespace Farmhand.Installers.Patcher
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;

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
    public class PatcherFirstPass : Patcher
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
        /// <param name="path">Game's executable Path</param>
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
                        typeof(MakeVirtualBaseCallAttributeConverter)));
            this.InitialiseContainer(catalog);

            path = path ?? PatcherConstants.StardewExe;
            var repackOutput = this.GetAssemblyPath(PatcherConstants.PassOnePackageResult);

            this.merger.Merge(
                repackOutput,
                path,
                PatcherConstants.FarmhandDll,
                PatcherConstants.JsonLibrary,
                PatcherConstants.MonoCecilLibrary,
                PatcherConstants.MonoCecilRocksLibrary);

            Assembly.LoadFrom(path);

            this.injectionContext.SetPrimaryAssembly(repackOutput, true);
            this.injectionContext.LoadAssembly(this.GetAssemblyPath(PatcherConstants.FarmhandDll));

            this.injector.Inject();

            Console.WriteLine("First Pass Installation Completed");

            this.injectionContext.WriteAssembly(
                this.GetAssemblyPath(PatcherConstants.PassOneFarmhandExe),
                true);
        }
    }
}
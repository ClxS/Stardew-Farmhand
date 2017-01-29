namespace Farmhand.Installers.Patcher.Injection
{
    /// <summary>
    ///     The CecilInjector interface.
    /// </summary>
    public interface IInjectionProcessor
    {
        /// <summary>
        ///     Injects Farmhand assemblies into Stardew and applies hooks.
        /// </summary>
        void Inject();
    }
}
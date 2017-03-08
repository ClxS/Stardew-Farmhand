namespace Farmhand.Installers.Patcher.Injection.Components.Hooks
{
    using System;

    /// <summary>
    ///     The HookHandler interface.
    /// </summary>
    public interface IHookHandler
    {
        /// <summary>
        ///     Performs the relevant hooking for this handler.
        /// </summary>
        /// <param name="attribute">
        ///     The attribute.
        /// </param>
        /// <param name="type">
        ///     Name of Type whose method is requesting this hook.
        /// </param>
        /// <param name="method">
        ///     Name of method requesting this hook.
        /// </param>
        void PerformAlteration(Attribute attribute, string type, string method);

        /// <summary>
        ///     Whether this hook matches the provided type name.
        /// </summary>
        /// <param name="fullName">
        ///     The full name.
        /// </param>
        /// <returns>
        ///     Whether the fullName is for this hook.
        /// </returns>
        bool Equals(string fullName);
    }
}
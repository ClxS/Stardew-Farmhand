namespace Farmhand.Events
{
    using System;
    using System.Reflection;

    using Farmhand.Events.Arguments;
    using Farmhand.Events.Arguments.ApiEvents;
    using Farmhand.Registries.Containers;

    /// <summary>
    ///     Contains events relating to the API
    /// </summary>
    public static class ApiEvents
    {
        /// <summary>
        ///     Fires when a mod throws an unhandled exception.
        /// </summary>
        public static event EventHandler<EventArgsOnModError> ModError = delegate { };

        /// <summary>
        ///     Fires just prior to loading the content/assembly of a mod.
        /// </summary>
        public static event EventHandler<EventArgsOnModLoadEvent> ModPreLoad = delegate { };

        /// <summary>
        ///     Fires just prior to after the content/assembly of a mod.
        /// </summary>
        public static event EventHandler<EventArgsOnModLoadEvent> ModPostLoad = delegate { };

        /// <summary>
        ///     Fires when an exception occurs during loading the content/assembly of a mod.
        /// </summary>
        public static event EventHandler<EventArgsOnModLoadEvent> ModLoadError = delegate { };

        internal static void OnModError(Assembly erroredAssembly, Exception ex)
        {
            EventCommon.SafeInvoke(ModError, null, new EventArgsOnModError(erroredAssembly, ex));
        }

        internal static void OnModPreLoad(ModManifest mod)
        {
            EventCommon.SafeInvoke(ModPreLoad, null, new EventArgsOnModLoadEvent(mod));
        }

        internal static void OnModPostLoad(ModManifest mod)
        {
            EventCommon.SafeInvoke(ModPostLoad, null, new EventArgsOnModLoadEvent(mod));
        }

        internal static void OnModLoadError(ModManifest mod)
        {
            EventCommon.SafeInvoke(ModLoadError, null, new EventArgsOnModLoadEvent(mod));
        }
    }
}
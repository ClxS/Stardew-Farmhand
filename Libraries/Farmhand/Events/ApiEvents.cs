using Farmhand.Events.Arguments;
using Farmhand.Events.Arguments.ApiEvents;
using Farmhand.Registries.Containers;
using System;
using System.Reflection;

namespace Farmhand.Events
{
    /// <summary>
    /// Contains events relating to the API
    /// </summary>
    public static class ApiEvents
    {
        /// <summary>
        /// Triggered when a mod throws an unhandled exception
        /// </summary>
        public static event EventHandler<EventArgsOnModError> OnModError = delegate { };

        /// <summary>
        /// Triggers just prior to loading the content/assembly of a mod
        /// </summary>
        public static event EventHandler<EventArgsOnModLoadEvent> OnModPreLoad = delegate { };

        /// <summary>
        /// Triggers just prior to after the content/assembly of a mod
        /// </summary>
        public static event EventHandler<EventArgsOnModLoadEvent> OnModPostLoad = delegate { };

        /// <summary>
        /// Triggers when an exception occurs during loading the content/assembly of a mod
        /// </summary>
        public static event EventHandler<EventArgsOnModLoadEvent> OnModLoadError = delegate { };


        internal static void InvokeOnModError(Assembly erroredAssembly, Exception ex)
        {
            EventCommon.SafeInvoke(OnModError, null, new EventArgsOnModError(erroredAssembly, ex));
        }

        internal static void InvokeModPreLoad(ModManifest mod)
        {
            EventCommon.SafeInvoke(OnModPreLoad, null, new EventArgsOnModLoadEvent(mod));
        }

        internal static void InvokeModPostLoad(ModManifest mod)
        {
            EventCommon.SafeInvoke(OnModPostLoad, null, new EventArgsOnModLoadEvent(mod));
        }

        internal static void InvokeModLoadError(ModManifest mod)
        {
            EventCommon.SafeInvoke(OnModLoadError, null, new EventArgsOnModLoadEvent(mod));
        }
    }
}

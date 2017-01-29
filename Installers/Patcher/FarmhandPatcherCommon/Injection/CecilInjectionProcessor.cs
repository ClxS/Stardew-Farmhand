namespace Farmhand.Installers.Patcher.Injection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Helpers;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;

    /// <summary>
    ///     Processes the hooking of Stardew using Cecil.
    /// </summary>
    /// <typeparam name="THookBase">
    ///     The Hook parameter base class.
    /// </typeparam>
    [Export(typeof(IInjectionProcessor))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CecilInjectionProcessor<THookBase> : IInjectionProcessor
        where THookBase : Attribute
    {
        private readonly CecilContext cecilContext;

        private readonly IEnumerable<IHookHandler> hookHandlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CecilInjectionProcessor{THookBase}" /> class.
        /// </summary>
        /// <param name="hooks">
        ///     An enumeration of hooks to perform.
        /// </param>
        /// <param name="injectionContext">
        ///     The injection context.
        /// </param>
        /// <exception cref="Exception">
        ///     Throws if an incompatible injectionContext is provided.
        /// </exception>
        [ImportingConstructor]
        public CecilInjectionProcessor(
            [ImportMany] IEnumerable<IHookHandler> hooks,
            IInjectionContext injectionContext)
        {
            this.hookHandlers = hooks;
            var context = injectionContext as CecilContext;
            if (context != null)
            {
                this.cecilContext = context;
            }
            else
            {
                throw new Exception(
                    $"CecilInjectionProcessor is only compatible with {nameof(CecilContext)}.");
            }
        }

        #region IInjectionProcessor Members

        /// <summary>
        ///     Injects Farmhand assemblies into Stardew and applies hooks.
        /// </summary>
        public void Inject()
        {
            this.InjectMethodHooks();
            this.InjectClassHooks();
        }

        private void InjectClassHooks()
        {
            var types =
                this.cecilContext.LoadedAssemblies.SelectMany(a => a.GetTypes())
                    .Where(m => m.GetCustomAttributes(typeof(THookBase), true).Any())
                    .ToArray();

            foreach (var type in types)
            {
                var typeName = type.FullName;
                var hookAttributes =
                    type.GetCustomAttributes(false).Cast<THookBase>();

                foreach (var hook in hookAttributes)
                {
                    var matchingHandlers =
                        this.hookHandlers.Where(h => h.Equals(hook.GetType().FullName));

                    foreach (var handler in matchingHandlers)
                    {
                        handler.PerformAlteration(hook, typeName, string.Empty);
                    }
                }
            }
        }

        #endregion

        private void InjectMethodHooks()
        {
            var methods =
                this.cecilContext.LoadedAssemblies.SelectMany(a => a.GetTypes())
                    .SelectMany(
                        t =>
                            t.GetMethods(
                                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
                                | BindingFlags.Static))
                    .Where(m => m.GetCustomAttributes(typeof(THookBase), true).Any())
                    .ToArray();

            foreach (var method in methods)
            {
                if (method.DeclaringType == null)
                {
                    continue;
                }

                var typeName = method.DeclaringType.FullName;
                var methodName = method.Name;
                var hookAttributes =
                    method.GetCustomAttributes(false).Cast<THookBase>();

                foreach (var hook in hookAttributes)
                {
                    var matchingHandlers =
                        this.hookHandlers.Where(h => h.Equals(hook.GetType().FullName));

                    foreach (var handler in matchingHandlers)
                    {
                        handler.PerformAlteration(hook, typeName, methodName);
                    }
                }
            }
        }
    }
}
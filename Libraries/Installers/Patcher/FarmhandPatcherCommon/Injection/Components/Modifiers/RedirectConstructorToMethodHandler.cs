namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Reflection;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Helpers;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;

    [Export(typeof(IHookHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RedirectConstructorToMethodHandler : IHookHandler
    {
        private readonly CecilContext cecilContext;

        private readonly IRedirectConstructorToMethodAttributeConverter propertyConverter;

        [ImportingConstructor]
        public RedirectConstructorToMethodHandler(
            IRedirectConstructorToMethodAttributeConverter propertyConverter,
            IInjectionContext injectionContext)
        {
            this.propertyConverter = propertyConverter;
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

        #region IHookHandler Members

        public void PerformAlteration(Attribute attribute, string typeName, string methodName)
        {
            this.propertyConverter.FromAttribute(attribute);
            
            var asmTypeSet =
                this.cecilContext.LoadedAssemblies.Select(a => a.GetType(typeName)).ToArray();
            if (!asmTypeSet.Any())
            {
                throw new Exception("No types found matching name " + typeName);
            }

            if (asmTypeSet.Length > 1)
            {
                throw new Exception("Multiple types found matching name " + typeName);
            }

            var type = asmTypeSet.First();

            var method = type.GetMethod(
                methodName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
                | BindingFlags.Static);
            if (method?.DeclaringType == null)
            {
                throw new Exception(
                    $"Could not find matching method ({methodName}) on type ({typeName})");
            }

            var asmType = method.ReturnType;

            var methodType = method.DeclaringType.FullName;

            var methodParameterInfo = method.GetParameters();
            var parameters = new Type[methodParameterInfo.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                parameters[i] = methodParameterInfo[i].ParameterType;
            }

            // Get a single string containing the full names of all parameters, comma separated
            var parametersString = string.Empty;
            for (var i = 0; i < parameters.Length; i++)
            {
                parametersString += parameters[i].FullName;
                if (i != parameters.Length - 1)
                {
                    parametersString += ",";
                }
            }
            
            var oldConstructor = asmType.GetConstructor(parameters);

            if (oldConstructor == null)
            {
                return;
            }

            // Build a FullName version of oldConstructor.Name
            var oldConstructorFullName = "System.Void " + asmType.FullName + "::.ctor" + "("
                                         + parametersString + ")";
            var oldConstructorReference =
                this.cecilContext.GetMethodDefinitionFullName(
                    asmType.FullName ?? asmType.Namespace + "." + asmType.Name,
                    oldConstructorFullName);

            // Build a FullName version of the new method
            string methodFullName = $"{asmType} {methodType}::{methodName}({parametersString})";
            var newMethod = this.cecilContext.GetMethodDefinitionFullName(
                methodType,
                methodFullName);
            if (newMethod == null)
            {
                return;
            }

            var processor = this.cecilContext.GetMethodIlProcessor(
                this.propertyConverter.Type,
                this.propertyConverter.Method);

            var instructions =
                processor.Body.Instructions.Where(
                        n => n.OpCode == OpCodes.Newobj && n.Operand == oldConstructorReference)
                    .ToList();
            foreach (var instruction in instructions)
            {
                processor.Body.SimplifyMacros();
                processor.Replace(instruction, processor.Call(newMethod));
                processor.Body.OptimizeMacros();
            }
        }

        public bool Equals(string fullName)
        {
            return this.propertyConverter.FullName == fullName;
        }

        #endregion
    }
}
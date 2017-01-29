namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;

    [Export(typeof(IHookHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RedirectConstructorFromBaseHandler : IHookHandler
    {
        private readonly CecilContext cecilContext;

        private readonly IRedirectConstructorFromBaseAttributeConverter propertyConverter;

        [ImportingConstructor]
        public RedirectConstructorFromBaseHandler(
            IRedirectConstructorFromBaseAttributeConverter propertyConverter,
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
                this.cecilContext.LoadedAssemblies.Select(a => a.GetType(typeName)).Where(n => n != null).ToArray();
            if (!asmTypeSet.Any())
            {
                throw new Exception("No types found matching name " + typeName);
            }

            if (asmTypeSet.Length > 1)
            {
                throw new Exception("Multiple types found matching name " + typeName);
            }

            var asmType = asmTypeSet.First();

            if (asmType.BaseType == null)
            {
                return;
            }

            if (this.propertyConverter.GenericArguments != null && this.propertyConverter.GenericArguments.Any())
            {
                this.PerformAlterationGeneric(asmType);
                return;
            }

            var parametersString = string.Empty;
            for (var i = 0; i < this.propertyConverter.Parameters.Length; i++)
            {
                if (this.propertyConverter.Parameters[i].FullName.Contains("`"))
                {
                    parametersString +=
                        this.ConvertGenericTypeFullNameToGenericTypeParameterFormat(
                            this.propertyConverter.Parameters[i].FullName);
                }
                else
                {
                    parametersString += this.propertyConverter.Parameters[i].FullName;
                }

                if (i != this.propertyConverter.Parameters.Length - 1)
                {
                    parametersString += ",";
                }
            }

            var newConstructor = asmType.GetConstructor(this.propertyConverter.Parameters);

            var oldConstructor = asmType.BaseType.GetConstructor(this.propertyConverter.Parameters);

            if (oldConstructor == null)
            {
                throw new NullReferenceException(
                    $"Failed to get matching constructor for type ({asmType.BaseType.FullName}) with parameters ({this.propertyConverter.Parameters})");
            }

            if (newConstructor == null)
            {
                throw new NullReferenceException(
                    $"Failed to get matching constructor for type ({asmType.FullName}) with parameters ({this.propertyConverter.Parameters})");
            }

            // Build a FullName version of newConstructor.Name
            var newConstructorFullName = "System.Void " + asmType.FullName + "::.ctor" + "("
                                         + parametersString + ")";
            var newConstructorReference =
                this.cecilContext.GetMethodDefinitionFullName(
                    asmType.FullName,
                    newConstructorFullName);

            if (newConstructorReference == null)
            {
                throw new NullReferenceException(
                    $"Failed to get matching method definition for constructor: {asmType.FullName}.{newConstructorFullName}");
            }

            // Build a FullName version of oldConstructor.Name
            var oldConstructorFullName = "System.Void " + asmType.BaseType.FullName + "::.ctor"
                                         + "(" + parametersString + ")";
            var oldConstructorReference =
                this.cecilContext.GetMethodDefinitionFullName(
                    asmType.BaseType.FullName
                    ?? asmType.BaseType.Namespace + "." + asmType.BaseType.Name,
                    oldConstructorFullName);

            var processor = this.cecilContext.GetMethodIlProcessor(
                this.propertyConverter.Type,
                this.propertyConverter.Method);

            var instructions =
                processor.Body.Instructions.Where(
                        n => n.OpCode == OpCodes.Newobj && n.Operand == oldConstructorReference)
                    .ToList();
            foreach (var instruction in instructions)
            {
                processor.Replace(
                    instruction,
                    processor.Create(OpCodes.Newobj, newConstructorReference));
            }
        }

        public bool Equals(string fullName)
        {
            return this.propertyConverter.FullName == fullName;
        }

        #endregion

        private void PerformAlterationGeneric(Type newConstructorType)
        {
            var types =
                this.propertyConverter.GenericArguments.Select(this.cecilContext.GetTypeReference)
                    .ToArray();
            var typeDef =
                this.cecilContext.GetTypeDefinition(
                    newConstructorType.Namespace + "." + newConstructorType.Name);
            if (newConstructorType.BaseType != null)
            {
                var typeDefBase =
                    this.cecilContext.GetTypeDefinition(
                        newConstructorType.BaseType.Namespace + "."
                        + newConstructorType.BaseType.Name);

                var newConstructorReference = this.cecilContext.GetConstructorReference(typeDef);
                var oldConstructorReference = this.cecilContext.GetConstructorReference(typeDefBase);

                var concreteFromConstructor = this.MakeHostInstanceGeneric(
                    oldConstructorReference,
                    types);
                var concreteToConstructor = this.MakeHostInstanceGeneric(
                    newConstructorReference,
                    types);

                var processor = this.cecilContext.GetMethodIlProcessor(
                    this.propertyConverter.Type,
                    this.propertyConverter.Method);

                var instructions =
                    processor.Body.Instructions.Where(
                        n =>
                            n.OpCode == OpCodes.Newobj && n.Operand is MethodReference
                            && ((MethodReference)n.Operand).FullName
                            == concreteFromConstructor.FullName).ToList();

                foreach (var instruction in instructions)
                {
                    processor.Replace(
                        instruction,
                        processor.Create(OpCodes.Newobj, concreteToConstructor));
                }
            }
        }

        private MethodReference MakeHostInstanceGeneric(
            MethodReference thisRef,
            params TypeReference[] arguments)
        {
            var reference = new MethodReference(
                                thisRef.Name,
                                thisRef.ReturnType,
                                thisRef.DeclaringType.MakeGenericInstanceType(arguments))
                                {
                                    HasThis
                                        =
                                        thisRef
                                            .HasThis,
                                    ExplicitThis
                                        =
                                        thisRef
                                            .ExplicitThis,
                                    CallingConvention
                                        =
                                        thisRef
                                            .CallingConvention
                                };

            foreach (var parameter in thisRef.Parameters)
            {
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));
            }

            foreach (var genericParameter in thisRef.GenericParameters)
            {
                reference.GenericParameters.Add(
                    new GenericParameter(genericParameter.Name, reference));
            }

            return reference;
        }

        private string ConvertGenericTypeFullNameToGenericTypeParameterFormat(
            string genericTypeFullName)
        {
            // Start with an empty string we'll build on
            var convertedString = string.Empty;

            // Everything before the first [[ can be directly lifted into the new string
            convertedString += genericTypeFullName.Split(new[] { "[[" }, StringSplitOptions.None)[0];

            // Open symbol
            convertedString += "<";

            // Spit the string after the first [[ by commas
            var commaSeparated =
                genericTypeFullName.Split(new[] { "[[" }, StringSplitOptions.None)[1].Split(',');

            // Iterate over the comma separated array. Every 5 commas is the string value we need, the rest is useless to us
            for (var c = 0; c < commaSeparated.Length; c += 5)
            {
                // Iterate over that string we got even further, in order to clean it up character by character
                for (var characterNum = 0; characterNum < commaSeparated[c].Length; characterNum++)
                {
                    // The only garbage symbol we should have to deal with is stray '[', but we can't get rid of them all, since we may need open and close brackets for arrays
                    // So, if a '[' doesn't have a matching ']' after it, it's garbage and can be ignored, while everything else gets tacked on our converted string
                    if (commaSeparated[c][characterNum].Equals('[')
                        && (characterNum >= commaSeparated[c].Length
                            || !commaSeparated[c][characterNum + 1].Equals(']')))
                    {
                        continue;
                    }

                    convertedString += commaSeparated[c][characterNum];
                }

                // Comma separation between the generic parameters
                if (c != commaSeparated.Length - 5)
                {
                    convertedString += ",";
                }
            }

            // Close symbol
            convertedString += ">";

            return convertedString;
        }
    }
}
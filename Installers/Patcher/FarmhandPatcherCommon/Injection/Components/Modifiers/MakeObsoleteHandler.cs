namespace Farmhand.Installers.Patcher.Injection.Components.Modifiers
{
    // ReSharper disable StyleCop.SA1600
    using System;
    using System.ComponentModel.Composition;

    using Farmhand.Installers.Patcher.Cecil;
    using Farmhand.Installers.Patcher.Injection.Components.Hooks;
    using Farmhand.Installers.Patcher.Injection.Components.Modifiers.Converters;

    using Mono.Cecil;

    [Export(typeof(IHookHandler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MakeObsoleteHandler : IHookHandler
    {
        private readonly CecilContext cecilContext;

        private readonly IMakeObsoleteAttributeConverter propertyConverter;

        [ImportingConstructor]
        public MakeObsoleteHandler(
            IMakeObsoleteAttributeConverter propertyConverter,
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
                throw new Exception($"CecilInjectionProcessor is only compatible with {nameof(CecilContext)}.");
            }
        }

        #region IHookHandler Members

        public void PerformAlteration(Attribute attribute, string typeName, string methodName)
        {
            this.propertyConverter.FromAttribute(attribute);

            if (this.propertyConverter.IsField)
            {
                var fieldDefinition = this.cecilContext.GetFieldDefinition(
                    this.propertyConverter.TypeName,
                    this.propertyConverter.ElementName);

                if (fieldDefinition == null)
                {
                    return;
                }

                fieldDefinition.CustomAttributes.Add(this.GetAttribute(typeName, methodName));
            }
            else if (this.propertyConverter.IsField)
            {
                var propertyDefinition = this.cecilContext.GetPropertyDefinition(
                    this.propertyConverter.TypeName,
                    this.propertyConverter.ElementName);

                if (propertyDefinition == null)
                {
                    return;
                }

                propertyDefinition.CustomAttributes.Add(this.GetAttribute(typeName, methodName));
            }
            else if (this.propertyConverter.IsField)
            {
                var methodDefinition = this.cecilContext.GetMethodDefinition(
                    this.propertyConverter.TypeName,
                    this.propertyConverter.ElementName);

                if (methodDefinition == null)
                {
                    return;
                }

                methodDefinition.CustomAttributes.Add(this.GetAttribute(typeName, methodName));
            }
        }

        private CustomAttribute GetAttribute(string typeName, string methodName)
        {
            var stringType = this.cecilContext.GetTypeReference(typeof(string));
            var boolType = this.cecilContext.GetTypeReference(typeof(bool));
            var generatedCodeCtor =
                    this.cecilContext.ImportMethod(typeof(ObsoleteAttribute).GetConstructor(new[] { typeof(string), typeof(bool) }));
            var result = new CustomAttribute(generatedCodeCtor);
            result.ConstructorArguments.Add(
                new CustomAttributeArgument(
                    stringType,
                    this.propertyConverter.Message ?? this.GetDefaultMessage("field", typeName, methodName)));
            result.ConstructorArguments.Add(new CustomAttributeArgument(boolType, this.propertyConverter.IsError));
            return result;
        }

        public bool Equals(string fullName)
        {
            return this.propertyConverter.FullName == fullName;
        }

        #endregion

        private string GetDefaultMessage(string elementTypeName, string typeName, string elementName)
        {
            return $"This {elementTypeName} is obsolete. Please use the safe API alternative: {typeName}.{elementName}";
        }
    }
}
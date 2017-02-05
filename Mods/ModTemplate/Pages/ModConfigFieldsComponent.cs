namespace ModTemplate.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using Farmhand;
    using Farmhand.UI;
    using Farmhand.UI.Base;
    using Farmhand.UI.Containers;
    using Farmhand.UI.Form;
    using Farmhand.UI.Generic;
    using Farmhand.UI.Interfaces;

    using Microsoft.Xna.Framework;

    internal class ModConfigFieldsComponent : FormCollectionComponent
    {
        private readonly List<Property> properties = new List<Property>();

        private ModSettings settingsInstance;

        public ModConfigFieldsComponent(Rectangle area)
            : base(area)
        {
        }

        public bool HaveFieldsChanged { get; set; }

        public void SetMod(ModSettings settings)
        {
            this.ClearForm();
            this.ReflectModProperties(settings);
            this.PopulateControls();
        }

        private void ClearForm()
        {
            this.HaveFieldsChanged = false;
            this.properties.Clear();
            this.settingsInstance = null;
            this.InteractiveComponents.Clear();
            this.StaticComponents.Clear();
        }

        private void PopulateControls()
        {
            var scrollCollection =
                new ScrollableCollectionComponent(
                    new Rectangle(0, 0, this.ZoomEventRegion.Width, this.ZoomEventRegion.Height));

            for (var i = 0; i < this.properties.Count; ++i)
            {
                var label = new TextComponent(new Point(0, 12 + 12 * i), this.properties[i].PropertyName);
                BaseFormComponent control = null;

                switch (this.properties[i].Type)
                {
                    case PropertyType.Boolean:
                        var checkbox = new CheckboxFormComponent(new Point(60, 10 + 12 * i), string.Empty)
                                       {
                                           Value =
                                               this
                                                   .properties
                                                   [
                                                       i
                                                   ]
                                                   .BoolValue
                                       };
                        checkbox.Handler += this.CheckBox_OnChange;
                        control = checkbox;
                        break;
                    case PropertyType.Number:
                        var intControl = new PlusMinusFormComponent(
                                             new Point(60, 10 + 12 * i),
                                             int.MinValue,
                                             int.MaxValue) {
                                                              Value = this.properties[i].IntValue 
                                                           };
                        intControl.Handler += this.IntControl_OnChange;
                        control = intControl;
                        break;
                    case PropertyType.String:
                        var tc = new TextboxFormComponent(new Point(60, 10 + 12 * i), 75, null, null)
                                 {
                                     Value =
                                         this
                                             .properties
                                             [i]
                                             .StringValue
                                 };
                        tc.Handler += this.TextControl_OnChange;
                        control = tc;
                        break;
                }

                this.properties[i].Control = control;

                if (control != null)
                {
                    scrollCollection.AddComponent(label);
                    scrollCollection.AddComponent(control);
                }
            }

            this.AddComponent(scrollCollection);
        }

        private void TextControl_OnChange(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu,
            string value)
        {
            this.HaveFieldsChanged = true;
            var property = this.properties.FirstOrDefault(n => n.Control == component);
            if (property != null)
            {
                property.StringValue = value;
            }
        }

        private void IntControl_OnChange(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu,
            int value)
        {
            this.HaveFieldsChanged = true;
            var property = this.properties.FirstOrDefault(n => n.Control == component);
            if (property != null)
            {
                property.IntValue = value;
            }
        }

        private void CheckBox_OnChange(
            IInteractiveMenuComponent component,
            IComponentContainer collection,
            FrameworkMenu menu,
            bool value)
        {
            this.HaveFieldsChanged = true;
            var property = this.properties.FirstOrDefault(n => n.Control == component);
            if (property != null)
            {
                property.BoolValue = value;
            }
        }

        private void ReflectModProperties(ModSettings settings)
        {
            this.settingsInstance = settings;

            var type = settings.GetType();
            var typeProperties = type.GetProperties();
            foreach (var reflectedProperty in typeProperties)
            {
                var propertyType = this.GetPropertyType(reflectedProperty);

                if (!reflectedProperty.CanRead || !reflectedProperty.CanWrite || propertyType == PropertyType.Unknown)
                {
                    continue;
                }

                var property = new Property
                               {
                                   Type = propertyType,
                                   IsWritable = reflectedProperty.CanWrite,
                                   PropertyInfo = reflectedProperty,
                                   PropertyName = this.PrettyFormatPropertyName(reflectedProperty.Name)
                               };

                switch (propertyType)
                {
                    case PropertyType.Boolean:
                        property.BoolValue = (bool)reflectedProperty.GetValue(this.settingsInstance, null);
                        break;
                    case PropertyType.Number:
                        property.IntValue = (int)reflectedProperty.GetValue(this.settingsInstance, null);
                        break;
                    case PropertyType.String:
                        property.StringValue = (string)reflectedProperty.GetValue(this.settingsInstance, null);
                        break;
                }

                this.properties.Add(property);
            }
        }

        private PropertyType GetPropertyType(PropertyInfo reflectedProperty)
        {
            switch (reflectedProperty.PropertyType.FullName)
            {
                case "System.Boolean":
                    return PropertyType.Boolean;
                case "System.String":
                    return PropertyType.String;
                case "System.Int32":
                    return PropertyType.Number;
                default:
                    return PropertyType.Unknown;
            }
        }

        private string PrettyFormatPropertyName(string reflectedPropertyName)
        {
            var result = Regex.Replace(
                Regex.Replace(reflectedPropertyName, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2");
            if (char.IsLower(result[0]))
            {
                var resultSb = new StringBuilder(result) { [0] = char.ToUpper(result[0]) };
                result = resultSb.ToString();
            }

            return result;
        }

        public void SaveChanges()
        {
            if (this.settingsInstance == null)
            {
                return;
            }

            foreach (var property in this.properties)
            {
                if (!property.IsWritable)
                {
                    continue;
                }

                switch (property.Type)
                {
                    case PropertyType.Boolean:
                        property.PropertyInfo.SetValue(this.settingsInstance, property.BoolValue, null);
                        break;
                    case PropertyType.Number:
                        property.PropertyInfo.SetValue(this.settingsInstance, property.IntValue, null);
                        break;
                    case PropertyType.String:
                        property.PropertyInfo.SetValue(this.settingsInstance, property.StringValue, null);
                        break;
                    case PropertyType.Unknown:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(property.Type));
                }
            }

            this.settingsInstance.Save();
        }

        #region Nested type: Property

        private class Property
        {
            public bool IsWritable { get; set; }

            public string PropertyName { get; set; }

            public PropertyType Type { get; set; }

            public PropertyInfo PropertyInfo { get; set; }

            public BaseFormComponent Control { get; set; }

            public string StringValue { get; set; }

            public int IntValue { get; set; }

            public bool BoolValue { get; set; }
        }

        #endregion

        #region Nested type: PropertyType

        private enum PropertyType
        {
            Unknown,

            String,

            Number,

            Boolean
        }

        #endregion
    }
}
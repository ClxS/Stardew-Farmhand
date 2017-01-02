namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Binds to a local variable by index.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class LocalBindAttribute : ParameterBindAttribute
    {
        public LocalBindAttribute(Type type, int index)
        {
            this.Type = type;
            this.Index = index;
        }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        public int Index { get; set; }
    }
}
namespace Farmhand.Attributes
{
    using System;

    /// <summary>
    ///     Used by the installer. Binds to an input parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InputBindAttribute : ParameterBindAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InputBindAttribute" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="ref">
        ///     The ref.
        /// </param>
        public InputBindAttribute(Type type, string name, bool @ref = false)
        {
            this.Type = type;
            this.Name = name;
            this.Ref = @ref;
        }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether ref.
        /// </summary>
        public bool Ref { get; set; }
    }
}
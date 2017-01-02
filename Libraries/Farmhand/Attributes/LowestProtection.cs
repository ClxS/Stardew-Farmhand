namespace Farmhand.Attributes
{
    /// <summary>
    ///     Used by the installer. Defines a minimum protection for protection rewriting.
    /// </summary>
    public enum LowestProtection
    {
        /// <summary>
        ///     Sets minimum to private.
        /// </summary>
        Private,

        /// <summary>
        ///     Sets minimum to protected.
        /// </summary>
        Protected,

        /// <summary>
        ///     Sets minimum to public.
        /// </summary>
        Public
    }
}
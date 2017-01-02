namespace Farmhand.Attributes
{
    /// <summary>
    ///     Used by the installer. Specifies where a call hook is inserted.
    /// </summary>
    public enum HookType
    {
        /// <summary>
        ///     At the start of a method.
        /// </summary>
        Entry,

        /// <summary>
        ///     Just before return.
        /// </summary>
        Exit
    }
}
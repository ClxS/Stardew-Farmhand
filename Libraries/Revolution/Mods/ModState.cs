namespace Revolution
{
    public enum ModState
    {
        Unloaded,
        Loaded,
        Deactivated,
        MissingDependency,
        Errored,
        ForciblyUnloaded,
        InvalidManifest
    }
}

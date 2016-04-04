namespace Farmhand.Registries.Containers
{
    public enum DependencyState
    {
        Ok,
        Missing,
        ParentMissing,
        TooLowVersion,
        TooHighVersion
    }
}
namespace Farmhand.API.Buildings
{
    public interface IBlueprint
    {
        string Name { get; set; }
        string BlueprintString { get; }
        bool IsCarpenterBlueprint { get; }
        
    }
}

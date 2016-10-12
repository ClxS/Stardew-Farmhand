using Microsoft.Xna.Framework;

namespace Farmhand.API.Buildings
{
    public class AnimalBlueprint : IBlueprint
    {
        public string Name { get; set; }
        public int MoneyRequired { get; set; } //Animal:[1]
        public Vector2 SourceViewRect { get; set; } //Animal:[2-3]
        public string Description { get; set; } //Animal:[4]

        public string BlueprintString => $"animal/{MoneyRequired}/{SourceViewRect.X}/{SourceViewRect.Y}/{Description}/null/Farm";
        public bool IsCarpenterBlueprint => false;
        
    }
}

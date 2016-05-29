namespace Farmhand.API.Generic
{
    public class ItemChancePair
    {
        public int ItemId;
        public double Chance;

        public override string ToString()
        {
            return $"{ItemId} {Chance}";
        }
    }
}
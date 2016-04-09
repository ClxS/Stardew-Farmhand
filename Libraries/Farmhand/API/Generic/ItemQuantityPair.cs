namespace Farmhand.API.Generic
{
    public class ItemQuantityPair
    {
        public int ItemId;
        public int Count;

        public override string ToString()
        {
            return $"{ItemId} {Count}";
        }
    }
}
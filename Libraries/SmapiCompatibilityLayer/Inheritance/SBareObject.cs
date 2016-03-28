namespace StardewModdingAPI.Inheritance
{
    public struct SBareObject
    {
        public int parentSheetIndex { get; set; }
        public int stack { get; set; }
        public bool isRecipe { get; set; }
        public int price { get; set; }
        public int quality { get; set; }

        public SBareObject(int psi, int sta, bool ir, int pri, int qua)
        {
            parentSheetIndex = psi;
            stack = sta;
            isRecipe = ir;
            price = pri;
            quality = qua;
        }
    }
}
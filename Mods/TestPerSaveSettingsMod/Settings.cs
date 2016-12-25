namespace TestPerSaveSettings
{
    using Farmhand;

    using Newtonsoft.Json;

    internal class Settings : ModSettings
    {
        [JsonIgnore]
        public override bool UseSaveSpecificConfiguration => true;

        public string TestStringGlobal { get; set; } = "Global";

        public int TestInt { get; set; } = 42;

        public int TestInt1 { get; set; } = 42;

        public int TestInt2 { get; set; } = 42;

        public int TestInt3 { get; set; } = 42;

        public int TestInt4 { get; set; } = 42;
    }
}

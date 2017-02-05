namespace TestPerSaveSettings
{
    using Farmhand;

    using Newtonsoft.Json;

    internal class Settings : ModSettings
    {
        [JsonIgnore]
        public override bool UseSaveSpecificConfiguration => true;

        public string TestStringGlobal { get; set; } = "Global";

        public string TestString2 { get; set; } = "Global";

        public bool TestBool { get; set; } = false;

        public bool Test2Bool { get; set; } = true;

        public int TestInt { get; set; } = 42;

        public int TestInt1 { get; set; } = 42;

        public int TestInt2 { get; set; } = 42;

        public int TestInt3 { get; set; } = 42;

        public int TestInt4 { get; set; } = 42;

        public int testInt5 { get; set; } = 42;

        public int testInt6 { get; set; } = 42;

        public int testInt7 { get; set; } = 42;

        public int TestInt8 { get; set; } = 42;
    }
}

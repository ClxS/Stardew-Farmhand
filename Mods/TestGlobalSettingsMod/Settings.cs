namespace TestGlobalSettings
{
    using Farmhand;

    internal class Settings : ModSettings
    {
        public string TestStringGlobal { get; set; } = "Global";

        public int TestInt { get; set; } = 42;
    }
}

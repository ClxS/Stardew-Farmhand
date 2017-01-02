using Farmhand.API.Tools;

namespace TestToolMod.Weapons
{
    public class TestWeapon : StardewValley.Tools.MeleeWeapon
    {
        private static WeaponInformation _information;
        public static WeaponInformation Information => _information ?? (_information = new Farmhand.API.Tools.WeaponInformation
        {
            Name = "Test Weapon",
            Texture = TestToolMod.Instance.ModSettings.GetTexture("sprite_TestWeapon"),
            Description = "Something feels out of place about this blade.",
            MinDamage = 2,
            MaxDamage = 5,
            Knockback = 1,
            Speed = 0,
            AddedPrecision = 0,
            AddedDefense = 0,
            WeaponType = WeaponType.Special,
            AddedAreaOfEffect = 0,
            CritChance = .02f,
            CritMultiplier = 3
        });

        // A default constructor that calls the base default constructor is required for proper ID fixing.
        // Using the default constructor to create an object intended to be used is not recommended
        protected TestWeapon() : base()
        {
            
        }

        public TestWeapon(WeaponInformation information)
            : base(information.Id)
        {

        }
    }
}

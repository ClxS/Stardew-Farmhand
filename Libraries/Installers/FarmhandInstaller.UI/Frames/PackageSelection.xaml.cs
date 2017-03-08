namespace Farmhand.Installers.Frames
{
    using System.Windows;

    using Farmhand.Installers.Utilities;

    /// <summary>
    /// Interaction logic for PackageSelection
    /// </summary>
    public partial class PackageSelection
    {
        internal static string CommandPlayerPackage => "Player";

        internal static string CommandDeveloperPackage => "Developer";

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageSelection"/> class.
        /// </summary>
        public PackageSelection()
        {
            this.InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Package Selection");
            TitleInfoService.SetPackageSelection(string.Empty);
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnBack();
        }

        private void ButtonPlayerPackage_OnClick(object sender, RoutedEventArgs e)
        {
            InstallationContext.PackageType = PackageType.Player;
            TitleInfoService.SetPackageSelection("Player");
            this.OnNavigate(CommandPlayerPackage);
        }

        private void ButtonDeveloperPackage_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnNavigate(CommandDeveloperPackage);
        }
    }
}

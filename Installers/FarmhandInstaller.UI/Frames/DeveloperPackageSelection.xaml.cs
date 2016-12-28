namespace FarmhandInstaller.UI.Frames
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Navigation;

    using FarmhandInstaller.UI.Utilities;

    /// <summary>
    /// Interaction logic for DeveloperPackageSelection
    /// </summary>
    public partial class DeveloperPackageSelection : BaseFrame
    {
        internal static string CommandDevPackage => "next";

        internal DeveloperPackageSelection()
        {
            this.InitializeComponent();
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnBack();
        }

        private void ButtonPackageHard_OnClick(object sender, RoutedEventArgs e)
        {
            InstallationContext.PackageType = PackageType.DeveloperFull;
            this.OnNavigate(CommandDevPackage);
        }

        private void ButtonPackageEasy_OnClick(object sender, RoutedEventArgs e)
        {
            InstallationContext.PackageType = PackageType.DeveloperLite;
            this.OnNavigate(CommandDevPackage);
        }
    }
}

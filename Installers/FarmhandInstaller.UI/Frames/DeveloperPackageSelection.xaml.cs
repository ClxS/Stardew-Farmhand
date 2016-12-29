namespace FarmhandInstaller.UI.Frames
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Navigation;

    using FarmhandInstaller.UI.Utilities;

    /// <summary>
    /// Interaction logic for DeveloperPackageSelection
    /// </summary>
    public partial class DeveloperPackageSelection
    {
        internal static string CommandNext => "next";

        internal static string CommandCreateEmptyMod => "createEmpty";
        
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
            this.checkBoxCreateEmptyMod.IsChecked = InstallationContext.AddNewModFromTemplate;
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Dev Package Selection");
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnBack();
        }

        private void ButtonPackageHard_OnClick(object sender, RoutedEventArgs e)
        {
            InstallationContext.PackageType = PackageType.DeveloperFull;
            InstallationContext.AddNewModFromTemplate = this.checkBoxCreateEmptyMod.IsChecked ?? true;
            TitleInfoService.SetPackageSelection("DevFull");
            this.OnNavigate(InstallationContext.AddNewModFromTemplate ? CommandCreateEmptyMod : CommandNext);
        }

        private void ButtonPackageEasy_OnClick(object sender, RoutedEventArgs e)
        {
            InstallationContext.PackageType = PackageType.DeveloperLite;
            InstallationContext.AddNewModFromTemplate = this.checkBoxCreateEmptyMod.IsChecked ?? true;
            TitleInfoService.SetPackageSelection("DevLite");
            this.OnNavigate(InstallationContext.AddNewModFromTemplate ? CommandCreateEmptyMod : CommandNext);
        }
    }
}

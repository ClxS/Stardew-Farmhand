namespace FarmhandInstaller.UI.Frames
{
    using System.Windows;

    using FarmhandInstaller.UI.Utilities;

    /// <summary>
    /// Interaction logic for Install
    /// </summary>
    public partial class Install
    {
        internal static string CommandFinished => "installed";

        internal static string CommandError => "error";

        /// <summary>
        /// Initializes a new instance of the <see cref="Install"/> class.
        /// </summary>
        public Install()
        {
            this.InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Installing");
            PackageManager.InstallPackage();
        }

        private void ButtonTestSuccess_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnNavigate(CommandFinished);
        }

        private void ButtonTestFail_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnNavigate(CommandError);
        }
    }
}

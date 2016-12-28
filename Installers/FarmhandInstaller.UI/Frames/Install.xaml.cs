namespace FarmhandInstaller.UI.Frames
{
    using System.Windows;
    using System.Windows.Controls;

    using FarmhandInstaller.UI.Utilities;

    /// <summary>
    /// Interaction logic for Install.xaml
    /// </summary>
    public partial class Install : BaseFrame
    {
        internal static string CommandFinished => "installed";

        internal static string CommandError => "error";

        public Install()
        {
            InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
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

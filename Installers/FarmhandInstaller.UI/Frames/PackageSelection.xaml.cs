namespace FarmhandInstaller.UI.Frames
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for PackageSelection.xaml
    /// </summary>
    public partial class PackageSelection : BaseFrame
    {
        internal static string CommandPlayerPackage => "Player";

        internal static string CommandDeveloperPackage => "Developer";

        public PackageSelection()
        {
            InitializeComponent();
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

        private void ButtonPlayerPackage_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnNavigate(CommandPlayerPackage);
        }

        private void ButtonDeveloperPackage_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnNavigate(CommandDeveloperPackage);
        }
    }
}

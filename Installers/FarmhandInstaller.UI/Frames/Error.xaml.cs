namespace FarmhandInstaller.UI.Frames
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Navigation;

    /// <summary>
    /// Interaction logic for Error.xaml
    /// </summary>
    public partial class Error : BaseFrame
    {
        public Error()
        {
            InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}

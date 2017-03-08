namespace Farmhand.Installers.Frames
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Navigation;

    using Farmhand.Installers.Utilities;

    /// <summary>
    /// Interaction logic for Finished
    /// </summary>
    public partial class Finished
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Finished"/> class.
        /// </summary>
        public Finished()
        {
            this.InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Complete!");
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

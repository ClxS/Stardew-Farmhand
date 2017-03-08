namespace Farmhand.Installers.Frames
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Navigation;

    using Farmhand.Installers.Utilities;

    /// <summary>
    /// Interaction logic for Error
    /// </summary>
    public partial class Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        public Error()
        {
            this.InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Error");

            if (InstallationContext.Exception != null)
            {
                this.ExceptionInfo.Text = InstallationContext.Exception.Message;
                this.ExceptionInfo.Text += "\n" + InstallationContext.Exception.StackTrace;

                if (InstallationContext.Exception.InnerException != null)
                {
                    this.ExceptionInfo.Text += "\n\nInner Exception:\n";
                    this.ExceptionInfo.Text += InstallationContext.Exception.InnerException.Message;
                    this.ExceptionInfo.Text += InstallationContext.Exception.InnerException.StackTrace;
                }
            }
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

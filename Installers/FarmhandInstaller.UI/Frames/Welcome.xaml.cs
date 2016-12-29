namespace FarmhandInstaller.UI.Frames
{
    using System;
    using System.Windows;

    using FarmhandInstaller.UI.Utilities;

    /// <summary>
    /// Interaction logic for Welcome frame
    /// </summary>
    public partial class Welcome
    {
        internal static string CommandNext => "next";

        /// <summary>
        /// Initializes a new instance of the <see cref="Welcome"/> class.
        /// </summary>
        public Welcome()
        {
            this.InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Welcome");
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnNavigate(CommandNext);
        }
    }
}

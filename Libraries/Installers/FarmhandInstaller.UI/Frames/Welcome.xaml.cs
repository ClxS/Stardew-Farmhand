namespace Farmhand.Installers.Frames
{
    using System.Windows;

    using Farmhand.Installers.Utilities;

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
            TitleInfoService.SetPackageSelection(string.Empty);
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnNavigate(CommandNext);
        }
    }
}

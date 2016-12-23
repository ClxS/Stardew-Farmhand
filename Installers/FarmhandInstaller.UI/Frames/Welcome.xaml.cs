namespace FarmhandInstaller.UI.Frames
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Welcome frame
    /// </summary>
    public partial class Welcome : BaseFrame
    {
        internal static string CommandNext => "next";

        public Welcome()
        {
            InitializeComponent();
        }

        internal override void ClearFrame()
        {
        }

        internal override void Start()
        {
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            base.OnNavigate(CommandNext);
        }
    }
}

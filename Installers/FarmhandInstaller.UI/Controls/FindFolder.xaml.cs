namespace FarmhandInstaller.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for FindFolder
    /// </summary>
    public partial class FindFolder
    {
        /// <summary>
        /// Gets a value indicating whether is valid.
        /// </summary>
        public bool IsValid { get; private set; } = false;

        /// <summary>
        /// Called when a folder is selected
        /// </summary>
        public event EventHandler<EventArgsFolderSelected> Selected = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="FindFolder"/> class.
        /// </summary>
        public FindFolder()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Set filter for file extension and default file extension 
            dlg.ValidateNames = false;
            dlg.CheckFileExists = false;
            dlg.CheckPathExists = false;
            dlg.FileName = "Folder Selection.";
            dlg.Filter = "None|(*.*)";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                var folder = System.IO.Path.GetDirectoryName(filename);
                this.textBoxFileName.Text = folder;
                this.Validate();
            }
        }

        private void TextBoxFileName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Validate();
        }

        private void Validate()
        {
            EventArgsFolderSelected args = new EventArgsFolderSelected(this.textBoxFileName.Text);
            this.Selected.Invoke(this, args);

            this.iconFail.Visibility = Visibility.Hidden;
            this.iconPass.Visibility = Visibility.Hidden;
            this.textBlockValidation.Text = string.Empty;

            if (!args.Valid)
            {
                this.textBlockValidation.Text = args.ValidationFailureReason;
                this.iconFail.Visibility = Visibility.Visible;
                this.iconPass.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.iconPass.Visibility = Visibility.Visible;
                this.iconFail.Visibility = Visibility.Collapsed;
            }
            this.IsValid = args.Valid;
        }
    }
}

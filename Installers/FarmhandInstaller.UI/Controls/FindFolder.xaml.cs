namespace Farmhand.Installers.Controls
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
        public bool IsValid { get; private set; }

        /// <summary>
        /// Called when a folder is selected
        /// </summary>
        internal event EventHandler<EventArgsFolderSelected> Selected = delegate { };

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.textBoxFileName.Text;
            }

            set
            {
                this.textBoxFileName.Text = value;
                this.Validate();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindFolder"/> class.
        /// </summary>
        public FindFolder()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = false,
                FileName = "Folder Selection.",
                Filter = "None|(*.*)"
            };

            // Set filter for file extension and default file extension 

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

        public void Validate()
        {
            if (this.Selected == null)
            {
                return;
            }

            EventArgsFolderSelected args = new EventArgsFolderSelected(this.textBoxFileName.Text);
            this.Selected.Invoke(this, args);

            this.iconFail.Visibility = Visibility.Collapsed;
            this.iconPass.Visibility = Visibility.Hidden;
            this.textBlockValidation.Text = string.Empty;

            if (!args.SuppressValidationError)
            {
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
            }

            this.IsValid = args.Valid;
        }
    }
}

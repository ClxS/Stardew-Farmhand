namespace Farmhand.Installers.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ValidatedTextBox
    /// </summary>
    public partial class ValidatedTextBox
    {
        /// <summary>
        /// Called when a value is entered
        /// </summary>
        internal event EventHandler<EventArgsValidationRequested> Changed = delegate { };

        internal string FormLabel
        {
            get
            {
                return this.LabelForm.Content as string;
            }

            set
            {
                this.LabelForm.Content = value;
            }
        }

        internal double TextBoxWidth
        {
            get
            {
                return this.TextBox.Width;
            }

            set
            {
                this.TextBox.Width = value;
            }
        }

        internal double TextBoxHeight
        {
            get
            {
                return this.TextBox.Height;
            }

            set
            {
                this.TextBox.Height = value;
            }
        }

        internal bool AcceptsReturn
        {
            get
            {
                return this.TextBox.AcceptsReturn;
            }

            set
            {
                this.TextBox.AcceptsReturn = value;
            }
        }

        internal TextWrapping TextWrapping
        {
            get
            {
                return this.TextBox.TextWrapping;
            }

            set
            {
                this.TextBox.TextWrapping = value;
            }
        }

        internal string Value
        {
            get
            {
                return this.TextBox.Text;
            }

            set
            {
                this.TextBox.Text = value ?? string.Empty;
                this.Validate();
            }
        }

        internal bool IsValid { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedTextBox"/> class.
        /// </summary>
        public ValidatedTextBox()
        {
            this.InitializeComponent();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Validate();
        }

        private void Validate()
        {
            if (this.Changed == null)
            {
                return;
            }

            var args = new EventArgsValidationRequested(this.Value);
            this.Changed.Invoke(this, args);
            
            if (args.Valid)
            {
                this.IconPass.Visibility = Visibility.Visible;
                this.IconFail.Visibility = Visibility.Collapsed;
                this.IsValid = true;
            }
            else
            {
                this.ValidationFailMessage.Text = args.ValidationFailureReason;
                this.IconFail.Visibility = Visibility.Visible;
                this.IconPass.Visibility = Visibility.Collapsed;
                this.IsValid = false;
            }
        }
    }
}

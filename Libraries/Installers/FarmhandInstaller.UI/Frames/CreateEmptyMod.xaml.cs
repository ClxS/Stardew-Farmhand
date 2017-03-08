namespace Farmhand.Installers.Frames
{
    using System.Windows;

    using Farmhand.Installers.Controls;
    using Farmhand.Installers.Utilities;

    /// <summary>
    /// Interaction logic for CreateEmptyMod
    /// </summary>
    public partial class CreateEmptyMod : BaseFrame
    {
        internal static string CommandNext => "next";

        internal CreateEmptyMod()
        {
            this.InitializeComponent();
        }

        internal override void ClearFrame()
        {
            this.NameTextBox.Value = InstallationContext.ModSettings.Name;
            this.AuthorTextBox.Value = InstallationContext.ModSettings.Author;
            this.DescriptionTextBox.Value = InstallationContext.ModSettings.Description;
            this.CheckBoxCreateEmptyMod.IsChecked = InstallationContext.ModSettings.UsesSettingsFile;
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Create Empty Mod");
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnBack();
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            InstallationContext.ModSettings.Name = this.NameTextBox.Value;
            InstallationContext.ModSettings.Author = this.AuthorTextBox.Value;
            InstallationContext.ModSettings.Description = this.DescriptionTextBox.Value;
            InstallationContext.ModSettings.UsesSettingsFile = this.CheckBoxCreateEmptyMod.IsChecked ?? false;
            this.OnNavigate(CommandNext);
        }

        private void NameTextBox_OnChanged(object sender, EventArgsValidationRequested e)
        {
            if (e.Valid && e.Value.Replace(" ", string.Empty).Length <= 0)
            {
                e.Valid = false;
                e.ValidationFailureReason = "Name must contain some characters";
            }

            if (e.Valid && e.Value.Length > 64)
            {
                e.Valid = false;
                e.ValidationFailureReason = "Name is too long!";
            }

            if (e.Valid && e.Value.Contains("\""))
            {
                e.Valid = false;
                e.ValidationFailureReason = "Name cannot contain \" characters";
            }

            this.ButtonNext.IsEnabled = this.AuthorTextBox.IsValid && this.DescriptionTextBox.IsValid && e.Valid;
        }

        private void AuthorTextBox_OnChanged(object sender, EventArgsValidationRequested e)
        {
            if (e.Valid && e.Value.Replace(" ", string.Empty).Length <= 0)
            {
                e.Valid = false;
                e.ValidationFailureReason = "Author must contain some characters";
            }

            if (e.Valid && e.Value.Length > 64)
            {
                e.Valid = false;
                e.ValidationFailureReason = "Author is too long!";
            }

            if (e.Valid && e.Value.Contains("\""))
            {
                e.Valid = false;
                e.ValidationFailureReason = "Author cannot contain \" characters";
            }

            this.ButtonNext.IsEnabled = this.NameTextBox.IsValid && this.DescriptionTextBox.IsValid && e.Valid;
        }

        private void DescriptionTextBox_OnChanged(object sender, EventArgsValidationRequested e)
        {
            if (e.Valid && e.Value.Contains("\""))
            {
                e.Valid = false;
                e.ValidationFailureReason = "Description cannot contain \" characters";
            }

            this.ButtonNext.IsEnabled = this.NameTextBox.IsValid && this.AuthorTextBox.IsValid && e.Valid;
        }
    }
}

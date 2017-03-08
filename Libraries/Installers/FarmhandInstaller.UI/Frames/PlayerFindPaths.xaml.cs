namespace Farmhand.Installers.Frames
{
    using System.IO;
    using System.Windows;

    using Farmhand.Installers.Controls;
    using Farmhand.Installers.Utilities;

    /// <summary>
    /// Interaction logic for PlayerFindPaths
    /// </summary>
    public partial class PlayerFindPaths
    {
        internal static string CommandInstall => "install";

        internal PlayerFindPaths()
        {
            this.InitializeComponent();
        }

        internal override void ClearFrame()
        {
            this.finderInstallLocation.Value = InstallationContext.OutputPath;
            this.finderStardewFolder.Value = InstallationContext.StardewPath;
        }

        internal override void Start()
        {
            TitleInfoService.SetCurrentPage("Set Location");
        }

        private void ButtonInstall_OnClick(object sender, RoutedEventArgs e)
        {
            InstallationContext.OutputPath = this.finderInstallLocation.Value;
            InstallationContext.StardewPath = this.finderStardewFolder.Value;
            this.OnNavigate(CommandInstall);
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnBack();
        }

        private void FinderStardewFolder_OnSelected(object sender, EventArgsFolderSelected e)
        {
            if (string.IsNullOrWhiteSpace(e.Folder))
            {
                e.Valid = false;
                e.SuppressValidationError = true;
            }

            if (e.Valid)
            {
                try
                {
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Path.GetFullPath(e.Folder);
                    if (!Path.IsPathRooted(e.Folder))
                    {
                        e.Valid = false;
                        e.ValidationFailureReason = "This is not a valid path";
                    }
                }
                catch (System.Exception)
                {
                    e.Valid = false;
                    e.ValidationFailureReason = "This is not a valid path";
                }
            }

            if (e.Valid && !Directory.Exists(e.Folder))
            {
                e.Valid = false;
                e.ValidationFailureReason = "Folder does not exist";
            }

            var stardew = Path.Combine(e.Folder, "Stardew Valley.exe");
            var xtile = Path.Combine(e.Folder, "xTile.dll");
            var content = Path.Combine(e.Folder, "Content");

            if (e.Valid && !File.Exists(stardew))
            {
                e.Valid = false;
                e.ValidationFailureReason = "Could not find StardewValley.exe in folder";
            }

            if (e.Valid && !File.Exists(xtile))
            {
                e.Valid = false;
                e.ValidationFailureReason = "Could not find StardewValley.exe in folder";
            }

            if (e.Valid && !Directory.Exists(content))
            {
                e.Valid = false;
                e.ValidationFailureReason = "Could not find the Content folder in folder";
            }

            if (e.Valid && this.finderInstallLocation.IsValid)
            {
                if (this.finderInstallLocation.Value.Contains(e.Folder))
                {
                    this.finderInstallLocation.Validate();
                }
            }

            this.buttonInstall.IsEnabled = this.finderInstallLocation.IsValid && e.Valid;
        }

        private void FinderInstallLocation_OnSelected(object sender, EventArgsFolderSelected e)
        {
            if (string.IsNullOrWhiteSpace(e.Folder))
            {
                e.Valid = false;
                e.SuppressValidationError = true;
            }

            if (e.Valid)
            {
                try
                {
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Path.GetFullPath(e.Folder);

                    if (!Path.IsPathRooted(e.Folder))
                    {
                        e.Valid = false;
                        e.ValidationFailureReason = "This is not a valid path";
                    }
                }
                catch (System.Exception)
                {
                    e.Valid = false;
                    e.ValidationFailureReason = "This is not a valid path";
                }
            }

            if (e.Valid && Directory.Exists(e.Folder))
            {
                if (Directory.GetFiles(e.Folder).Length > 0 || Directory.GetDirectories(e.Folder).Length > 0)
                {
                    e.Valid = false;
                    e.ValidationFailureReason = "Install folder must be empty";
                    return;
                }
            }

            if (e.Valid && !string.IsNullOrEmpty(this.finderStardewFolder.Value))
            {
                if (e.Folder.Contains(this.finderStardewFolder.Value))
                {
                    e.Valid = false;
                    e.ValidationFailureReason = "Farmhand cannot be placed in the Stardew Valley folder";
                }
            }

            this.buttonInstall.IsEnabled = e.Valid && this.finderStardewFolder.IsValid;
        }
    }
}

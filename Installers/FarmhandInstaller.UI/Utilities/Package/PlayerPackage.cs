namespace FarmhandInstaller.UI.Utilities.Package
{
    using System;
    using System.IO;

    internal class PlayerPackage : IPackage
    {
        public const string PackageFile = "FarmhandInstaller.UI.Payload.Player.package";

        #region IPackage Members

        public void Install()
        {
            var farmhandBinary = Path.Combine(InstallationContext.OutputPath, "Stardew Farmhand.exe");
            var temporaryDirectory = Path.Combine(InstallationContext.OutputPath, "FarmhandInstallTemp");
            var workingDirectory = Path.Combine(temporaryDirectory, "WorkingDirectory");
            var binDirectory = Path.Combine(temporaryDirectory, "Bin");

            DirectoryUtility.EnsureDirectoryExists(InstallationContext.OutputPath);
            DirectoryUtility.CleanDirectory(InstallationContext.OutputPath);
            DirectoryUtility.EnsureDirectoryExists(temporaryDirectory);
            PackageManager.ExtractPackageFile(PackageFile, temporaryDirectory);
            
            DirectoryUtility.CopyAll(InstallationContext.StardewPath, InstallationContext.OutputPath, ".*\\.exe");
            DirectoryUtility.CopyAll(workingDirectory, InstallationContext.OutputPath);

            StardewPatcher.Patch(farmhandBinary, binDirectory);

            Directory.Delete(temporaryDirectory, true);
        }

        #endregion
    }
}
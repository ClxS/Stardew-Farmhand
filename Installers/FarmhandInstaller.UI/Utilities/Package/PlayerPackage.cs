namespace Farmhand.Installers.Utilities.Package
{
    using System.IO;

    using Farmhand.Installers.Utilities;

    internal class PlayerPackage : IPackage
    {
        public const string PackageFile = "Farmhand.Installers.Payload.Player.package";

        #region IPackage Members

        public void Install(PackageStatusContext context)
        {
            var farmhandBinary = Path.Combine(InstallationContext.OutputPath, "Stardew Farmhand.exe");
            var temporaryDirectory = Path.Combine(InstallationContext.OutputPath, "FarmhandInstallTemp");
            var workingDirectory = Path.Combine(temporaryDirectory, "WorkingDirectory");
            var binDirectory = Path.Combine(temporaryDirectory, "Bin");

            context.SetState(10, "Cleaning Output Directory");
            DirectoryUtility.EnsureDirectoryExists(InstallationContext.OutputPath);
            DirectoryUtility.CleanDirectory(InstallationContext.OutputPath);
            DirectoryUtility.EnsureDirectoryExists(temporaryDirectory);

            context.SetState(25, "Extracting Package File");
            PackageManager.ExtractPackageFile(PackageFile, temporaryDirectory);
            
            context.SetState(40, "Copying Stardew Valley Files");
            DirectoryUtility.CopyAll(InstallationContext.StardewPath, InstallationContext.OutputPath, ".*\\.exe");

            context.SetState(50, "Copying SMAPI Files");
            DirectoryUtility.CopyAll(workingDirectory, InstallationContext.OutputPath);
            
            StardewPatcher.Patch(farmhandBinary, binDirectory, true, context);

            context.SetState(100, "Deleting Temporary Directory");
            Directory.Delete(temporaryDirectory, true);
        }

        #endregion
    }
}
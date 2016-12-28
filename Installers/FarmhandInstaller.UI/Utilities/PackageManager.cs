namespace FarmhandInstaller.UI.Utilities
{
    using System;

    using FarmhandInstaller.UI.Utilities.Packages;

    internal class PackageManager
    {
        public static void InstallPackage()
        {
            IPackage package = null;
            switch (InstallationContext.PackageType)
            {
                case PackageType.DeveloperFull:
                    package = new DeveloperFullPackage();
                    break;
                case PackageType.DeveloperLite:
                    package = new DeveloperLitePackage();
                    break;
                case PackageType.Player:
                    package = new PlayerPackage();
                    break;
            }

            if (package == null)
            {
                throw new Exception("Unknown Package Type");
            }

            package.Install();
        }
    }
}

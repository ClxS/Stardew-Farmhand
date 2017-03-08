namespace Farmhand.Installers.Utilities
{
    using System;
    using System.IO;

    using Farmhand.Installers.Utilities.Package;

    using ICSharpCode.SharpZipLib.Core;
    using ICSharpCode.SharpZipLib.Zip;

    internal class PackageManager
    {
        public static void InstallPackage(PackageStatusContext context)
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

            package.Install(context);
        }
        
        public static void ExtractPackageFile(string fileName, string destination)
        {
            var zip = Path.Combine(destination, "package.zip");
            EmbeddedResourceUtility.ExtractResource(fileName, zip);

            ZipFile zf = null;
            try
            {
                var fs = File.OpenRead(zip);
                zf = new ZipFile(fs);

                foreach (ZipEntry zipEntry in zf)
                {
                    var entryFileName = zipEntry.Name;
                    var buffer = new byte[4096];
                    var zipStream = zf.GetInputStream(zipEntry);

                    var fullZipToPath = Path.Combine(destination, entryFileName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }

                File.Delete(zip);
            }
        }
    }
}

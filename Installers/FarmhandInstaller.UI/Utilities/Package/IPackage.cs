namespace Farmhand.Installers.Utilities.Package
{
    using Farmhand.Installers.Utilities;

    internal interface IPackage
    {
        void Install(PackageStatusContext context);
    }
}
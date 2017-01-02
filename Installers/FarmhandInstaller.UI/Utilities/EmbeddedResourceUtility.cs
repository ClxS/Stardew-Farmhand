namespace Farmhand.Installers.Utilities
{
    using System;
    using System.Linq;

    internal static class EmbeddedResourceUtility
    {
        public static string[] GetEmbeddedResourceNames()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames().ToArray();
        }

        public static void ExtractResource(string resourceName, string destination)
        {
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new NullReferenceException("GetManifestResourceStream returned null");
                }

                using (var fileStream = new System.IO.FileStream(destination, System.IO.FileMode.Create))
                {
                    for (var i = 0; i < stream.Length; i++)
                    {
                        fileStream.WriteByte((byte)stream.ReadByte());
                    }

                    fileStream.Close();
                }
            }
        }
    }
}

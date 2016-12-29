using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FarmhandInstaller.UI.Utilities
{
    internal static class TitleInfoService
    {
        public static Button TitleInfoElement { get; set; }

        private static string Page { get; set; }

        private static string Package { get; set; }

        public static void SetCurrentPage(string page)
        {
            Page = page;
            SetElementText();
        }

        public static void SetPackageSelection(string package)
        {
            Package = package;
            SetElementText();
        }

        private static void SetElementText()
        {
            var title = Page;
            if (!string.IsNullOrEmpty(Package))
            {
                title += " - " + Package;
            }

            TitleInfoElement.Content = title;
        }
    }
}

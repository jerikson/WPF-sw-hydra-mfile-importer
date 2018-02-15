using RemObjects.Hydra;
using System;
using SystemWeaver.ExtensionsAPI;

namespace SWExtension.CalAndLocalsImporter
{
    [Plugin, NonVisualPlugin]
    public class ExtensionInfo : NonVisualPlugin, IswExtensionInfo
    {
        public string GetAbout()
        {
            return string.Empty;
        }

        public string GetExtensionsAPIVersion()
        {
            return ExtensionsAPI.Version;
        }
    }
}

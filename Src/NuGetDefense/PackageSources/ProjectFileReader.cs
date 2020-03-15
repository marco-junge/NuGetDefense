﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NuGetDefense.PackageSources
{
    public static class ProjectFileReader
    {
        public static bool TryReadFromFile(string path, out IEnumerable<NuGetPackage> nugetPackages)
        {
            var projectFile = new FileInfo(path);
            if (!(projectFile.Exists && projectFile.Extension.EndsWith("proj", StringComparison.InvariantCultureIgnoreCase)))
            {
                nugetPackages = Enumerable.Empty<NuGetPackage>();
                return false;
            }

            nugetPackages = PackagesReferencesFromProjectFile(projectFile)
                .Select(ToNugetPackage)
                .AsEnumerable();

            return true;
        }

        private static NuGetPackage ToNugetPackage(XElement xElement)
        {
            return new NuGetPackage
            {
                Id = xElement.Attribute("Include")?.Value,
                Version = xElement.Attribute("Version")?.Value,
                LineNumber = ((IXmlLineInfo) xElement).LineNumber,
                LinePosition = ((IXmlLineInfo) xElement).LinePosition
            };
        }

        private static IEnumerable<XElement> PackagesReferencesFromProjectFile(FileSystemInfo packagesConfigPath) =>
            XElement.Load(packagesConfigPath.FullName, LoadOptions.SetLineInfo).DescendantsAndSelf("PackageReference");
    }
}
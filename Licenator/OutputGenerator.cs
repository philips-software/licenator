using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Licenator
{
    public class OutputGenerator
    {
        private const string IndentWhitespaces = "    ";

        private List<string> Lines;

        public bool OmitUsedIn { get; set; }

        public void Generate(PackageList packages, string rootPath, string outputFile)
        {
            Lines = new List<string>();
            WriteHeader(rootPath);
            
            var summary = packages.GetSummary();
            foreach(var p in summary)
            {
                WriteSummary(p);
            }

            WriteFailures(packages);

            File.WriteAllLines(outputFile, Lines);
            Lines = null;
        }

        private void WriteHeader(string rootPath)
        {
            Lines.Add(Program.LicenatorHeaderLine);
            Lines.Add("Directory: " + rootPath);
            Lines.Add("Generated on: " + DateTime.Now.ToString("s"));
            Lines.Add("");
        }

        private void WriteSummary(LicenseSummary p)
        {
            Lines.Add("License: " + p.LicenseUrl);
            Lines.Add("Packages with this license:");
            foreach (var ps in p.Packages)
            {
                Lines.Add(IndentWhitespaces + ps.Name + " (" + ps.Version + ")");
            }
            WriteUsedIn(p.UsedIn);
            Lines.Add("");
        }

        private void WriteFailures(PackageList packages)
        {
            var failedPackages = packages.GetFailedPackages();
            if (failedPackages.Any())
            {
                Lines.Add("[!] Fetching package metadata failed for the following packages. [!]");
                foreach (var p in failedPackages)
                {
                    Lines.Add("[!]" + IndentWhitespaces + p.Name +  " (" + p.Version + ")");
                    WriteUsedIn(p.UsedIn);
                    Lines.Add("");
                }
            }
        }

        private void WriteUsedIn(List<string> usedIn)
        {
            if (OmitUsedIn) return;
            
            Lines.Add("Used in:");
            foreach(var u in usedIn)
            {
                Lines.Add(IndentWhitespaces + u);
            }
        }
    }
}
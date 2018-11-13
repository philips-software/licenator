using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Licenator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Licenator \"<Root Directory>\" \"<Output filename>\"");
                return;
            }

            var rootPath = args[0];
            var outputFile = args[1];

            Console.WriteLine("Licenator (c) Philips 2018 by Ben Bierens");
            Console.WriteLine("Directory: " + rootPath);
            Console.WriteLine("Output: " + outputFile);

            var traveler = new DirectoryTraveler();
            var parser = new NuGetParser();
            var packages = new PackageList();
            var licenseResolver = new LicenseResolver();

            Console.WriteLine("Reading packages...");
            traveler.Begin(rootPath, file => HandleFile(file, parser, packages));

            Console.WriteLine("Fetching package information...");
            licenseResolver.ResolveAll(packages);

            var summary = packages.GetSummary();

            foreach(var p in summary)
            {
                Console.WriteLine("License: " + p.LicenseUrl);
                Console.WriteLine("Packages with this license: " + string.Join(Environment.NewLine, p.Packages.Select(s => s.Name + " (" + s.Version + ")")));
                Console.WriteLine("Used in: " + string.Join(',', p.UsedIn));
                Console.WriteLine(" ");
            }
            Console.WriteLine("output file: " + outputFile);
        }

        private static void HandleFile(string file, NuGetParser parser, PackageList packages)
        {
            if (parser.SupportsFile(file))
            {
                var result = parser.ProcessFile(file);
                if (result.Any())
                {
                    packages.Add(result);
                }
            }
        }
    }
}

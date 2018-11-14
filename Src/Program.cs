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
        private static string RootPath;
        private static string OutputFile;
        private static List<string> PackagesToIgnore;

        public static void Main(string[] args)
        {
            Console.WriteLine("Licenator (c) Philips 2018 by Ben Bierens");

            HandleAndEchoArguments(args);

            var traveler = new DirectoryTraveler();
            var parser = new NuGetParser();
            var packages = new PackageList();
            var licenseResolver = new LicenseResolver();

            Console.WriteLine("Reading packages...");
            traveler.Begin(RootPath, file => HandleFile(file, parser, packages));

            packages.IgnorePackages(PackagesToIgnore);

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
            Console.WriteLine("output file: " + OutputFile);
        }

        private static void HandleAndEchoArguments(string[] args)
        {
            if (args.Length < 2)
            {
                PrintUsageAndExit();
                return;
            }

            RootPath = args[0];
            OutputFile = args[1];
            if (args.Length > 2 && args[2] == "-i")
            {
                PackagesToIgnore = args.Skip(3).ToList();
            }
            
            Console.WriteLine("Directory: " + RootPath);
            Console.WriteLine("Output: " + OutputFile);
            if (PackagesToIgnore.Any())
            {
                Console.WriteLine("Ignoring packages: " + string.Join(',', PackagesToIgnore));
            }
        }

        private static void PrintUsageAndExit()
        {
            Console.WriteLine("Usage: Licenator \"<Root Directory>\" \"<Output filename>\"");
            Console.WriteLine("You can ignore any number of specific packages with -i:");
            Console.WriteLine("Usage: Licenator \"<Root Directory>\" \"<Output filename>\" -i \"Package.To.Ignore.One\" \"Package.To.Ignore.Two\"");
            Environment.Exit(0);
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

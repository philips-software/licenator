using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Licenator
{
    public class Program
    {
        private const string LicenatorHeaderLine = "Licenator (c) Philips 2018 by Ben Bierens";
        private const string IndentWhitespaces = "    ";
        private static string RootPath;
        private static string OutputFile;
        private static List<string> PackagesToIgnore;
        private static NuGetParser Parser = new NuGetParser();

        public static void Main(string[] args)
        {
            Console.WriteLine(LicenatorHeaderLine);

            HandleAndEchoArguments(args);

            var packages = new PackageList();

            ProcessPackages(packages);

            GenerateOutputFile(packages);

            Console.WriteLine("Done!");
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

        private static void ProcessPackages(PackageList packages)
        {
            var traveler = new DirectoryTraveler();
            Console.WriteLine("Reading packages...");
            traveler.Begin(RootPath, file => HandleFile(file, packages));

            packages.IgnorePackages(PackagesToIgnore);

            var licenseResolver = new LicenseResolver();
            Console.WriteLine("Fetching package information...");
            licenseResolver.ResolveAll(packages);
        }

        private static void HandleFile(string file, PackageList packages)
        {
            if (Parser.SupportsFile(file))
            {
                var result = Parser.ProcessFile(file);
                if (result.Any())
                {
                    packages.Add(result);
                }
            }
        }

        private static void GenerateOutputFile(PackageList packages)
        {
            var lines = new List<string>();
            lines.Add(LicenatorHeaderLine);
            lines.Add("Directory: " + RootPath);
            lines.Add("Generated on: " + DateTime.Now.ToString("s"));
            lines.Add("");
            
            var summary = packages.GetSummary();
            foreach(var p in summary)
            {
                lines.Add("License: " + p.LicenseUrl);
                lines.Add("Packages with this license:");
                foreach (var ps in p.Packages)
                {
                    lines.Add(IndentWhitespaces + ps.Name + " (" + ps.Version + ")");
                }
                lines.Add("Used in:");
                foreach(var u in p.UsedIn)
                {
                    lines.Add(IndentWhitespaces + u);
                }
                lines.Add("");
            }

            File.WriteAllLines(OutputFile, lines);
        }
    }
}

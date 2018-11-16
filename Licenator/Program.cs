using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Licenator
{
    public class Program
    {
        public const string LicenatorHeaderLine = "Licenator v0.0.1 (c) Philips 2018 by Ben Bierens";
        private static string RootPath;
        private static string OutputFile;
        private static List<string> PackagesToIgnore = new List<string>();
        private static NuGetParser Parser = new NuGetParser();
        private static OutputGenerator Generator = new OutputGenerator();

        public static void Main(string[] args)
        {
            Console.WriteLine(LicenatorHeaderLine);

            HandleAndEchoArguments(args);

            var packages = new PackageList();

            FindAllPackages(packages);

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
            Generator.OmitUsedIn = args.ToList().Any(a => a == "-o");
            if (args.Length > 2 && args.Any(a => a == "-i"))
            {
                var index = args.ToList().IndexOf("-i");
                PackagesToIgnore = args.Skip(index + 1).ToList();
            }

            Console.WriteLine("Directory: " + RootPath);
            Console.WriteLine("Output: " + OutputFile);
            if (PackagesToIgnore.Any())
            {
                Console.WriteLine("Ignoring packages: " + string.Join(',', PackagesToIgnore));
            }

            if (PackagesToIgnore.Any(p => p.Contains("-")) || 
                string.IsNullOrEmpty(RootPath) ||
                string.IsNullOrEmpty(OutputFile))
            {
                throw new Exception("Invalid arguments");
            }

            if (!Directory.Exists(RootPath))
            {
                throw new Exception("Provided directory doesn't exist.");
            }
        }

        private static void PrintUsageAndExit()
        {
            Console.WriteLine("Usage: Licenator \"<Root Directory>\" \"<Output filename>\"");
            Console.WriteLine("Options:");
            Console.WriteLine("    -o : Omits the \"Used In\" information from the output.");
            Console.WriteLine("    -i : Specify a list of packages to be ignored.");
            Console.WriteLine("Usage: Licenator \"<Root Directory>\" \"<Output filename>\" -o -i \"Package.To.Ignore.One\" \"Package.To.Ignore.Two\"");
            Environment.Exit(0);
        }

        private static void FindAllPackages(PackageList packages)
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
            Generator.Generate(packages, RootPath, OutputFile);
        }
    }
}

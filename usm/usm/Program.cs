using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using usmcore;


namespace usm
{
    class Program
    {
        static bool again;
        static void Main()
        {
            Console.WriteLine("What you want to do?");
            Console.WriteLine("-----");
            Console.WriteLine("1. Create Skeleton from existing package");
            Console.WriteLine("2. Create Package from existing folder");

            ConsoleKeyInfo selected = Console.ReadKey();

            switch (selected.Key)
            {
                case ConsoleKey.D1:
                    CreateSkeletonOption();
                    break;

                case ConsoleKey.D2:
                    CreatePackageOption();
                    break;
            }
        }

        static void CreateSkeletonOption()
        {
            Console.Clear();
            Console.WriteLine("Select path to create skeleton [dest]:");
            var projectbasedir = Console.ReadLine();

            if (!Directory.Exists(Path.Combine(projectbasedir, ".git")))
            {
                Console.WriteLine("Selected path must be GIT project directory! [Clone empty project]");
                Console.ReadKey();
                return;
            }

            var projectusmpath = GetProjectUsmPathFromBase(projectbasedir);

            if (File.Exists(projectusmpath))
            {
                var projusm = ReadProjectUsm(projectusmpath);
                var skeletons = UsmPackageHelper.GetAllSkeletonsAvailiable();
                int i = 0;
                Console.WriteLine("Availiable Packages:");
                Console.WriteLine("-------------");

                foreach (var skeleton in skeletons)
                {
                    i++;
                    Console.WriteLine($"{i}." + skeleton.Name.Replace(".zip", ""));
                }

                Console.WriteLine("Select package number:");
                var num = Console.ReadLine();

                int numero = int.Parse(num);
                var k = skeletons[numero - 1];

                ExtractPackage(k.Name.Replace(".zip", ""), projectbasedir, projusm);


            }
            else
            {
                Console.WriteLine($"Couldnt locate {projectusmpath} do you want to create {UsmJsonHelper.FileName} in this location? [Y/N]");
                ConsoleKeyInfo answer = Console.ReadKey();
                switch (answer.Key)
                {
                    case ConsoleKey.Y:
                        var defaultusmjsonpath = UsmJsonHelper.GetDefaultUsmpath();
                        File.Copy(defaultusmjsonpath, projectusmpath);
                        again = true;
                        Console.ReadKey();
                        break;

                    default:
                        Environment.Exit(0);
                        break;
                }
            }

            if (again)
            {
                Console.Clear();
                Console.WriteLine($"Now you can modify settings of your project in {projectusmpath}");
                Console.ReadKey();

                if (File.Exists(projectusmpath))
                {
                    var projusm = ReadProjectUsm(projectusmpath);
                    ExtractPackage("unityemptygame", projectbasedir, projusm);

                    Console.WriteLine();
                    Console.WriteLine("Thanks for using Unity Skeleton Maker!");
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            Console.ReadKey();
        }

        static void CreatePackageOption()
        {
            Console.Clear();
            Console.WriteLine("Select directory you want to convert to skeleton:");
            var path = Console.ReadLine();
            Console.WriteLine("Name of your skeleton package ?");
            var name = Console.ReadLine();
            UsmPackageHelper.ConvertToSkeleton(path, name);


            Console.WriteLine("Thanks for using Unity Skeleton Maker!");
        }

        static string GetProjectUsmPathFromBase(string projectbasedir)
        {
            return Path.Combine(projectbasedir, UsmJsonHelper.FileName);
        }

        static UsmJsonModel ReadProjectUsm(string projectusmpath)
        {
            var usmjsonraw = File.ReadAllText(projectusmpath);
            var projectUsm = JsonConvert.DeserializeObject<UsmJsonModel>(usmjsonraw);
            Console.WriteLine($"usm.json v{projectUsm.UsmJsonVersion} found!");


            return projectUsm;

        }

        static void ExtractPackage(string packagename, string projectbasedir, UsmJsonModel projectUsm)
        {
            //DO YOU WANT TO CREATE PROJ ,, AND SELECT PACKAGE
            Console.WriteLine("Do you want to create new project base on this settings?");
            Console.WriteLine("---------------");
            Console.WriteLine($"Game name: {projectUsm.ProjectSettings.GameName}");
            Console.WriteLine($"Game name: {projectUsm.ProjectSettings.Description}");
            Console.WriteLine("---------------");
            Console.WriteLine("[Y/N]?");

            packagename = packagename + ".zip";
            ConsoleKeyInfo answer = Console.ReadKey();
            switch (answer.Key)
            {
                case ConsoleKey.Y:
                    var appPath = AppDomain.CurrentDomain.BaseDirectory;
                    var mb = Path.Combine(appPath, "skeletons");
                    var cb = Path.Combine(mb, packagename);
                    ZipFile.ExtractToDirectory(cb, projectbasedir);
                    ReadmeCreator.CreateReadme(projectbasedir, projectUsm.ProjectSettings);
                    break;

                default:
                    Environment.Exit(0);
                    break;
            }


        }
    }
}

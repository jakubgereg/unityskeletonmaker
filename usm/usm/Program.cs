using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;


namespace usm
{
    class Program
    {
        static bool again;
        static void Main(string[] args)
        {
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
                ExtractPackage("skeleton-unityemptygame", projectbasedir, projusm);

                Console.WriteLine("Thanks for using Unity Skeleton Maker!");
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
                    ExtractPackage("skeleton-unityemptygame", projectbasedir, projusm);

                    Console.WriteLine();
                    Console.WriteLine("Thanks for using Unity Skeleton Maker!");
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            //ReadJSON

            Console.ReadKey();
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

            ConsoleKeyInfo answer = Console.ReadKey();
            switch (answer.Key)
            {
                case ConsoleKey.Y:
                    var appPath = AppDomain.CurrentDomain.BaseDirectory;
                    var cb = Path.Combine(appPath, packagename);
                    var rb = Path.Combine(cb, "default.zip");
                    ZipFile.ExtractToDirectory(rb, projectbasedir);
                    ReadmeCreator.CreateReadme(projectbasedir, projectUsm.ProjectSettings);
                    break;

                default:
                    Environment.Exit(0);
                    break;
            }


        }
    }
}

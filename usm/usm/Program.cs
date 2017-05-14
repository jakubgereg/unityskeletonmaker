using System;
using usmcore;


namespace usm
{
    class Program
    {
        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            CreateHeader();

            ColoredConsoleWrite(ConsoleColor.White, "What do you want to do?\n");
            ColoredConsoleWrite(ConsoleColor.Black, "-----------------------\n");
            Console.Write("1. Create ");
            ColoredConsoleWrite(ConsoleColor.Yellow, "Skeleton");
            Console.Write(" from existing package\n");
            Console.Write("2. Create ");
            ColoredConsoleWrite(ConsoleColor.Green, "Package");
            Console.Write(" from existing folder\n");

            ConsoleKeyInfo selected = Console.ReadKey();

            switch (selected.Key)
            {
                case ConsoleKey.D1:
                    CreateSkeletonOption();
                    break;

                case ConsoleKey.D2:
                    CreatePackageOption();
                    break;
                case ConsoleKey.X:
                    var ps = UsmJsonHelper.ReadProjectUsm(AppDomain.CurrentDomain.BaseDirectory);
                    ReadmeCreator.CreateReadme(AppDomain.CurrentDomain.BaseDirectory, ps.ProjectSettings, CallBackError);
                    break;
            }

            ColoredConsoleWrite(ConsoleColor.Green, "\nThanks for using Unity Skeleton Maker!\t(press [R] to restart)");
            ConsoleKeyInfo exit = Console.ReadKey();
            switch (exit.Key)
            {
                case ConsoleKey.R:
                    Main();
                    break;
            }
        }

        static void CreateSkeletonOption()
        {
            CreateHeader();

            Console.WriteLine("Select path to create skeleton [dest]:");
            var projectbasedir = Console.ReadLine();

            //check if project is git
            if (!UsmJsonHelper.IsProjectGitRepo(projectbasedir))
            {
                ColoredConsoleWrite(ConsoleColor.White,
                    "Selected path must be GIT project directory! [Clone empty project]", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            var projectusmpath = UsmJsonHelper.GetProjectUsmpath(projectbasedir);

            //If usm.json exist try to create it
            if (!UsmJsonHelper.IsProjectUsmExists(projectbasedir))
            {
                if (CreateNewUsmJsonOption(projectusmpath, projectbasedir))
                {
                    CallBackMessage("\n\nCreated usm.json file in project directory!\n");
                    ColoredConsoleWrite(ConsoleColor.Black, $"Now you can modify settings of your project in {projectusmpath}\n", ConsoleColor.Green);

                    Console.WriteLine("\nPress [enter] to continue!");
                    Console.ReadKey();
                }
            }

            //check if usm.json exist and extract skeleton
            if (UsmJsonHelper.IsProjectUsmExists(projectbasedir))
            {
                CreateHeader();

                var skeletons = UsmPackageHelper.GetAllSkeletonsAvailiable();
                int i = 0;
                Console.WriteLine("Availiable skeleton packages:");
                ColoredConsoleWrite(ConsoleColor.Black, "-----------------------------\n");

                foreach (var skeleton in skeletons)
                {
                    i++;
                    ConsoleColor bc = i % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.Cyan;
                    var str = MakeOneLine($"{i}." + skeleton.Name.Replace(".zip", ""));
                    ColoredConsoleWrite(ConsoleColor.Black, str + "\n", bc);
                }

                Console.Write("\nSelect package [number]:");
                var num = Console.ReadLine();

                if (num != null)
                {
                    int numero = int.Parse(num);
                    if (numero == 0) return;

                    if (numero <= skeletons.Count)
                    {
                        UsmJsonModel projusm = UsmJsonHelper.ReadProjectUsm(projectbasedir);
                        var k = skeletons[numero - 1];
                        ExtractOption(k.Name.Replace(".zip", ""), projectbasedir, projusm);
                    }
                }
            }
        }

        static bool CreateNewUsmJsonOption(string projectusmpath, string projectbasedir)
        {
            ColoredConsoleWrite(ConsoleColor.White, $"Couldn`t locate {projectusmpath}\n", ConsoleColor.Red);
            Console.Write($"Do you want to create ");
            ColoredConsoleWrite(ConsoleColor.Cyan, "(default) ");
            Console.Write($"{UsmJsonHelper.FileName} in this location? [Y/N]:");
            ConsoleKeyInfo answer = Console.ReadKey();
            switch (answer.Key)
            {
                case ConsoleKey.Y:
                    UsmJsonHelper.InitializeNewUsmJson(projectbasedir);
                    return true;
            }
            return false;
        }

        static void CreatePackageOption()
        {
            CreateHeader();

            Console.Write("Select directory you want to convert to ");
            ColoredConsoleWrite(ConsoleColor.Yellow, "skeleton");
            Console.Write(":\n");
            var path = Console.ReadLine();
            Console.Write("Name of your skeleton ");
            ColoredConsoleWrite(ConsoleColor.Green, "Package");
            Console.Write(":");
            var name = Console.ReadLine();
            Console.WriteLine();
            UsmPackageHelper.ConvertToSkeleton(path, name, CallBackMessage);

        }

        static void ExtractOption(string packagename, string projectbasedir, UsmJsonModel projectUsm)
        {
            CreateHeader();
            //DO YOU WANT TO CREATE PROJ ,, AND SELECT PACKAGE
            Console.Write($"Do you want to extract package ");
            ColoredConsoleWrite(ConsoleColor.Green, $"\"{packagename}\"");
            Console.WriteLine(" to project base on this settings?");
            ColoredConsoleWrite(ConsoleColor.Black, "----------------------------------------------------------------------\n");
            ColoredConsoleWrite(ConsoleColor.Yellow, "Game name: ");
            Console.WriteLine($"{projectUsm.ProjectSettings.GameName}");
            ColoredConsoleWrite(ConsoleColor.Yellow, "Description: ");
            Console.WriteLine($"{projectUsm.ProjectSettings.Description}");
            ColoredConsoleWrite(ConsoleColor.Yellow, "Url: ");
            Console.WriteLine($"{projectUsm.ProjectSettings.Url}");
            ColoredConsoleWrite(ConsoleColor.Black, "----------------------------------------------------------------------\n");
            Console.Write("[Y/N]:");


            ConsoleKeyInfo answer = Console.ReadKey();
            switch (answer.Key)
            {
                case ConsoleKey.Y:
                    UsmJsonHelper.ExtractPackage(packagename, projectbasedir, projectUsm, CallBackError);
                    break;
            }
        }

        static void CallBackError(string str)
        {
            ColoredConsoleWrite(ConsoleColor.White, $"\n[!] - {str}\n", ConsoleColor.Red);
        }

        static void CallBackMessage(string str)
        {
            ColoredConsoleWrite(ConsoleColor.Yellow, $"[$] - {str}\n");
        }

        public static string MakeOneLine(string text, bool centered = false)
        {
            string result = text;
            int maxlenght = 80;

            var textl = text.Length;

            var emptyspaces = maxlenght - textl;

            for (int i = 1; i < emptyspaces; i++)
            {
                result += " ";
            }

            return result;
        }

        public static void CreateHeader()
        {
            Console.Clear();
            ColoredConsoleWrite(ConsoleColor.DarkRed,
            "                      Unity Skeleton Maker by Jakub Gereg                       \n",
            ConsoleColor.Yellow);
        }

        public static void ColoredConsoleWrite(ConsoleColor color, string text, ConsoleColor background = ConsoleColor.DarkCyan)
        {
            ConsoleColor originalColor = ConsoleColor.White;
            ConsoleColor backgroundoriginalColor = ConsoleColor.DarkCyan;

            Console.BackgroundColor = background;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
            Console.BackgroundColor = backgroundoriginalColor;
        }
    }
}

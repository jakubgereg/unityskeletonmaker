using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace usmcore
{
    public static class UsmPackageHelper
    {

        /// <summary>
        /// Convert to package, addes .gitkeep files in empty directories
        /// </summary>
        public static void ConvertToSkeleton(string dir, string mycustompackage, Action<string> callback = null)
        {
            callback?.Invoke($"Converting folder to skeleton {mycustompackage}");

            Console.WriteLine();
            String[] alldirectories = Directory.GetDirectories(dir, "*.*", SearchOption.AllDirectories);
            foreach (var dire in alldirectories)
            {
                DirectoryInfo info = new DirectoryInfo(dire);
                /* Do something with the Folder or just add them to a list via nameoflist.add(); */

                //ignore all .git folders
                if (info.FullName.Contains(".git")) continue;
                var numberOfFilesInside = NumberOfFilesInDirectory(info.FullName);
                //Console.WriteLine($"{info.Name}({numberOfFilesInside})");

                if (numberOfFilesInside != 0) continue;
                AddGitKeepFile(info.FullName);


                callback?.Invoke($".gitkeep created for directory {info.Name}.");
            }

            callback?.Invoke("All files prepared!");

            CompressAllFiles(dir, mycustompackage);

            callback?.Invoke($"Package {mycustompackage} created!");
        }

        private static int NumberOfFilesInDirectory(string dir)
        {
            String[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

            return files.Length;
        }

        private static void AddGitKeepFile(string dirpath)
        {
            File.Create(Path.Combine(dirpath, ".gitkeep"), 1);
        }

        private static void CompressAllFiles(string path, string mypackagename)
        {
            var packageFileName = mypackagename + ".zip";
            var skeletonsDirectory = UsmJsonHelper.GetSkeletonsFolderPath();
            var newPath = Path.Combine(skeletonsDirectory, packageFileName);

            ZipFile.CreateFromDirectory(path, newPath);
        }

        public static List<FileInfo> GetAllSkeletonsAvailiable()
        {
            var skeletonPath = UsmJsonHelper.GetSkeletonsFolderPath();

            String[] skeletons = Directory.GetFiles(skeletonPath, "*", SearchOption.TopDirectoryOnly);
            var files = new List<FileInfo>();

            foreach (var skeleton in skeletons)
            {
                FileInfo info = new FileInfo(skeleton);
                files.Add(info);
            }

            return files;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace usmcore
{

    /// <summary>
    /// This helper class is used to create new version of default-usm-json
    /// It does not create it automatically yet, but it create file called ResultFromHelper.txt
    /// </summary>
    public static class UsmJsonHelper
    {
        /*Default usm.json path*/
        public const string FileName = "usm.json";
        private const string DefaultJsonFolder = "default-usm-json";
        private const string DefaultSkeletonsFolderName = "skeletons";

        /*New usm.json*/
        private static string _usmJsonVersion = "0.1";

        public static void CreateNewEmptyUsm()
        {
            var author = new ProjectSettings.Author
            {
                Name = "Jakub Gereg",
                NickName = "wramp",
                WebSiteUrl = "https://github.com/wramp"
            };

            var projectSettings = new ProjectSettings
            {
                GameName = "Skeleton Game",
                Description = $"This is description created in Unity Skeleton Maker v{_usmJsonVersion}",
                Url = "https://github.com/wramp",
                ImageUrl = "https://media.giphy.com/media/MX9r4jOTStUeA/giphy.gif",
                Authors = new List<ProjectSettings.Author> { author }
            };


            var model = new UsmJsonModel
            {
                UsmJsonVersion = _usmJsonVersion,
                ProjectSettings = projectSettings
            };
            /*Add other settings if model changes*/



            var result = JsonConvert.SerializeObject(model);
            File.WriteAllText("ResultFromHelper.txt", result);
        }

        public static string GetDefaultUsmpath()
        {
            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            var defaultUsmFolder = Path.Combine(appPath, DefaultJsonFolder);
            var usmFilePath = Path.Combine(defaultUsmFolder, FileName);

            return usmFilePath;
        }

        public static string GetSkeletonsFolderPath()
        {
            var appPath = AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(appPath, DefaultSkeletonsFolderName);
        }




        /* Method used in project */

        public static bool IsProjectGitRepo(string projectPath)
        {
            return Directory.Exists(Path.Combine(projectPath, ".git"));
        }

        public static bool IsProjectUsmExists(string projectPath)
        {
            return File.Exists(GetProjectUsmpath(projectPath));
        }

        public static void InitializeNewUsmJson(string projectPath)
        {
            var defaultusmjsonpath = GetDefaultUsmpath();
            var projectusmpath = GetProjectUsmpath(projectPath);
            File.Copy(defaultusmjsonpath, projectusmpath);
        }

        public static string GetProjectUsmpath(string projectPath)
        {
            return Path.Combine(projectPath, FileName);
        }

        public static UsmJsonModel ReadProjectUsm(string projectPath)
        {
            var projectusmpath = GetProjectUsmpath(projectPath);
            var usmjsonraw = File.ReadAllText(projectusmpath);
            var projectUsm = JsonConvert.DeserializeObject<UsmJsonModel>(usmjsonraw);
            return projectUsm;
        }

        public static void ExtractPackage(string packagename, string projectbasedir, UsmJsonModel projectUsm, Action<string> error = null)
        {
            packagename += ".zip";
            var skeletonFolderPath = GetSkeletonsFolderPath();
            var cb = Path.Combine(skeletonFolderPath, packagename);
            try
            {
                ZipFile.ExtractToDirectory(cb, projectbasedir);
            }
            catch (Exception ex)
            {
                error?.Invoke(ex.Message);
            }
            ReadmeCreator.CreateReadme(projectbasedir, projectUsm.ProjectSettings);
        }
    }
}

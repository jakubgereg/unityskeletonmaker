using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace usm
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
    }
}

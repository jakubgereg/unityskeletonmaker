using Newtonsoft.Json;
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
        private static string UsmJsonVersion = "0.1";
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
                Description = $"This is description created in Unity Skeleton Maker v{UsmJsonVersion}",
                Url = "https://github.com/wramp",
                ImageUrl = "https://media.giphy.com/media/MX9r4jOTStUeA/giphy.gif",
                Authors = new List<ProjectSettings.Author> { author }
            };


            var model = new UsmJsonModel
            {
                UsmJsonVersion = UsmJsonVersion,
                ProjectSettings = projectSettings
            };
            /*Add other settings if model changes*/



            var result = JsonConvert.SerializeObject(model);
            File.WriteAllText("ResultFromHelper.txt", result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace usmcore
{
    public static class ReadmeCreator
    {
        private const string ReadmeFileName = "README.md";

        public static void CreateReadme(string filepath, ProjectSettings settings, Action<string> callback = null)
        {
            var lines = new List<string>();

            lines.Add($"# **{settings.GameName}**");
            if (settings.Description != null) lines.Add($"{settings.Description}   ");
            if (settings.Url != null) lines.Add($"[{settings.Url}]({settings.Url})   ");
            if (settings.ImageUrl != null) lines.Add($"![{settings.GameName}]({settings.ImageUrl})   ");

            lines.Add(string.Empty);

            if (settings.Authors != null)
            {
                lines.AddRange(CreateAuthorSection(settings.Authors));
            }

            try
            {
                WriteToFile(filepath, lines.ToArray());
                callback?.Invoke("Readme file created!");
            }
            catch (Exception ex)
            {
                callback?.Invoke(ex.Message);
            }


        }

        private static List<string> CreateAuthorSection(List<ProjectSettings.Author> authors)
        {
            var result = new List<string>();
            result.Add("Authors");
            result.Add("--- ");
            foreach (var author in authors)
            {
                result.Add($"{author.Name} ([@{author.NickName}]({author.WebSiteUrl}))   ");
            }

            return result;
        }

        private static void WriteToFile(string dirlocation, string[] lines)
        {
            using (var file = new StreamWriter(Path.Combine(dirlocation, ReadmeFileName)))
            {
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    if (!line.Contains("Second"))
                    {
                        file.WriteLine(line);
                    }
                }
            }
        }
    }


}

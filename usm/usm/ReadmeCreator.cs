using System.Collections.Generic;
using System.IO;

namespace usm
{
    public static class ReadmeCreator
    {
        private const string ReadmeFileName = "README.md";

        public static void CreateReadme(string gameName, string description, string url, string image, List<Author> authors)
        {
            var lines = new List<string>();

            lines.Add($"# **{gameName}**");
            if (description != null) lines.Add($"{description}   ");
            if (url != null) lines.Add($"[{url}]({url})   ");
            if (image != null) lines.Add($"![{gameName}]({image})   ");

            lines.Add(string.Empty);

            if (authors != null)
            {
                lines.AddRange(CreateAuthorSection(authors));
            }


            WriteToFile(lines.ToArray());
        }

        private static List<string> CreateAuthorSection(List<Author> authors)
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

        private static void WriteToFile(string[] lines)
        {
            using (var file = new StreamWriter(ReadmeFileName))
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

    public class Author
    {
        public string Name { get; set; }
        public string NickName { get; set; }
        public string WebSiteUrl { get; set; }
    }
}

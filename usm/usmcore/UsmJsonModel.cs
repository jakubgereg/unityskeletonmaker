using System.Collections.Generic;

namespace usmcore
{
    /// <summary>
    /// This model shown how usm.json shoulld looks like
    /// !!!If you do new changes here, you should automatically create new version throught UsmJsonHelper class
    /// </summary>
    public class UsmJsonModel
    {
        public string UsmJsonVersion { get; set; }
        public ProjectSettings ProjectSettings { get; set; }
    }

    public class ProjectSettings
    {
        public string GameName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public List<Author> Authors { get; set; }

        public class Author
        {
            public string Name { get; set; }
            public string NickName { get; set; }
            public string WebSiteUrl { get; set; }
        }
    }


}

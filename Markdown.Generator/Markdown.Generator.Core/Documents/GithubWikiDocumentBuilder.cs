using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Markdown.Generator.Core.Markdown;

namespace Markdown.Generator.Core.Documents
{
    public class GithubWikiDocumentBuilder<T> : IDocumentBuilder<T> where T: IMarkdownGenerator
    {
        private readonly T _markdownGenerator;

        public GithubWikiDocumentBuilder(T markdownGenerator)
        {
            _markdownGenerator = markdownGenerator;
        }
        
        public void Generate(Type[] types, string folder)
        {
            var mdTypes = _markdownGenerator.Load(types);
            CreateDocumentation(mdTypes, folder);
        }

        public void Generate(string dllPath, string namespaceMatch, string folder)
        {
            var mdTypes = _markdownGenerator.Load(dllPath, namespaceMatch);
            CreateDocumentation(mdTypes, folder);
        }

        public void Generate(Assembly[] assemblies, string namespaceMatch, string folder)
        {
            var mdTypes = _markdownGenerator.Load(assemblies, namespaceMatch);
            CreateDocumentation(mdTypes, folder);
        }

        private void CreateDocumentation(MarkdownableType[] types, string folder)
        {
            if (!Directory.Exists(folder)) 
                Directory.CreateDirectory(folder);
            
            var homeBuilder = new MarkdownBuilder();
            homeBuilder.Header(1, "References");
            homeBuilder.AppendLine();

            foreach (var group in types.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
            {
                homeBuilder.HeaderWithLink(2, group.Key, group.Key);
                homeBuilder.AppendLine();

                var sb = new StringBuilder();
                foreach (var item in group.OrderBy(x => x.Name))
                {
                    homeBuilder.ListLink(MarkdownBuilder.MarkdownCodeQuote(item.BeautifyName), group.Key + "#" + item.BeautifyName.Replace("<", "").Replace(">", "").Replace(",", "").Replace(" ", "-").ToLower());

                    sb.Append(item);
                }

                File.WriteAllText(Path.Combine(folder, group.Key + ".md"), sb.ToString());
                homeBuilder.AppendLine();
            }

            // Gen Home
            File.WriteAllText(Path.Combine(folder, "Home.md"), homeBuilder.ToString());
        }
    }
}
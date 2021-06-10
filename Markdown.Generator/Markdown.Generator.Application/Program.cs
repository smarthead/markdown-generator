using System;
using System.IO;
using System.Linq;
using System.Text;
using Markdown.Generator.Core;

namespace Markdown.Generator.Application
{
    class Program
    {
        // 0 = dll src path, 1 = dest root
        static void Main(string[] args)
        {
            // put dll & xml on same diretory.
            var target = String.Empty;
            var dest = "docs";
            var namespaceMatch = string.Empty;
            
            switch (args.Length)
            {
                case 1:
                    target = args[0];
                    break;
                case 2:
                    target = args[0];
                    dest = args[1];
                    break;
                case 3:
                    target = args[0];
                    dest = args[1];
                    namespaceMatch = args[2];
                    break;
            }

            var types = MarkdownGenerator.Load(target, namespaceMatch);

            // Home Markdown Builder
            var homeBuilder = new MarkdownBuilder();
            homeBuilder.Header(1, "References");
            homeBuilder.AppendLine();

            foreach (var group in types.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
            {
                if (!Directory.Exists(dest)) Directory.CreateDirectory(dest);

                homeBuilder.HeaderWithLink(2, group.Key, group.Key);
                homeBuilder.AppendLine();

                var sb = new StringBuilder();
                foreach (var item in group.OrderBy(x => x.Name))
                {
                    homeBuilder.ListLink(MarkdownBuilder.MarkdownCodeQuote(item.BeautifyName), group.Key + "#" + item.BeautifyName.Replace("<", "").Replace(">", "").Replace(",", "").Replace(" ", "-").ToLower());

                    sb.Append(item.ToString());
                }

                File.WriteAllText(Path.Combine(dest, group.Key + ".md"), sb.ToString());
                homeBuilder.AppendLine();
            }

            // Gen Home
            File.WriteAllText(Path.Combine(dest, "Home.md"), homeBuilder.ToString());
        }
    }
}
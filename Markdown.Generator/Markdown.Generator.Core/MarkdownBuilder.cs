using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Generator.Core.Elements;

namespace Markdown.Generator.Core
{
    public class MarkdownBuilder
    {
        private readonly List<ElementBase> _elements = new();
        
        public static string MarkdownCodeQuote(string code)
        {
            return "`" + code + "`";
        }

        public void Append(string text) 
            => _elements.Add(new Text(text));

        public void AppendLine(string text = null) 
            => _elements.Add(new NewLine(text));

        public void Header(int level, string text) => _elements.Add(new Header(level, text));

        public void HeaderWithCode(int level, string code) => _elements.Add(new Header<CodeQuote>(level, new CodeQuote(code)));

        public void HeaderWithLink(int level, string text, string url) => _elements.Add(new Header<Link>(level, new Link(text, url)));

        public void Link(string text, string url) => _elements.Add(new Link(text, url));

        public void Image(string altText, string imageUrl) => _elements.Add(new Image(altText, imageUrl));

        public void Code(string language, string code) => _elements.Add(new Code(language, code));

        public void CodeQuote(string code) => _elements.Add(new CodeQuote(code));

        public void Table(string[] headers, IEnumerable<string[]> items) => _elements.Add(new Table(headers, items));

        public void List(string text) // nest zero
            => _elements.Add(new List(text));

        public void ListLink(string text, string url) // nest zero
            => _elements.Add(new LinkList(new Link(text, url)));

        public override string ToString()
        {
            var builder = new StringBuilder();

            _elements.Aggregate(builder, (b, element) => b.Append(element.Create()));

            return builder.ToString();
        }
    }
}

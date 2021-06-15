using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Generator.Core.Markdown.Elements;

namespace Markdown.Generator.Core.Markdown
{
    public class MarkdownBuilder
    {
        private readonly List<ElementBase> _elements = new();

        public IEnumerable<ElementBase> Elements => _elements;
        
        public static string MarkdownCodeQuote(string code)
        {
            return new CodeQuote(code).Create();
        }

        public void Append(string text) 
            => _elements.Add(new Text(text));

        public void AppendLine(string text = null) 
            => _elements.Add(new NewLine(text));

        public void Header(int level, string text) => _elements.Add(new Header(level, text));

        public void HeaderWithCode(int level, string code) => _elements.Add(new Header(level, new CodeQuote(code)));

        public void HeaderWithLink(int level, string text, string url) => _elements.Add(new Header(level, new Link(text, url)));

        public void Link(string text, string url) => _elements.Add(new Link(text, url));

        public void Image(string altText, string imageUrl) => _elements.Add(new Image(altText, imageUrl));

        public void Code(string language, string code) => _elements.Add(new Code(language, code));

        public void CodeQuote(string code) => _elements.Add(new CodeQuote(code));

        public void Table(string[] headers, IEnumerable<string[]> items) => _elements.Add(new Table(headers, items));

        public void List(string text)
            => _elements.Add(new List(text));

        public void ListLink(string text, string url)
            => _elements.Add(new List(new Link(text, url)));

        public override string ToString()
        {
            var builder = new StringBuilder();

            _elements.Aggregate(builder, (b, element) => b.Append(element.Create()));
        
            return builder.ToString();
        }
    }
}

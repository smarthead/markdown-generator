using System.Collections.Generic;

namespace Markdown.Generator.Core.Markdown.Elements
{
    public class Table: ElementBase
    {
        private readonly string[] _headers;
        private readonly IEnumerable<string[]> _items;

        public Table(string[] headers, IEnumerable<string[]> items)
        {
            _headers = headers;
            _items = items;
        }
        
        public override string Create()
        {
            Builder.Append("| ");
            foreach (var item in _headers)
            {
                Builder.Append(item);
                Builder.Append(" | ");
            }
            Builder.AppendLine();

            Builder.Append("| ");
            foreach (var item in _headers)
            {
                Builder.Append("---");
                Builder.Append(" | ");
            }
            Builder.AppendLine();


            foreach (var item in _items)
            {
                Builder.Append("| ");
                foreach (var item2 in item)
                {
                    Builder.Append(item2);
                    Builder.Append(" | ");
                }
                Builder.AppendLine();
            }
            Builder.AppendLine();

            return Builder.ToString();
        }
    }
}
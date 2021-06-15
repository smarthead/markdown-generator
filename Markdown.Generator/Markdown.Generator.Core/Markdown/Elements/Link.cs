namespace Markdown.Generator.Core.Markdown.Elements
{
    /// <summary>
    /// Represents markdown link
    /// e.g [title](url)
    /// </summary>
    public class Link : ElementBase
    {
        private readonly string _text;
        private readonly string _url;

        public Link(string text, string url)
        {
            _text = text;
            _url = url;
        }
        public override string Create()
        {
            Builder.Append('[');
            Builder.Append(_text);
            Builder.Append(']');
            Builder.Append('(');
            Builder.Append(_url);
            Builder.Append(')');

            return Builder.ToString();
        }
    }
}
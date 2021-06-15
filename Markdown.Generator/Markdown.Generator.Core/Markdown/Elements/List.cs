namespace Markdown.Generator.Core.Markdown.Elements
{
    /// <summary>
    /// Represents markdown list item
    /// e.g - list\n
    /// </summary>
    public class List : ElementBase
    {
        private readonly string _text;

        public List(string text)
        {
            _text = text;
        }
        
        public List(Link link): this(link.Create())
        {
        }
        
        public override string Create()
        {
            Builder.Append("- ");
            Builder.AppendLine(_text);

            return Builder.ToString();
        }
    }
}
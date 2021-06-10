namespace Markdown.Generator.Core.Elements
{
    public class List : ElementBase
    {
        private readonly string _text;

        public List(string text)
        {
            _text = text;
        }
        
        public override string Create()
        {
            Builder.Append("- ");
            Builder.AppendLine(_text);

            return Builder.ToString();
        }
    }
    
    public class LinkList : List
    {
        public LinkList(Link link) : base(link.Create())
        {
        }
    }
}
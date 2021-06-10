namespace Markdown.Generator.Core.Elements
{
    public class Header : ElementBase
    {
        private readonly int _level;
        private readonly string _text;

        public Header(int level, string text)
        {
            _level = level;
            _text = text;
        }

        public override string Create()
        {
            for (var i = 0; i < _level; i++)
            {
                Builder.Append('#');
            }
            Builder.Append(' ');
            Builder.AppendLine(_text);

            return Builder.ToString();
        }
    }
    
    public class Header<T> : Header
        where T: ElementBase 
    {
        public Header(int level, T element) : base(level, element.Create())
        {
        }
    }
}
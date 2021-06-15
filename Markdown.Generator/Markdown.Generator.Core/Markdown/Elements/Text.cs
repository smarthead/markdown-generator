namespace Markdown.Generator.Core.Markdown.Elements
{
    public class Text : ElementBase
    {
        private readonly string _text;

        public Text(string text)
        {
            _text = text;
        }
        
        public override string Create()
        {
            Builder.Append(_text);

            return Builder.ToString();
        }
    }
}
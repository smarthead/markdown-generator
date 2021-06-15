namespace Markdown.Generator.Core.Markdown.Elements
{
    public class NewLine : ElementBase
    {
        private readonly string _text;

        public NewLine(string text = null)
        {
            _text = text;
        }
        
        public override string Create()
        {
            if (_text != null) 
                Builder.AppendLine(_text);
            else 
                Builder.AppendLine();
            return Builder.ToString();
        }
    }
}
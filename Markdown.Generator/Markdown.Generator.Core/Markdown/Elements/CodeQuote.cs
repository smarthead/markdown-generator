namespace Markdown.Generator.Core.Markdown.Elements
{
    /// <summary>
    /// Represents markdown single-line code-block
    /// e.g `code` 
    /// </summary>
    public class CodeQuote : ElementBase
    {
        private readonly string _code;

        public CodeQuote(string code)
        {
            _code = code;
        }
        
        public override string Create()
        {
            Builder.Append("```");
            Builder.Append(_code);
            Builder.Append("```");

            return Builder.ToString();
        }
    }
}
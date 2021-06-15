namespace Markdown.Generator.Core.Markdown.Elements
{
    /// <summary>
    /// Represents markdown code-block
    /// e.g ```csharp\nRandom string\n```\n 
    /// </summary>
    public class Code: ElementBase
    {
        private readonly string _language;
        private readonly string _code;

        public Code(string language, string code)
        {
            _language = language;
            _code = code;
        }
        
        public override string Create()
        {
            Builder.Append("```");
            Builder.AppendLine(_language);
            Builder.AppendLine(_code);
            Builder.AppendLine("```");

            return Builder.ToString();
        }
    }
}
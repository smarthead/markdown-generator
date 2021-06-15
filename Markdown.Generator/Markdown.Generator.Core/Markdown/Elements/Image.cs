namespace Markdown.Generator.Core.Markdown.Elements
{
    /// <summary>
    /// Represents markdown image
    /// e.g ![title](url)
    /// </summary>
    public class Image: ElementBase
    {
        private readonly string _altText;
        private readonly string _imageUrl;

        public Image(string altText, string imageUrl)
        {
            _altText = altText;
            _imageUrl = imageUrl;
        }
        
        public override string Create()
        {
            Builder.Append('!');
            Builder.Append(new Link(_altText, _imageUrl).Create());

            return Builder.ToString();
        }
    }
}
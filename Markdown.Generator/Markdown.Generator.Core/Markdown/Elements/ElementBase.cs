using System.Text;

namespace Markdown.Generator.Core.Markdown.Elements
{
    public abstract class ElementBase
    {
        protected readonly StringBuilder Builder = new();
        public abstract string Create();
    }
}
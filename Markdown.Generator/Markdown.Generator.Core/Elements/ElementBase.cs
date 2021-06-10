using System.Text;

namespace Markdown.Generator.Core.Elements
{
    public abstract class ElementBase
    {
        protected readonly StringBuilder Builder = new();
        public abstract string Create();
    }
}
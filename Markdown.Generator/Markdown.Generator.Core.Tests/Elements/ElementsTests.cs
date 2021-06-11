using Markdown.Generator.Core.Elements;
using Xunit;

namespace Markdown.Generator.Core.Tests.Elements
{
    public class ElementsTests
    {
        [Fact]
        public void Code_ShouldReturn_Markdown()
        {
            var expected = "```csharp\nRandom string\n```\n";
            
            var code = new Code("csharp", "Random string");

            var result = code.Create();
            
            Assert.Equal(expected, result);
        }
        
        [Fact]
        public void Ling_ShouldReturn_Markdown()
        {
            var expected = "```csharp\nRandom string\n```\n";
            
            var code = new List("list");

            var result = code.Create();
            
            Assert.Equal(expected, result);
        }
    }
}
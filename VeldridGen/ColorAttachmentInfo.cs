using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridGen
{
    public class ColorAttachmentInfo
    {
        public ColorAttachmentInfo(AttributeData attrib)
        {
            Format = attrib.ConstructorArguments[0].ToCSharpString();
        }

        public string Format { get; }
    }
}

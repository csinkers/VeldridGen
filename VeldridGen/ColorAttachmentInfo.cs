using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridGen
{
    class ColorAttachmentInfo
    {
        public ColorAttachmentInfo(AttributeData attrib)
        {
            Format = attrib.ConstructorArguments[0].ToCSharpString();
        }

        public string Format { get; }
    }
}

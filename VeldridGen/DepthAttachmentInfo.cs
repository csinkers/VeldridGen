using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridGen
{
    class DepthAttachmentInfo
    {
        public DepthAttachmentInfo(AttributeData attrib)
        {
            Format = attrib.ConstructorArguments[0].ToCSharpString();
        }

        public string Format { get; }
    }
}

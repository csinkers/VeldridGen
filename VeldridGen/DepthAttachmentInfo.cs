using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridGen
{
    public class DepthAttachmentInfo
    {
        public DepthAttachmentInfo(AttributeData attrib)
        {
            Format = attrib.ConstructorArguments[0].ToCSharpString();
        }

        public string Format { get; }
    }
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridGen;

public class DepthAttachmentInfo(AttributeData attrib)
{
    public string Format { get; } = attrib.ConstructorArguments[0].ToCSharpString();
}

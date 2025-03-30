using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridGen;

public class ColorAttachmentInfo(AttributeData attrib)
{
    public string Format { get; } = attrib.ConstructorArguments[0].ToCSharpString();
}

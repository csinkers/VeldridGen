using System;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    class DepthAttachmentInfo
    {
        public DepthAttachmentInfo(AttributeData attrib)
        {
            var value = attrib.ConstructorArguments[0].Value;
            Format = value ?? throw new ArgumentOutOfRangeException("Depth attachment attribute did not contain a pixel format parameter");
        }

        public object Format { get; }
    }
}

using System;
using Microsoft.CodeAnalysis;
using Veldrid;

namespace VeldridCodeGen
{
    class DepthAttachmentInfo
    {
        public DepthAttachmentInfo(AttributeData attrib)
        {
            var value = attrib.ConstructorArguments[0].Value;
            Format = (PixelFormat?)value ?? throw new ArgumentOutOfRangeException("Depth attachment attribute did not contain a pixel format parameter");
        }

        public PixelFormat Format { get; }
    }
}
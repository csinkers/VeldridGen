using System;
using Microsoft.CodeAnalysis;
using Veldrid;

namespace VeldridCodeGen
{
    class ColorAttachmentInfo
    {
        public ColorAttachmentInfo(AttributeData attrib)
        {
            var value = attrib.ConstructorArguments[0].Value;
            Format = (PixelFormat?)value ?? throw new ArgumentOutOfRangeException("Color attachment attribute did not contain a pixel format parameter");
        }

        public PixelFormat Format { get; }
    }
}
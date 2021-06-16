using System;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    class ColorAttachmentInfo
    {
        public ColorAttachmentInfo(AttributeData attrib)
        {
            var value = attrib.ConstructorArguments[0].Value;
            Format = value ?? throw new ArgumentOutOfRangeException("Color attachment attribute did not contain a pixel format parameter");
        }

        public object Format { get; }
    }
}

using System;

namespace VeldridCodeGen
{
    public class ShaderInfo
    {
        public Type VertexFormat { get; set; }
        public Type InstanceFormat { get; set; }
        public Type[] ResourceSets { get; set; }
    }
}
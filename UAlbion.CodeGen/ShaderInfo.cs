using System;

namespace UAlbion.CodeGen
{
    public class ShaderInfo
    {
        public Type VertexFormat { get; set; }
        public Type InstanceFormat { get; set; }
        public Type[] ResourceSets { get; set; }
    }
}
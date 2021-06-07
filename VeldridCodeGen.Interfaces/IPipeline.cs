using System;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public interface IPipeline : IDisposable
    {
        public Veldrid.Pipeline DevicePipeline { get; }
        public bool UseDepthTest { get; set; }
        public bool UseScissorTest { get; set; }
        public DepthStencilStateDescription DepthStencilMode { get; set; }
        public FaceCullMode CullMode { get; set; }
        public PrimitiveTopology Topology { get; set; }
        public PolygonFillMode FillMode { get; set; }
        public FrontFace Winding { get; set; }
        public BlendStateDescription AlphaBlend { get; set; }
        public OutputDescription? OutputDescription { get; set; }
        public string Name { get; set; }
    }
}
﻿using System;
using System.Linq;
using UAlbion.Core.SpriteBatch;
using Veldrid;

namespace UAlbion.Core
{
    public class Pipeline : Component, IDisposable
    {
        readonly object _syncRoot = new();
        readonly string _vertexShaderName;
        readonly string _fragmentShaderName;
        readonly VertexLayoutDescription[] _vertexLayouts;
        readonly ResourceLayoutDescription[] _resourceLayouts;

        Veldrid.Pipeline _pipeline;
        Shader[] _shaders;
        string _name;
        bool _useDepthTest = true;
        bool _useScissorTest = true;
        OutputDescription? _outputDescription;
        FrontFace _winding = FrontFace.Clockwise;
        FaceCullMode _cullMode = FaceCullMode.None;
        PolygonFillMode _fillMode = PolygonFillMode.Solid;
        PrimitiveTopology _topology = PrimitiveTopology.TriangleList;
        BlendStateDescription _alphaBlend = BlendStateDescription.SingleAlphaBlend;
        DepthStencilStateDescription _depthStencilMode = DepthStencilStateDescription.DepthOnlyLessEqual;

        public Pipeline(string vertexShaderName, string fragmentShaderName, VertexLayoutDescription[] vertexLayouts, ResourceLayoutDescription[] resourceLayouts)
        {
            _vertexShaderName = vertexShaderName ?? throw new ArgumentNullException(nameof(vertexShaderName));
            _fragmentShaderName = fragmentShaderName ?? throw new ArgumentNullException(nameof(fragmentShaderName));
            _vertexLayouts = vertexLayouts ?? throw new ArgumentNullException(nameof(vertexLayouts));
            _resourceLayouts = resourceLayouts ?? throw new ArgumentNullException(nameof(resourceLayouts));

            On<CreateDeviceObjectsEvent>(e => Dirty());
            On<DestroyDeviceObjectsEvent>(e => Dispose());
            Dirty();
        }

        public Veldrid.Pipeline DevicePipeline => _pipeline;
        public bool UseDepthTest { get => _useDepthTest; set { _useDepthTest = value; Dirty(); } } 
        public bool UseScissorTest { get => _useScissorTest; set { _useScissorTest = value; Dirty(); } } 
        public DepthStencilStateDescription DepthStencilMode { get => _depthStencilMode; set { _depthStencilMode = value; Dirty(); } } 
        public FaceCullMode CullMode { get => _cullMode; set { _cullMode = value; Dirty(); } } 
        public PrimitiveTopology Topology { get => _topology; set { _topology = value; Dirty(); } } 
        public PolygonFillMode FillMode { get => _fillMode; set { _fillMode = value; Dirty(); } } 
        public FrontFace Winding { get => _winding; set { _winding = value; Dirty(); } } 
        public BlendStateDescription AlphaBlend { get => _alphaBlend; set { _alphaBlend = value; Dirty(); } } 
        public OutputDescription? OutputDescription { get => _outputDescription; set { _outputDescription = value; Dirty(); } }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                lock (_syncRoot)
                {
                    if (_pipeline != null)
                        _pipeline.Name = value;
                }
            }
        }


        protected override void Subscribed() => Dirty();
        protected override void Unsubscribed() => Dispose();
        void Dirty() => On<PrepareFrameResourcesEvent>(e => Update(e.Device));

        void Update(GraphicsDevice device)
        {
            lock (_syncRoot)
            {
                Dispose();

                if (OutputDescription == null && device.SwapchainFramebuffer == null)
                    throw new InvalidOperationException("An output description must be specified when running headless (i.e. without a primary swapchain)");

                var shaderSource = Resolve<IShaderSource>();
                var vertexShaderContent = shaderSource.GetGlsl(_vertexShaderName);
                var fragmentShaderContent = shaderSource.GetGlsl(_fragmentShaderName);

                _shaders = shaderSource.GetShaderPair(
                    device.ResourceFactory,
                    _vertexShaderName, _fragmentShaderName,
                    vertexShaderContent, fragmentShaderContent);

                var layoutSource = Resolve<IResourceLayoutSource>();
                var shaderSetDescription = new ShaderSetDescription(
                    _vertexLayouts,
                    _shaders,
                    Array.Empty<SpecializationConstant>()); // TODO: Add specialisation constant support

                var pipelineDescription = new GraphicsPipelineDescription(
                    AlphaBlend,
                    DepthStencilMode,
                    new RasterizerStateDescription(CullMode, FillMode, Winding, UseDepthTest, UseScissorTest),
                    Topology,
                    shaderSetDescription,
                    _resourceLayouts.Select(layoutSource.Get).ToArray(),
                    OutputDescription ?? device.SwapchainFramebuffer.OutputDescription);

                _pipeline = device.ResourceFactory.CreateGraphicsPipeline(ref pipelineDescription);
                _pipeline.Name = Name;
            }
        }

        public void Dispose()
        {
            lock (_syncRoot)
            {
                if (_shaders != null)
                {
                    foreach (var shader in _shaders)
                        shader.Dispose();
                    _shaders = null;
                }

                _pipeline?.Dispose();
                _pipeline = null;
            }
        }
    }
}
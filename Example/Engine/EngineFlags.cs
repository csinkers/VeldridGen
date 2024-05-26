using System;

namespace VeldridGen.Example.Engine;

[Flags]
public enum EngineFlags
{
    ShowBoundingBoxes = 0x1,
    ShowCentre = 0x2,
    FlipDepthRange = 0x4,
    FlipYSpace = 0x8,
    Vsync = 0x10,
    HighlightSelection = 0x20,
    RenderDepth = 0x40
}
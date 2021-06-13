namespace UAlbion.Core
{
    public enum EngineFlags : uint
    {
        ShowBoundingBoxes = 0X1,
        ShowCentre = 0X2,
        FlipDepthRange = 0X4,
        FlipYSpace = 0X8,
        Vsync = 0X10,
        HighlightSelection = 0X20,
        UseCylBillboards = 0X40,
        RenderDepth = 0X80
    }
}

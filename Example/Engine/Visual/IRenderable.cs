namespace VeldridGen.Example.Engine.Visual
{
    public interface IRenderable
    {
        string Name { get; }
        uint RenderOrder { get; }
    }
}

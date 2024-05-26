using System.Collections.Generic;

namespace VeldridGen.Example.Engine.Visual;

public interface IRenderableSource
{
    void Collect(List<IRenderable> renderables);
}
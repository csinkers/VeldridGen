using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit;

namespace VeldridCodeGen.Tests
{
    public class ResourceSetTests
    {
        [Fact]
        public void ResourceSetTest1()
        {
            const string source = 
                @"using System.ComponentModel;
using System.Numerics;
using VeldridCodeGen.Interfaces;
using Veldrid;

namespace VeldridCodeGenTests
{
" + 
                BaseClasses.ResourceSetHolderSource +
                BaseClasses.SingleBufferSource +
                BaseClasses.Texture2DHolderSource + @"
    public struct GlobalInfo
    {
        public Vector3 CameraPosition;
        public float Time;
    }

    public sealed partial class CommonSet : ResourceSetHolder
    {
        [Resource(""_Shared"")]                         SingleBuffer<GlobalInfo>     _globalInfo; 
        [Resource(""uPalette"", ShaderStages.Fragment)] Texture2DHolder              _palette;
    }
}
";
            Compilation comp = TestCommon.CreateCompilation(source);
            Compilation newComp = TestCommon.RunGenerators(comp, out var generatorDiags, new VeldridGenerator());
            var generatedTrees = newComp.RemoveSyntaxTrees(comp.SyntaxTrees).SyntaxTrees.ToList();

            Assert.Equal(3, generatedTrees.Count);
            Assert.Empty(generatorDiags);
            Assert.Empty(newComp.GetDiagnostics());
        }
    }
}

using System.Linq;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    public class UniformInfo
    {
        public UniformInfo(AttributeData attrib)
        {
            Name = (string)attrib.ConstructorArguments[0].Value;
            EnumPrefix = 
                (string)attrib.NamedArguments
                    .Where(x => x.Key == "EnumPrefix")
                    .Select(x => (TypedConstant?)x.Value)
                    .SingleOrDefault()?.Value;
        }

        public string Name { get; }
        public string EnumPrefix { get; }
    }
}
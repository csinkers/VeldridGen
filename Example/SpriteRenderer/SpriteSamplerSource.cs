using System;
using Veldrid;
using VeldridGen.Example.Engine;
using VeldridGen.Example.Engine.Visual.Sprites;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.SpriteRenderer
{
    public sealed class SpriteSamplerSource : ServiceComponent<ISpriteSamplerSource>, ISpriteSamplerSource, IDisposable
    {
        readonly SamplerHolder _linearSampler;
        readonly SamplerHolder _pointSampler;

        public SpriteSamplerSource()
        {
            _linearSampler = new SamplerHolder
            {
                AddressModeU = SamplerAddressMode.Clamp,
                AddressModeV = SamplerAddressMode.Clamp,
                AddressModeW = SamplerAddressMode.Clamp,
                BorderColor = SamplerBorderColor.TransparentBlack,
                Filter = SamplerFilter.MinLinear_MagLinear_MipLinear,
            };

            _pointSampler = new SamplerHolder
            {
                AddressModeU = SamplerAddressMode.Clamp,
                AddressModeV = SamplerAddressMode.Clamp,
                AddressModeW = SamplerAddressMode.Clamp,
                BorderColor = SamplerBorderColor.TransparentBlack,
                Filter = SamplerFilter.MinPoint_MagPoint_MipPoint,
            };
            AttachChild(_linearSampler);
            AttachChild(_pointSampler);
        }

        public ISamplerHolder GetSampler(SpriteSampler sampler) =>
            sampler switch
            {
                SpriteSampler.Linear => _linearSampler,
                SpriteSampler.Point => _pointSampler,
                _ => throw new ArgumentOutOfRangeException(nameof(sampler), "Unexpected sprite sampler \"{sampler}\"")
            };

        public void Dispose()
        {
            _linearSampler?.Dispose();
            _pointSampler?.Dispose();
        }
    }
}
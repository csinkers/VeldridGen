using System;
using UAlbion.Core.Sprites;
using UAlbion.Core.Veldrid;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    public class SpriteSamplerSource : Component, ISpriteSamplerSource
    {
        readonly SamplerHolder DefaultSampler;
        readonly SamplerHolder PointSampler;
        public SpriteSamplerSource()
        {
            DefaultSampler = AttachChild(new SamplerHolder
            {
                AddressModeU = SamplerAddressMode.Clamp,
                AddressModeV = SamplerAddressMode.Clamp,
                AddressModeW = SamplerAddressMode.Clamp,
                BorderColor = SamplerBorderColor.TransparentBlack,
                Filter = SamplerFilter.MinLinear_MagLinear_MipLinear,
            });

            PointSampler = AttachChild(new SamplerHolder
            {
                AddressModeU = SamplerAddressMode.Clamp,
                AddressModeV = SamplerAddressMode.Clamp,
                AddressModeW = SamplerAddressMode.Clamp,
                BorderColor = SamplerBorderColor.TransparentBlack,
                Filter = SamplerFilter.MinPoint_MagPoint_MipPoint,
            });
        }

        public SamplerHolder Get(SpriteSampler sampler) =>
            sampler switch
            {
                SpriteSampler.Default => DefaultSampler,
                SpriteSampler.Point => PointSampler,
                _ => throw new ArgumentOutOfRangeException(nameof(sampler), "Unexpected sprite sampler \"{sampler}\"")
            };
    }
}
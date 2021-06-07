using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public interface ISamplerHolder : INotifyPropertyChanged, IDisposable
    {
        Sampler Sampler { get; }
        SamplerAddressMode AddressModeU { get; set; }
        SamplerAddressMode AddressModeV { get; set; }
        SamplerAddressMode AddressModeW { get; set; }
        SamplerFilter Filter { get; set; }
        ComparisonKind? ComparisonKind { get; set; }
        uint MaximumAnisotropy { get; set; }
        uint MinimumLod { get; set; }
        uint MaximumLod { get; set; }
        int LodBias { get; set; }
        SamplerBorderColor BorderColor { get; set; }
    }
}
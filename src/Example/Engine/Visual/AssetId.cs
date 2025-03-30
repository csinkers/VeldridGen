using System.Globalization;

namespace VeldridGen.Example.Engine.Visual;

public class AssetId : IAssetId
{
    readonly uint _value;
    readonly string _name;

    public AssetId(uint value, string name)
    {
        _value = value;
        _name = name ?? _value.ToString(CultureInfo.InvariantCulture);
    }

    public uint ToUInt32() => _value;
    public override string ToString() => _name;
    public string ToStringNumeric() => _value.ToString(CultureInfo.InvariantCulture);
}

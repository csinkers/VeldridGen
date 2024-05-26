using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Example.Engine;

public abstract class TextureHolder
{
    Texture _deviceTexture;

    public string Name { get; set; }
    protected TextureHolder(string name) => Name = name;
    protected abstract void Validate(Texture texture);

    public Texture DeviceTexture
    {
        get => _deviceTexture;
        set
        {
            if (_deviceTexture == value)
                return;

            Validate(value);
            _deviceTexture = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceTexture)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}

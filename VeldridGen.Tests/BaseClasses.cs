namespace VeldridGen.Tests
{
    static class BaseClasses
    {
        public const string ResourceSetHolderSource = @"
    public abstract class ResourceSetHolder : IResourceSetHolder
    {
        ResourceSet _resourceSet;
        string _name;

        public ResourceSet ResourceSet => _resourceSet;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (_resourceSet != null)
                    _resourceSet.Name = _name;
            }
        }

        protected void Dirty() { /* dummy stub */ }
        protected void PropertyDirty(object sender, PropertyChangedEventArgs _) => Dirty();

        protected abstract ResourceSet Build(GraphicsDevice device, ResourceLayout layout);

        public void Dispose()
        {
            _resourceSet?.Dispose();
            _resourceSet = null;
        }
    }
";

        public const string SingleBufferSource = @"
    public class SingleBuffer<T> : IBufferHolder<T> where T : struct // GPU buffer containing a single instance of T
    {
        readonly BufferUsage _usage;
        string _name;
        T _instance;

        public DeviceBuffer DeviceBuffer { get; private set; }

        public T Data
        {
            get => _instance;
            set
            {
                _instance = value;
                Dirty();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (DeviceBuffer != null)
                    DeviceBuffer.Name = _name;
            }
        }

        public SingleBuffer(in T data, BufferUsage usage, string name = null)
        {
            _instance = data;
            _usage = usage;
            _name = name;
        }

        void Dirty() { /* dummy stub */ }
        public void Dispose() { }
    }
";

        public const string Texture2DHolderSource = @"
    public class Texture2DHolder : ITextureHolder
    {
        Texture _deviceTexture;
        TextureView _textureView;

        public Texture DeviceTexture
        {
            get => _deviceTexture;
            private set
            {
                if (_deviceTexture == value)
                    return;
                _deviceTexture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceTexture)));
            }
        }

        public TextureView TextureView
        {
            get => _textureView;
            private set
            {
                if (_textureView == value)
                    return;
                _textureView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextureView)));
            }
        }

        void Dirty() { /* dummy stub */ }
        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
    }
";
    }
}

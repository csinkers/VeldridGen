using System;

namespace UAlbion.Core.Visual
{
    public class Description
    {
        public Type Type { get; set; }
        public string Name { get; set; }

        public static Description Create<T>(string name) 
            => new Description {Type = typeof(T), Name = name};
    }
}
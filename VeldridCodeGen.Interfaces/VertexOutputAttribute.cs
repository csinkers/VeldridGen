﻿using System;

namespace VeldridCodeGen.Interfaces
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class VertexOutputAttribute : Attribute
    {
        public override object TypeId => this;
        public int Order { get; }
        public Type Type { get; }
        public VertexOutputAttribute(int order, Type type)
        {
            Order = order;
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
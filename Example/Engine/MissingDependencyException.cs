using System;

namespace VeldridGen.Example.Engine;

public class MissingDependencyException : Exception
{
    public MissingDependencyException() { }
    public MissingDependencyException(string message) : base(message) { }
    public MissingDependencyException(string message, Exception innerException) : base(message, innerException) { }
}
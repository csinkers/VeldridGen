namespace VeldridGen.Example.Engine;
// Some of these interfaces are checked for extremely frequently and the performance cost of using attributes instead would be excessive.
#pragma warning disable CA1040 // Avoid empty interfaces
public interface IEvent { }
public interface IVerboseEvent : IEvent { }
#pragma warning restore 1040 // Avoid empty interfaces
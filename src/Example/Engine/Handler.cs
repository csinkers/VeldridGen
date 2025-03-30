using System;

namespace VeldridGen.Example.Engine;

public abstract class Handler(Type type, IComponent component)
{
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));
    public IComponent Component { get; } = component ?? throw new ArgumentNullException(nameof(component));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <returns>True if the handler intends to call, or has already called the continuation.</returns>
    public abstract bool Invoke(IEvent e);
    public override string ToString() => $"H<{Component.GetType().Name}, {Type.Name}>";
}

public class Handler<TEvent>(Action<TEvent> callback, IComponent component)
    : Handler(typeof(TEvent), component)
{
    public Action<TEvent> Callback { get; } = callback;
    public override bool Invoke(IEvent e) { Callback((TEvent)e); return false; }
}

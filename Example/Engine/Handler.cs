using System;

namespace VeldridGen.Example.Engine;

public abstract class Handler
{
    public Type Type { get; }
    public IComponent Component { get; }
    protected Handler(Type type, IComponent component)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Component = component ?? throw new ArgumentNullException(nameof(component));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <returns>True if the handler intends to call, or has already called the continuation.</returns>
    public abstract bool Invoke(IEvent e);
    public override string ToString() => $"H<{Component.GetType().Name}, {Type.Name}>";
}

public class Handler<TEvent> : Handler
{
    public Action<TEvent> Callback { get; }
    public Handler(Action<TEvent> callback, IComponent component) : base(typeof(TEvent), component) => Callback = callback;
    public override bool Invoke(IEvent e) { Callback((TEvent)e); return false; }
}

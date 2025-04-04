﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
#if DEBUG
using VeldridGen.Example.Engine.Events;
#endif

#pragma warning disable CA1030 // Use events where appropriate
namespace VeldridGen.Example.Engine;

/// <summary>
/// An event exchange responsible for efficiently distributing events to all
/// components that have registered handlers for the event. Also acts as a 
/// service locator, via the Register / Resolve methods.
/// </summary>
public sealed class EventExchange : IDisposable
{
    readonly Lock _syncRoot = new();
    readonly Stack<List<Handler>> _dispatchLists = new();
    readonly Queue<(IEvent, object)> _queuedEvents = new();
    readonly IDictionary<Type, object> _registrations = new Dictionary<Type, object>();
    readonly IDictionary<Type, IList<Handler>> _subscriptions = new Dictionary<Type, IList<Handler>>();
    readonly IDictionary<IComponent, IList<Handler>> _subscribers = new Dictionary<IComponent, IList<Handler>>();

    int _nesting = -1;
    long _nextEventId;
    public int Nesting => _nesting;

#if DEBUG
    // ReSharper disable once CollectionNeverQueried.Local
    readonly IList<IEvent> _frameEvents = new List<IEvent>();
    IList<IComponent> _sortedSubscribersCached;
    public IList<IComponent> SortedSubscribers // Just for debugging
    {
        get
        {
            lock (_syncRoot)
            {
                if (_sortedSubscribersCached?.Count != _subscribers.Count)
                    _sortedSubscribersCached = _subscribers.Keys.OrderBy(x => x.ToString()).ToList();
                return _sortedSubscribersCached;
            }
        }
    }
#endif

    public void Dispose()
    {
        lock (_syncRoot)
            foreach (var disposableSystem in _registrations.Values.OfType<IDisposable>())
                disposableSystem.Dispose();
    }

    public EventExchange Attach(IComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);
        Stopwatch sw = Stopwatch.StartNew();
        component.Attach(this);
        PerfTracker.StartupEvent($"Attached {component.GetType().Name} in {sw.ElapsedMilliseconds}ms");
        return this;
    }

    public void Enqueue(IEvent e, object sender) { lock (_syncRoot) _queuedEvents.Enqueue((e, sender)); }

    public void FlushQueuedEvents()
    {
        IList<(IEvent, object)> events;
        lock (_syncRoot)
        {
            events = _queuedEvents.ToList();
            _queuedEvents.Clear();
        }

        foreach (var (e, sender) in events)
            Raise(e, sender);
    }

    void Collect(List<Handler> subscribers, Type eventType) // Must be called from inside lock(_syncRoot)!
    {
        if (!_subscriptions.TryGetValue(eventType, out var tempSubscribers))
            return;

        if (subscribers.Capacity < tempSubscribers.Count)
            subscribers.Capacity = tempSubscribers.Count;

        foreach (var subscriber in tempSubscribers)
            subscribers.Add(subscriber);
    }

    public void Raise(IEvent e, object sender)
    {
        ArgumentNullException.ThrowIfNull(e);
        bool verbose = e is IVerboseEvent;
        if (!verbose)
        { // Nesting level helps identify which events were caused by other events when reading the console window
            Interlocked.Increment(ref _nesting);
        }

#if DEBUG // Keep track of which events have been fired this frame for debugging
        if (e is BeginFrameEvent) _frameEvents.Clear();
        else _frameEvents.Add(e);
#endif

        long eventId = Interlocked.Increment(ref _nextEventId);
        string eventText = null;

        if (CoreTrace.Log.IsEnabled())
        {
            eventText = verbose ? e.GetType().Name : e.ToString();
            if (verbose) CoreTrace.Log.StartRaiseVerbose(eventId, _nesting, e.GetType().Name, eventText);
            else if (e is LogEvent log)
            {
                // ReSharper disable ExplicitCallerInfoArgument
                switch (log.Severity)
                {
                    case LogLevel.Info: CoreTrace.Log.Info("Log", log.Message, log.File, log.Member, log.Line ?? 0); break;
                    case LogLevel.Warning: CoreTrace.Log.Warning("Log", log.Message, log.File, log.Member, log.Line ?? 0); break;
                    case LogLevel.Error: CoreTrace.Log.Error("Log", log.Message, log.File, log.Member, log.Line ?? 0); break;
                    case LogLevel.Critical: CoreTrace.Log.Critical("Log", log.Message, log.File, log.Member, log.Line ?? 0); break;
                }
                // ReSharper restore ExplicitCallerInfoArgument
            }
            else CoreTrace.Log.StartRaise(eventId, _nesting, e.GetType().Name, eventText);
        }

        List<Handler> handlers;
        lock (_syncRoot)
        {
            if (!_dispatchLists.TryPop(out handlers)) // reuse the event handler lists to avoid GC churn
                handlers = new List<Handler>();
#if DEBUG
            if (e is BeginFrameEvent) _frameEvents.Clear();
#endif
            Collect(handlers, e.GetType());
        }

        foreach (var handler in handlers)
            if (sender != handler.Component)
                handler.Invoke(e);

        if (eventText != null)
        {
            if (verbose) CoreTrace.Log.StopRaiseVerbose(eventId, _nesting, e.GetType().Name, eventText, handlers.Count);
            else CoreTrace.Log.StopRaise(eventId, _nesting, e.GetType().Name, eventText, handlers.Count);
        }

        handlers.Clear();
        lock (_syncRoot)
            _dispatchLists.Push(handlers);

        if (!verbose)
            Interlocked.Decrement(ref _nesting);

    }

    public void Subscribe<T>(IComponent component) where T : IEvent
        => Subscribe(new Handler<T>(e => component.Receive(e, null), component));

    public void Subscribe(Handler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        lock (_syncRoot)
        {
            if (_subscribers.TryGetValue(handler.Component, out var subscribedTypes))
            {
                if (subscribedTypes.Any(x => x.Type == handler.Type))
                {
                    Raise(new LogEvent(
                        LogLevel.Error,
                        $"Component of type \"{handler.Component.GetType()}\" tried to register " +
                        $"handler for event {handler.Type}, but it has already registered a handler for that event."),
                        this);
                    return;
                }
            }
            else
            {
                _subscribers[handler.Component] = new List<Handler>();
            }

            if (handler.Type != null)
            {
                if (!_subscriptions.ContainsKey(handler.Type))
                    _subscriptions.Add(handler.Type, new List<Handler>());

                _subscriptions[handler.Type].Add(handler);
                _subscribers[handler.Component].Add(handler);
            }
        }
    }

    public void Unsubscribe(IComponent subscriber)
    {
        lock (_syncRoot)
        {
            if (!_subscribers.TryGetValue(subscriber, out var handlersForSubscriber))
                return;

            foreach (var handler in handlersForSubscriber.ToList())
                _subscriptions[handler.Type].Remove(handler);

            _subscribers.Remove(subscriber);
        }
    }

    public void Unsubscribe<T>(IComponent subscriber)
    {
        lock (_syncRoot)
        {
            if (!_subscribers.TryGetValue(subscriber, out var handlersForSubscriber))
                return;

            var handler = handlersForSubscriber.FirstOrDefault(x => x.Type == typeof(T));
            if (handler == null)
                return;

            handlersForSubscriber.Remove(handler);
            _subscriptions[typeof(T)].Remove(handler);
        }
    }

    public EventExchange Register<T>(T system) => Register(typeof(T), system);
    public EventExchange Register(Type type, object system, bool attach = true)
    {
        lock (_syncRoot)
        {
            if (_registrations.TryGetValue(type, out object registration))
            {
                if (registration != system)
                    throw new InvalidOperationException(Errors.ExchangeDoubleRegistration);

                attach = false;
            }
            else
            {
                _registrations.Add(type, system);
                attach &= system is IComponent component && !_subscribers.ContainsKey(component);
            }
        }

        if (attach)
            Attach((IComponent)system);

        return this;
    }

    public void Unregister(object system)
    {
        ArgumentNullException.ThrowIfNull(system);
        Unregister(system.GetType(), system);
        foreach (var i in system.GetType().GetInterfaces())
            Unregister(i, system);
    }

    public void Unregister(Type type, object system)
    {
        lock (_syncRoot)
        {
            if (_registrations.TryGetValue(type, out var current) && current == system)
                _registrations.Remove(type);
        }
    }

    public T Resolve<T>()
    {
        lock (_syncRoot)
            return _registrations.TryGetValue(typeof(T), out var result) ? (T)result : default;
    }

    public IEnumerable<IComponent> EnumerateRecipients(Type eventType)
    {
        ArgumentNullException.ThrowIfNull(eventType);
        lock (_syncRoot)
        {
            var subscribers = new List<Handler>();
            Collect(subscribers, eventType);
            return subscribers.Select(x => x.Component).ToList();
        }
    }
}
#pragma warning restore CA1030 // Use events where appropriate

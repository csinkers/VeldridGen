﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace VeldridGen.Example.Engine;

public static class PerfTracker
{
    // TODO: Enqueue console writes in debug mode to a queue with an output
    // task / thread, so ensure that writing to the console doesn't affect
    // the perf stats
    class FrameTimeTracker(string name) : IDisposable
    {
        readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public void Dispose()
        {
            lock (SyncRoot)
            {
                long ticks = _stopwatch.ElapsedTicks;
                if (!FrameTimes.ContainsKey(name))
                    FrameTimes[name] = new Stats { Fast = ticks };

                var stats = FrameTimes[name];
                stats.AddTicks(ticks);
            }
        }
    }

    class InfrequentTracker : IDisposable
    {
        readonly Stopwatch _stopwatch;
        readonly string _name;
        readonly long _initialTicks;

        public InfrequentTracker(string name, Stopwatch stopwatch)
        {
            _name = name;
            _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
            _initialTicks = _stopwatch.ElapsedTicks;
#if DEBUG
            var tid = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"[{tid}] at {_stopwatch.ElapsedMilliseconds}: Starting {_name}");
#endif
            CoreTrace.Log.StartupEvent(name);
        }

        public void Dispose()
        {
#if DEBUG
            var tid = Thread.CurrentThread.ManagedThreadId;
            var elapsedMs = (_stopwatch.ElapsedTicks - _initialTicks) * 1000 / Stopwatch.Frequency;
            Console.WriteLine($"[{tid}] at {_stopwatch.ElapsedMilliseconds}: Finished {_name} in {elapsedMs}");
#endif
            CoreTrace.Log.StartupEvent(_name);
        }
    }

    class Stats
    {
        public long Count { get; private set; }
        public long Total { get; private set; }
        public long Min { get; private set; } = long.MaxValue;
        public long Max { get; private set; } = long.MinValue;
        public float Fast { get; set; }

        public void AddTicks(long ticks)
        {
            Count++;
            Total += ticks;
            Fast = (ticks + 8 * Fast) / 9.0f;
            if (Min > ticks) Min = ticks;
            if (Max < ticks) Max = ticks;
        }

        public void AddMs(long ms) => AddTicks(ms * 10000);
    }

    static readonly Stopwatch StartupStopwatch = Stopwatch.StartNew();
    static readonly IDictionary<string, Stats> FrameTimes = new Dictionary<string, Stats>();
    static readonly IDictionary<string, int> FrameCounters = new Dictionary<string, int>();
    static readonly Lock SyncRoot = new();
    static int _frameCount;

    public static void BeginFrame()
    {
        if (_frameCount == 1)
            StartupEvent("First frame finished"); // Last startup event to be emitted

        _frameCount++;
        foreach (var key in FrameCounters.Keys.ToList())
        {
            var count = FrameCounters[key];
            if (!FrameTimes.ContainsKey(key))
                FrameTimes[key] = new Stats { Fast = count * 10000 };

            var stats = FrameTimes[key];
            stats.AddMs(count);

            FrameCounters[key] = 0;
        }
    }

    public static void StartupEvent(string name)
    {
        if (_frameCount > 1) return;
        //#if DEBUG
        var tid = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"[{tid}] at {StartupStopwatch.ElapsedMilliseconds}: {name}");
        //#endif
        CoreTrace.Log.StartupEvent(name);
    }

    public static IDisposable InfrequentEvent(string name) => new InfrequentTracker(name, StartupStopwatch);

    public static IDisposable FrameEvent(string name) => new FrameTimeTracker(name);

    public static void Clear()
    {
        lock (SyncRoot)
        {
            FrameTimes.Clear();
            _frameCount = 0;
        }
    }

    public static (IList<string>, IList<string>) GetFrameStats()
    {
        var sb = new StringBuilder();
        var descriptions = new List<string>();
        var results = new List<string>();
        lock (SyncRoot)
        {
            foreach (var kvp in FrameTimes.OrderBy(x => x.Key))
            {
                sb.Append($"Avg/frame: {(float)kvp.Value.Total / (10000 * _frameCount):F3}");
                sb.Append($" Min: {(float)kvp.Value.Min / 10000:F3}");
                sb.Append($" Max: {(float)kvp.Value.Max / 10000:F3}");
                sb.Append($" F:{kvp.Value.Fast / 10000:F3}");
                sb.Append($" Avg/call: {(float)kvp.Value.Total / (10000 * kvp.Value.Count):F3}");
                sb.Append($" Calls/Frame: {(float)kvp.Value.Count / _frameCount:F3}");
                sb.Append($" Total: {kvp.Value.Total / 10000}");
                sb.Append($" Count: {kvp.Value.Count}");
                descriptions.Add(kvp.Key);
                results.Add(sb.ToString());
                sb.Clear();
            }
        }

        return (descriptions, results);
    }

    public static void IncrementFrameCounter(string name)
    {
        lock (SyncRoot)
        {
            FrameCounters.TryGetValue(name, out var count);
            FrameCounters[name] = count + 1;
        }
    }
}

﻿namespace VeldridGen.Example.Engine;

public class FrameTimeAverager(double averagingIntervalSeconds)
{
    const double DecayRate = .3;
    double _accumulatedTime;
    int _frameCount;

    public double CurrentAverageFrameTimeSeconds { get; private set; }
    public double CurrentAverageFrameTimeMilliseconds => CurrentAverageFrameTimeSeconds * 1000.0;
    public double CurrentAverageFramesPerSecond => 1 / CurrentAverageFrameTimeSeconds;

    public void Reset()
    {
        _accumulatedTime = 0;
        _frameCount = 0;
    }

    public void AddTime(double seconds)
    {
        _accumulatedTime += seconds;
        _frameCount++;
        if (_accumulatedTime >= averagingIntervalSeconds)
            Average();
    }

    void Average()
    {
        double total = _accumulatedTime;
        CurrentAverageFrameTimeSeconds =
            CurrentAverageFrameTimeSeconds * DecayRate
            + (total / _frameCount) * (1 - DecayRate);

        _accumulatedTime = 0;
        _frameCount = 0;
    }
}

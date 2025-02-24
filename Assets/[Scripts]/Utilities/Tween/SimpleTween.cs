public static class SimpleTween
{
    /// <summary>
    /// Uses linear interpolation to return a value between start and end
    /// </summary>
    /// <param name="start">Starting value</param>
    /// <param name="end">End value</param>
    /// <param name="duration">Total animation time (in seconds)</param>
    /// <param name="timeElapsed">Time since animation started (in seconds)</param>
    /// <returns></returns>
    public static float LinearTween(float start, float end, float duration, float timeElapsed)
    {
        if (timeElapsed > duration) return end;
        else if (timeElapsed < 0) return start;
        // Use linear calculation to figure out the starting time
        float percent = (float)timeElapsed/ (float)duration;
        return ((float)end * (float)percent);
    }
}

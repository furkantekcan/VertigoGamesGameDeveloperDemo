using System;
using System.Collections.Generic;

public class WheelSpinResolver : IWheelSpinResolver
{
    private readonly IRandomProvider _randomProvider;

    public WheelSpinResolver(IRandomProvider randomProvider)
    {
        _randomProvider = randomProvider;
    }

    public SpinResult Resolve(List<WheelSliceData> slices)
    {
        if (slices == null || slices.Count == 0)
        {
            throw new ArgumentException("Slice list is empty, cannot resolve spin.");
        }

        int index = _randomProvider.Range(0, slices.Count);
        return new SpinResult(index, slices[index]);
    }
}
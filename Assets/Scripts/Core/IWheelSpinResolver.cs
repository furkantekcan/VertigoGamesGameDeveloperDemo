using System.Collections.Generic;

public interface IWheelSpinResolver
{
    SpinResult Resolve(List<WheelSliceData> slices);
}
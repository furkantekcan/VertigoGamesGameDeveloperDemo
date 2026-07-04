public readonly struct SpinResult
{
    public readonly int SliceIndex;
    public readonly WheelSliceData Slice;

    public SpinResult(int sliceIndex, WheelSliceData slice)
    {
        SliceIndex = sliceIndex;
        Slice = slice;
    }

    public bool IsBomb => Slice.sliceType == SliceType.Bomb;
}
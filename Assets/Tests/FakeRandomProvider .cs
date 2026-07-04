public class FakeRandomProvider : IRandomProvider
{
    private readonly int _fixedIndex;

    public FakeRandomProvider(int fixedIndex)
    {
        _fixedIndex = fixedIndex;
    }

    public int Range(int minInclusive, int maxExclusive)
    {
        return _fixedIndex;
    }
}
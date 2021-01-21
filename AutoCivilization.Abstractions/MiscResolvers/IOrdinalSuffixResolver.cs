namespace AutoCivilization.Abstractions
{
    public interface IOrdinalSuffixResolver
    {
        string GetOrdinalSuffix(int num);
        string GetOrdinalSuffixWithInput(int num);
    }
}

namespace ClinicalTools.SimEncounters
{
    public interface IParser<T>
    {
        T Parse(string text);
    }
}
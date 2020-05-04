namespace ClinicalTools.SimEncounters
{
    public interface ICharEnumeratorParser<T>
    {
        T Parse(CharEnumerator enumerator);
    }
}
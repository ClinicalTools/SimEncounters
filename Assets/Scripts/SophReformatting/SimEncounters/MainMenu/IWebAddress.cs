namespace ClinicalTools.SimEncounters
{
    public interface IWebAddress
    {
        void AddArgument(string varName, string varValue);
        string GetUrl(string page);
    }
}
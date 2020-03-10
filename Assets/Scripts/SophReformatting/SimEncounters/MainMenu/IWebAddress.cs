namespace ClinicalTools.SimEncounters
{
    public interface IWebAddress
    {
        void AddArgument(string varName, string varValue);
        void ClearArguments(); 
        string GetUrl(string page);
    }
}
namespace ClinicalTools.SimEncounters
{
    public class WebAddress : IWebAddress
    {
        protected virtual string ServerAddress { get; } = @"http://localhost/SimEncounters/";
        //protected virtual string ServerAddress { get; } = @"https://takecontrolgame.com/docs/games/CECreator/PHP/";

        public WebAddress() { }

        protected string Arguments { get; set; } = "";
        public void AddArgument(string varName, string varValue) => Arguments += UrlArgument(varName, varValue);
        public void ClearArguments() => Arguments = "";
        protected string UrlArgument(string varName, string varValue) => $"&{varName}={varValue}";

        public virtual string GetUrl(string page) => $"{ServerAddress}{page}?{Arguments}";
    }
}
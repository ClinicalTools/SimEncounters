using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IServerReader
    {
        WaitableResult<string> Begin(UnityWebRequest webRequest);
    }
}
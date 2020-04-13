using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IServerReader
    {
        WaitableResult<ServerResult> Begin(UnityWebRequest webRequest);
    }
}
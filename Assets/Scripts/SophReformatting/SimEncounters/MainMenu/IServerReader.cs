using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IServerReader
    {
        WaitableResult<ServerResult2> Begin(UnityWebRequest webRequest);
    }
}
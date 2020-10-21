using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ClosePopup : MonoBehaviour, ICloseHandler
    {
        public void Close(object sender) => gameObject.SetActive(false);
    }
}

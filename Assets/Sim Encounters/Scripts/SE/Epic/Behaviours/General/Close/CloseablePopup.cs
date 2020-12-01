using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class CloseablePopup : MonoBehaviour, ICloseHandler
    {
        public virtual void Close(object sender) => gameObject.SetActive(false);
    }
}
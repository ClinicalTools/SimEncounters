using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class ReaderPopup
    {
        public ReaderPopup(ReaderScene reader, PopupUI popupUI)
        {
            foreach (var closeButton in popupUI.CloseButtons)
                closeButton.onClick.AddListener(() => Object.Destroy(popupUI.gameObject));
        }
    }
}
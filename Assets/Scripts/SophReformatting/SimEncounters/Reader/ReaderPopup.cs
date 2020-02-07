using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class ReaderPopup
    {
        public ReaderPopup(EncounterReader reader, ReaderPopupUI popupUI)
        {
            foreach (var closeButton in popupUI.CloseButtons)
                closeButton.onClick.AddListener(() => Object.Destroy(popupUI.gameObject));
        }
    }
}
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderImagePopup : ReaderPopup
    {
        public ReaderImagePopup(ReaderScene reader, ReaderImagePopupUI popupUI, Sprite sprite) : base(reader, popupUI)
        {
            popupUI.Image.sprite = sprite;
            Canvas.ForceUpdateCanvases();
        }
    }
}
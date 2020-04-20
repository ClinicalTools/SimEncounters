using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderImagePopup : ReaderPopup
    {
        public ReaderImagePopup(ReaderScene reader, ReaderImagePopupUI popupUI, Sprite sprite) : base(reader, null)
        {
            popupUI.Image.sprite = sprite;
            Canvas.ForceUpdateCanvases();
        }
    }
}
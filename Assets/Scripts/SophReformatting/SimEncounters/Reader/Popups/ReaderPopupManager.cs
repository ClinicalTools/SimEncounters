using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPopupManager
    {
        protected ReaderScene Reader { get; }
        protected ReaderPopupsUI PopupsUI { get; }
        public ReaderPopupManager(ReaderScene reader, ReaderPopupsUI popupsUI)
        {
            Reader = reader;
            PopupsUI = popupsUI;
        }

        public virtual T Open<T>(T popup) where T : MonoBehaviour
            => Object.Instantiate(popup, PopupsUI.PopupsParent);


        public virtual ReaderImagePopup ShowImage(Sprite image)
        {
            var popupUI = Open(PopupsUI.ImagePopupPrefab);
            return new ReaderImagePopup(Reader, popupUI, image);
        }
    }
}
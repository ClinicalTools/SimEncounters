using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderImagePopupUI : PopupUI
    {
        [SerializeField] private Image image;
        public Image Image { get => image; set => image = value; }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderImagePopupUI : SpriteDrawer
    {
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }

        [SerializeField] private Image image;
        public Image Image { get => image; set => image = value; }

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public override void Display(Sprite sprite)
        {
            Image.sprite = sprite;
        }
    }
}
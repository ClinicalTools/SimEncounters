using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ToggleViewButtonUI : MonoBehaviour
    {
        public event Action Selected;

        [SerializeField] private TextMeshProUGUI text;
        public TextMeshProUGUI Text { get => text; set => text = value; }

        [SerializeField] private Image image;
        public Image Image { get => image; set => image = value; }

        [SerializeField] private Button button;
        public Button Button { get => button; set => button = value; }

        private void Awake()
        {
            Button.onClick.AddListener(() => Selected?.Invoke());
        }

        public void Show()
        {
            gameObject.SetActive(true);
            Button.interactable = true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            Button.interactable = false;
        }
    }
}
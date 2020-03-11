using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuCategorySelectorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI categoryLabel;
        public TextMeshProUGUI CategoryLabel { get => categoryLabel; set => categoryLabel = value; }

        [SerializeField] private Button selectButton;
        public Button SelectButton { get => selectButton; set => selectButton = value; }

        public event Action Selected;

        public void Display(string category)
        {
            CategoryLabel.text = category;
            SelectButton.onClick.AddListener(() => Selected?.Invoke());
        }
    }
}
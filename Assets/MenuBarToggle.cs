using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MenuBarToggle : MonoBehaviour
    {
        public Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;

        public TextMeshProUGUI Label { get => label; set => label = value; }
        [SerializeField] private TextMeshProUGUI label;

        public Image Icon { get => icon; set => icon = value; }
        [SerializeField] private Image icon;
    }
}
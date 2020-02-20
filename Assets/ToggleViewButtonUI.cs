using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ToggleViewButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        public TextMeshProUGUI Text { get => text; set => text = value; }

        [SerializeField] private Image image;
        public Image Image { get => image; set => image = value; }
    }
}
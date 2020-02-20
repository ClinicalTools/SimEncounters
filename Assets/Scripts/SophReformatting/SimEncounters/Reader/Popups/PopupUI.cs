using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public abstract class PopupUI : MonoBehaviour
    {
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
    }
}
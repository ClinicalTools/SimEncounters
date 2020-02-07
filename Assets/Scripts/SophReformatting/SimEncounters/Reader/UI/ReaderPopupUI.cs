using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class ReaderPopupUI : MonoBehaviour
    {
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
    }
}
using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Reader;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.ClinicalEncounters.Reader
{
    public class WebGLCompletionPopup : BaseCompletionPopup
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();

        public override event Action ExitScene;

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
        }
        public override void Display(Encounter encounter) => gameObject.SetActive(true);
        protected virtual void Hide() => gameObject.SetActive(false);
    }
}
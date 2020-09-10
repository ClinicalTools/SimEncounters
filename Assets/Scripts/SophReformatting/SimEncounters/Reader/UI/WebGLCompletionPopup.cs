using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class WebGLCompletionPopup : BaseCompletionPopup
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();

        public override event Action ExitScene { add { } remove { } }

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
        }
        public override void CompletionDraw(Encounter encounter) => gameObject.SetActive(true);
        protected virtual void Hide() => gameObject.SetActive(false);
    }
}
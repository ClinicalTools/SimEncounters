using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterSelectorReaderButtons : BaseEncounterSelectorButtons
    {
        public override event Action Start;
        public override event Action Copy;

        public virtual Button ReadButton { get => readButton; set => readButton = value; }
        [SerializeField] private Button readButton;
        public virtual TextMeshProUGUI ReadText { get => readText; set => readText = value; }
        [SerializeField] private TextMeshProUGUI readText;

        protected virtual void Awake() => ReadButton.onClick.AddListener(() => Start?.Invoke()); 

        public override void Display(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            gameObject.SetActive(true);

            ReadButton.gameObject.SetActive(true);
            ReadText.text = GetReadButtonText(menuEncounter.Status);
        }

        public override void Hide() => gameObject.SetActive(false);

        protected virtual string GetReadButtonText(EncounterBasicStatus basicStatus)
        {
            if (basicStatus == null)
                return "Start Case";
            else if (basicStatus.Completed)
                return "Review Case";
            else
                return "Continue Case";
        }
    }
}
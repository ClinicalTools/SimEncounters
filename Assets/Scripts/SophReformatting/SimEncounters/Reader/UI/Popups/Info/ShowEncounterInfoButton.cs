﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public interface ICloseHandler {
        void Close(object sender);
    }

    public class ShowEncounterInfoButton : MonoBehaviour
    {
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;

        protected UserEncounter CurrentUserEncounter { get; set; }
        protected BaseReaderEncounterInfoPopup EncounterInfoPopup { get; set; }
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<UserEncounterSelectedEventArgs> userEncounterSelector, 
            BaseReaderEncounterInfoPopup encounterInfoPopup)
        {
            EncounterInfoPopup = encounterInfoPopup;
            UserEncounterSelector = userEncounterSelector;
            UserEncounterSelector.AddSelectedListener(OnUserEncounterSelected);
        }

        protected virtual void OnUserEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs) 
            => CurrentUserEncounter = eventArgs.Encounter;

        protected virtual void OnDestroy() => UserEncounterSelector?.RemoveSelectedListener(OnUserEncounterSelected);
        protected virtual void Awake() => Button.onClick.AddListener(ShowEncounterInfo);
        public virtual void ShowEncounterInfo() => EncounterInfoPopup.ShowEncounterInfo(CurrentUserEncounter);
    }
}
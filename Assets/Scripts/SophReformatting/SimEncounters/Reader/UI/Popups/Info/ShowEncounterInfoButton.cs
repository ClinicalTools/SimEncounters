using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ShowEncounterInfoButton : MonoBehaviour, IUserEncounterDrawer
    {
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;

        protected BaseReaderEncounterInfoPopup EncounterInfoPopup { get; set; }
        [Inject]
        public void Inject(BaseReaderEncounterInfoPopup encounterInfoPopup)
            => EncounterInfoPopup = encounterInfoPopup;

        protected virtual void Awake() => Button.onClick.AddListener(ShowEncounterInfo);

        protected UserEncounter CurrentUserEncounter { get; set; }
        public virtual void Display(UserEncounter userEncounter) => CurrentUserEncounter = userEncounter;

        public virtual void ShowEncounterInfo() => EncounterInfoPopup.ShowEncounterInfo(CurrentUserEncounter);
    }
}
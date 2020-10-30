using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterButtonsUI : MonoBehaviour
    {
        public BaseEncounterSelectorButtons ReadButtons { get => readButtons; set => readButtons = value; }
        [SerializeField] private BaseEncounterSelectorButtons readButtons;
        public BaseEncounterSelectorButtons WriteButtons { get => writeButtons; set => writeButtons = value; }
        [SerializeField] private BaseEncounterSelectorButtons writeButtons;
        public BaseEncounterSelectorButtons TemplateButtons { get => templateButtons; set => templateButtons = value; }
        [SerializeField] private BaseEncounterSelectorButtons templateButtons;

        protected virtual ISelectedListener<MenuEncounterSelectedEventArgs> MenuEncounterSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<MenuEncounterSelectedEventArgs> menuEncounterSelectedListener)
        {
            MenuEncounterSelectedListener = menuEncounterSelectedListener;
            MenuEncounterSelectedListener.AddSelectedListener(MenuEncounterSelected);
        }

        protected virtual void MenuEncounterSelected(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {

        }
        public virtual void DisplayForRead(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            //if (ReadButtons)
                //ReadButtons.Display(sceneInfo, menuEncounter);
            if (WriteButtons)
                WriteButtons.Hide();
            if (TemplateButtons)
                TemplateButtons.Hide();
        }

        public virtual void DisplayForEdit(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (ReadButtons)
                ReadButtons.Hide();

            var metadata = menuEncounter.GetLatestMetadata();
            BaseEncounterSelectorButtons displayButtons;
            BaseEncounterSelectorButtons hideButtons;
            if (metadata.IsTemplate) {
                displayButtons = TemplateButtons;
                hideButtons = WriteButtons;
            } else {
                displayButtons = WriteButtons;
                hideButtons = TemplateButtons;
            }

            if (hideButtons)
                hideButtons.Hide();
            //if (displayButtons)
                //displayButtons.Display(sceneInfo, menuEncounter);
        }
    }
}
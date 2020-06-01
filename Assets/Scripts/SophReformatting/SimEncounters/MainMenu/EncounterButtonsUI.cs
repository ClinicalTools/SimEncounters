using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Writer;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public abstract class BaseEncounterSelectorButtons : MonoBehaviour
    {
        public abstract void Display(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter);
        public abstract void Hide();
    }

    public interface IEncounterCopier
    {
        void CopyEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter);
    }
    public class EncounterCopier : IEncounterCopier
    {
        public virtual void CopyEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {

        }
    }

    public class EncounterButtonsUI : MonoBehaviour
    {
        public BaseEncounterSelectorButtons ReadButtons { get => readButtons; set => readButtons = value; }
        [SerializeField] private BaseEncounterSelectorButtons readButtons;
        public BaseEncounterSelectorButtons WriteButtons { get => writeButtons; set => writeButtons = value; }
        [SerializeField] private BaseEncounterSelectorButtons writeButtons;
        public BaseEncounterSelectorButtons TemplateButtons { get => templateButtons; set => templateButtons = value; }
        [SerializeField] private BaseEncounterSelectorButtons templateButtons;

        public virtual void DisplayForRead(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (ReadButtons)
                ReadButtons.Display(sceneInfo, menuEncounter);
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
            if (displayButtons)
                displayButtons.Display(sceneInfo, menuEncounter);
        }
    }
}
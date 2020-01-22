using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterUI : SceneUI
    {
        [SerializeField] private Button variablesButton;
        public virtual Button VariablesButton { get => variablesButton; set => variablesButton = value; }

        [SerializeField] private Button saveAndViewButton;
        public virtual Button SaveAndViewButton { get => saveAndViewButton; set => saveAndViewButton = value; }

        [SerializeField] private Button saveButton;
        public virtual Button SaveButton { get => saveButton; set => saveButton = value; }

        [SerializeField] private Button mainMenuButton;
        public virtual Button MainMenuButton { get => mainMenuButton; set => mainMenuButton = value; }

        [SerializeField] private Button exitButton;
        public virtual Button ExitButton { get => exitButton; set => exitButton = value; }

        [SerializeField] private Button helpButton;
        public virtual Button HelpButton { get => helpButton; set => helpButton = value; }

        [SerializeField] private SectionsUI sections;
        public virtual SectionsUI Sections { get => sections; set => sections = value; }
    }
}
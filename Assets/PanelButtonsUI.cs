using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class PanelButtonsUI : MonoBehaviour
    {
        [SerializeField] private Toggle yourCasesToggle;
        public Toggle YourCasesToggle { get => yourCasesToggle; set => yourCasesToggle = value; }

        [SerializeField] private Toggle templateCasesToggle;
        public Toggle TemplateCasesToggle { get => templateCasesToggle; set => templateCasesToggle = value; }

        [SerializeField] private Toggle allCasesToggle;
        public Toggle AllCasesToggle { get => allCasesToggle; set => allCasesToggle = value; }
    }
}
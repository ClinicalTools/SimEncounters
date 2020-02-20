using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncountersViewUI : MonoBehaviour
    {
        public GameObject GameObject => gameObject;
        
        [SerializeField] private string viewName;
        public string ViewName { get => viewName; set => viewName = value; }

        [SerializeField] private Sprite viewSprite;
        public Sprite ViewSprite { get => viewSprite; set => viewSprite = value; }

        [SerializeField] private Button newCaseButton;
        public Button NewCaseButton { get => newCaseButton; set => newCaseButton = value; }

        [SerializeField] private Transform optionsParent;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }

        [SerializeField] private MainMenuEncounterUI optionPrefab;
        public MainMenuEncounterUI OptionPrefab { get => optionPrefab; set => optionPrefab = value; }
    }
}
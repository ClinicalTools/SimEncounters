using ClinicalTools.SimEncounters.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class AddEncounterPopup : MonoBehaviour
    {
        public TMP_InputField FirstNameField { get => firstNameField; set => firstNameField = value; }
        [SerializeField] private TMP_InputField firstNameField;
        public TMP_InputField LastNameField { get => lastNameField; set => lastNameField = value; }
        [SerializeField] private TMP_InputField lastNameField;
        public TMP_InputField DescriptionField { get => descriptionField; set => descriptionField = value; }
        [SerializeField] private TMP_InputField descriptionField;

        public virtual Button StartCaseButton { get => startCaseButton; set => startCaseButton = value; }
        [SerializeField] private Button startCaseButton;
        public virtual Button CloseButton { get => closeButton; set => closeButton = value; }
        [SerializeField] private Button closeButton;

        protected virtual void Awake()
        {
            StartCaseButton.onClick.AddListener(StartCase);
            CloseButton.onClick.AddListener(Close);
        }

        protected MenuSceneInfo SceneInfo { get; set; }
        protected EncounterMetadata CurrentMetadata { get; set; }
        public virtual void Display(MenuSceneInfo sceneInfo, MenuEncounter encounter)
        {
            SceneInfo = sceneInfo;
            CurrentMetadata = new EncounterMetadata(encounter.GetLatestMetadata());
            SetFields();
        }

        public virtual void Display(MenuSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
            CurrentMetadata = new EncounterMetadata();
            SetFields();
        }

        protected virtual void SetFields()
        {
            gameObject.SetActive(true);
            FirstNameField.text = "";
            LastNameField.text = "";
            DescriptionField.text = "";
        }
        
        protected virtual void StartCase()
        {
            CurrentMetadata.AuthorAccountId = SceneInfo.User.AccountId;
            CurrentMetadata.Title = $"{FirstNameField.text} {LastNameField.text}";
            CurrentMetadata.Description = DescriptionField.text;
        }

        protected virtual void Close()
        {
            gameObject.SetActive(false);
        }

    }
}
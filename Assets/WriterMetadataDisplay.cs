using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterMetadataDisplay : MonoBehaviour
    {
        public abstract void Display(User user, Encounter encounter);
    }
    public class WriterMetadataDisplay : BaseWriterMetadataDisplay
    {
        public TextMeshProUGUI Title { get => title; set => title = value; }
        [SerializeField] private TextMeshProUGUI title;
        public TMP_InputField Summary { get => summary; set => summary = value; }
        [SerializeField] private TMP_InputField summary;
        public TMP_InputField Description { get => description; set => description = value; }
        [SerializeField] private TMP_InputField description;
        public TMP_InputField Tags { get => tags; set => tags = value; }
        [SerializeField] private TMP_InputField tags;
        public Toggle PrivateToggle { get => privateToggle; set => privateToggle = value; }
        [SerializeField] private Toggle privateToggle;
        public Toggle TemplateToggle { get => templateToggle; set => templateToggle = value; }
        [SerializeField] private Toggle templateToggle;
        public BaseValueField Audience { get => audience; set => audience = value; }
        [SerializeField] private BaseValueField audience;
        public TMP_Dropdown Difficulty { get => difficulty; set => difficulty = value; }
        [SerializeField] private TMP_Dropdown difficulty;
        public Button SaveButton { get => saveButton; set => saveButton = value; }
        [SerializeField] private Button saveButton;

        protected IEncounterWriter EncounterWriter { get; set; }
        [Inject] public void Inject(IEncounterWriter encounterWriter) => EncounterWriter = encounterWriter;

        protected virtual void Awake() => SaveButton.onClick.AddListener(Save);

        protected Encounter CurrentEncounter { get; set; }
        protected User CurrentUser { get; set; }
        public override void Display(User user, Encounter encounter)
        {
            CurrentUser = user;
            gameObject.SetActive(true);

            CurrentEncounter = encounter;
            var metadata = encounter.Metadata;
            Title.text = metadata.Title;
            Summary.text = metadata.Subtitle;
            Description.text = metadata.Description;
            Tags.text = string.Join("; ", metadata.Categories);
            Audience.Initialize(metadata.Audience);
            Difficulty.value = (int)metadata.Difficulty;
            PrivateToggle.isOn = !metadata.IsPublic;
            TemplateToggle.isOn = !metadata.IsTemplate;
        }

        private readonly string[] tagsSplit = new string[] { "; " };
        protected virtual void Serialize()
        {
            var metadata = CurrentEncounter.Metadata;
            //metadata.Title = Title.text;
            metadata.Subtitle = Summary.text;
            metadata.Description = Description.text;
            metadata.Categories.AddRange(Tags.text.Split(tagsSplit, StringSplitOptions.RemoveEmptyEntries));
            metadata.Audience = Audience.Value;
            metadata.Difficulty = (Difficulty)Difficulty.value;
            metadata.IsPublic = !PrivateToggle.isOn;
            metadata.IsTemplate = !TemplateToggle.isOn;
        }
        protected virtual void Save()
        {
            Serialize();
            EncounterWriter.Save(CurrentUser, CurrentEncounter);

            gameObject.SetActive(false);
        }
    }
}
using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Writer;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.ClinicalEncounters.Writer
{
    public class CEWriterMetadataDisplay : BaseWriterDisplay
    {
        public TextMeshProUGUI Title { get => title; set => title = value; }
        [SerializeField] private TextMeshProUGUI title;
        public TMP_InputField Summary { get => summary; set => summary = value; }
        [SerializeField] private TMP_InputField summary;
        public TMP_InputField Description { get => description; set => description = value; }
        [SerializeField] private TMP_InputField description;
        public TMP_InputField Tags { get => tags; set => tags = value; }
        [SerializeField] private TMP_InputField tags;
        public TMP_InputField URL { get => url; set => url = value; }
        [SerializeField] private TMP_InputField url;
        public TMP_InputField CompletionCode { get => completionCode; set => completionCode = value; }
        [SerializeField] private TMP_InputField completionCode;
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
        public Button PublishButton { get => publishButton; set => publishButton = value; }
        [SerializeField] private Button publishButton;

        protected IEncounterWriter LocalWriter { get; set; }
        protected IEncounterWriter ServerWriter { get; set; }
        [Inject]
        public void Inject(
            [Inject(Id = SaveType.Local)] IEncounterWriter localWriter,
            [Inject(Id = SaveType.Server)] IEncounterWriter serverWriter)
        {
            LocalWriter = localWriter;
            ServerWriter = serverWriter;
        }

        protected virtual void Awake()
        {
            SaveButton.onClick.AddListener(Save);
            PublishButton.onClick.AddListener(Publish);
        }
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
            TemplateToggle.isOn = metadata.IsTemplate;
            if (metadata is CEEncounterMetadata ceMetadata) {
                URL.text = ceMetadata.Url;
                CompletionCode.text = ceMetadata.CompletionCode;
            }
        }

        private readonly string[] tagsSplit = new string[] { "; " };
        protected virtual void Serialize()
        {
            var metadata = CurrentEncounter.Metadata;
            metadata.Subtitle = Summary.text;
            metadata.Description = Description.text;
            metadata.Categories.Clear();
            var categories = Tags.text.Split(tagsSplit, StringSplitOptions.RemoveEmptyEntries);
            foreach (var category in categories.Where((c) => !string.IsNullOrWhiteSpace(c)))
                metadata.Categories.Add(category);
            metadata.Audience = Audience.Value;
            metadata.Difficulty = (Difficulty)Difficulty.value;
            metadata.IsPublic = !PrivateToggle.isOn;
            metadata.IsTemplate = TemplateToggle.isOn;
            if (metadata is CEEncounterMetadata ceMetadata) {
                ceMetadata.Url = URL.text.Trim();
                ceMetadata.CompletionCode = CompletionCode.text.Trim();
            }
        }
        protected virtual void Save()
        {
            Serialize();
            LocalWriter.Save(CurrentUser, CurrentEncounter);

            gameObject.SetActive(false);
        }

        protected virtual void Publish()
        {
            Serialize();
            var savingResult = ServerWriter.Save(CurrentUser, CurrentEncounter);
            savingResult.AddOnCompletedListener(PublishingFinished);

            gameObject.SetActive(false);
        }

        protected virtual void PublishingFinished(WaitedResult result) => LocalWriter.Save(CurrentUser, CurrentEncounter);
    }
}
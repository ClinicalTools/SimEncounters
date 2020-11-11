using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuEncounterUI : MonoBehaviour,
        ISelector<MenuEncounterSelectedEventArgs>,
        ISelectedListener<EncounterMetadataSelectedEventArgs>
    {
        protected Selector<MenuEncounterSelectedEventArgs> EncounterSelector { get; } = new Selector<MenuEncounterSelectedEventArgs>();
        protected Selector<EncounterMetadataSelectedEventArgs> MetadataSelector { get; } = new Selector<EncounterMetadataSelectedEventArgs>();

        public virtual Button SelectButton { get => selectButton; set => selectButton = value; }
        [SerializeField] private Button selectButton;
        public virtual GameObject InProgressObject { get => inProgressObject; set => inProgressObject = value; }
        [SerializeField] private GameObject inProgressObject;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }
        [SerializeField] private GameObject completedObject;

        protected virtual BaseMenuEncounterOverview MenuEncounterOverview { get; set; }
        [Inject] public virtual void Inject(BaseMenuEncounterOverview menuEncounterOverview) => MenuEncounterOverview = menuEncounterOverview;

        MenuEncounterSelectedEventArgs ISelectedListener<MenuEncounterSelectedEventArgs>.CurrentValue => EncounterSelector.CurrentValue;
        EncounterMetadataSelectedEventArgs ISelectedListener<EncounterMetadataSelectedEventArgs>.CurrentValue => MetadataSelector.CurrentValue;


        protected virtual void Start() => SelectButton.onClick.AddListener(OnSelected);
        protected virtual void OnSelected() => MenuEncounterOverview.Select(this, EncounterSelector.CurrentValue);

        public void AddSelectedListener(SelectedHandler<MenuEncounterSelectedEventArgs> handler)
            => EncounterSelector.AddSelectedListener(handler);
        public void AddSelectedListener(SelectedHandler<EncounterMetadataSelectedEventArgs> handler)
            => MetadataSelector.AddSelectedListener(handler);

        public void RemoveSelectedListener(SelectedHandler<EncounterMetadataSelectedEventArgs> handler)
            => MetadataSelector.RemoveSelectedListener(handler);
        public void RemoveSelectedListener(SelectedHandler<MenuEncounterSelectedEventArgs> handler)
            => EncounterSelector.RemoveSelectedListener(handler);

        public void Select(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            EncounterSelector.Select(sender, eventArgs);
            MetadataSelector.Select(sender, new EncounterMetadataSelectedEventArgs(eventArgs.Encounter.GetLatestMetadata()));

            var status = eventArgs.Encounter.Status;
            CompletedObject.SetActive(status?.Completed == true);
            InProgressObject.SetActive(status?.Completed == false);
        }

        public class Pool : SceneMonoMemoryPool<MainMenuEncounterUI>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
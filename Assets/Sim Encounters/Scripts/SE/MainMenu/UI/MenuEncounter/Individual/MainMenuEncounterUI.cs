using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuEncounterUI : MenuEncounterSelector
    {
        public virtual Button SelectButton { get => selectButton; set => selectButton = value; }
        [SerializeField] private Button selectButton;
        public virtual GameObject InProgressObject { get => inProgressObject; set => inProgressObject = value; }
        [SerializeField] private GameObject inProgressObject;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }
        [SerializeField] private GameObject completedObject;

        protected virtual BaseMenuEncounterOverview MenuEncounterOverview { get; set; }
        [Inject] public virtual void Inject(BaseMenuEncounterOverview menuEncounterOverview) => MenuEncounterOverview = menuEncounterOverview;

        protected virtual void Start() => SelectButton.onClick.AddListener(OnSelected);
        protected virtual void OnSelected() => MenuEncounterOverview.Select(this, EncounterSelector.CurrentValue);

        public override void Select(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);

            var status = eventArgs.Encounter.Status;
            CompletedObject.SetActive(status?.Completed == true);
            InProgressObject.SetActive(status?.Completed == false);
        }
    }
    public abstract class MenuEncounterSelector : MonoBehaviour,
        ISelector<MenuEncounterSelectedEventArgs>,
        ISelectedListener<EncounterMetadataSelectedEventArgs>
    {
        protected virtual Selector<MenuEncounterSelectedEventArgs> EncounterSelector { get; } = new Selector<MenuEncounterSelectedEventArgs>();
        protected virtual Selector<EncounterMetadataSelectedEventArgs> MetadataSelector { get; } = new Selector<EncounterMetadataSelectedEventArgs>();

        MenuEncounterSelectedEventArgs ISelectedListener<MenuEncounterSelectedEventArgs>.CurrentValue => EncounterSelector.CurrentValue;
        EncounterMetadataSelectedEventArgs ISelectedListener<EncounterMetadataSelectedEventArgs>.CurrentValue => MetadataSelector.CurrentValue;

        public virtual void AddSelectedListener(SelectedHandler<MenuEncounterSelectedEventArgs> handler)
            => EncounterSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<EncounterMetadataSelectedEventArgs> handler)
            => MetadataSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<EncounterMetadataSelectedEventArgs> handler)
            => MetadataSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<MenuEncounterSelectedEventArgs> handler)
            => EncounterSelector.RemoveSelectedListener(handler);

        public virtual void Select(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            EncounterSelector.Select(sender, eventArgs);
            MetadataSelector.Select(sender, new EncounterMetadataSelectedEventArgs(eventArgs.Encounter.GetLatestMetadata()));
        }

        public class Pool : SceneMonoMemoryPool<MenuEncounterSelector>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
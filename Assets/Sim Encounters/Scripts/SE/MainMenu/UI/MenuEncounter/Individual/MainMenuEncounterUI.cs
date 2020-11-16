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
        protected virtual void OnSelected() => Select(this, CurrentValue);

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
        public MenuEncounterSelectedEventArgs CurrentValue { get; protected set; }
        protected EncounterMetadataSelectedEventArgs CurrentMetadataValue { get; set; }
        EncounterMetadataSelectedEventArgs ISelectedListener<EncounterMetadataSelectedEventArgs>.CurrentValue => CurrentMetadataValue;

        public event SelectedHandler<MenuEncounterSelectedEventArgs> Selected;
        public event SelectedHandler<EncounterMetadataSelectedEventArgs> MetadataSelected;

        event SelectedHandler<EncounterMetadataSelectedEventArgs> ISelectedListener<EncounterMetadataSelectedEventArgs>.Selected {
            add => MetadataSelected += value;
            remove => MetadataSelected -= value;
        }

        public virtual void Select(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            CurrentValue = eventArgs;
            Selected?.Invoke(sender, eventArgs);

            CurrentMetadataValue = new EncounterMetadataSelectedEventArgs(eventArgs.Encounter.GetLatestMetadata());
            MetadataSelected?.Invoke(sender, CurrentMetadataValue);
        }

        public class Pool : SceneMonoMemoryPool<MenuEncounterSelector>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
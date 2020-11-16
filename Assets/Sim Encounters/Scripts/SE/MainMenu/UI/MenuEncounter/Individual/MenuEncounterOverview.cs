using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseMenuEncounterOverview : MonoBehaviour
    {
        public abstract void Select(object sender, MenuEncounterSelectedEventArgs eventArgs);
        public abstract void Hide();
    }

    public class MenuEncounterOverview : BaseMenuEncounterOverview,
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

        public virtual EncounterButtonsUI EncounterButtons { get => encounterButtons; set => encounterButtons = value; }
        [SerializeField] private EncounterButtonsUI encounterButtons;
        public virtual DeleteDownloadHandler DeleteDownloadHandler { get => deleteDownloadHandler; set => deleteDownloadHandler = value; }
        [SerializeField] private DeleteDownloadHandler deleteDownloadHandler;


        public override void Select(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            gameObject.SetActive(true);

            CurrentValue = eventArgs;
            Selected?.Invoke(sender, eventArgs);
            CurrentMetadataValue = new EncounterMetadataSelectedEventArgs(eventArgs.Encounter.GetLatestMetadata());
            MetadataSelected?.Invoke(sender, CurrentMetadataValue);
        }

        public override void Hide() => gameObject.SetActive(false);
    }
}
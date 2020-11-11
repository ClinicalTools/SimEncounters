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
        protected Selector<MenuEncounterSelectedEventArgs> EncounterSelector { get; } = new Selector<MenuEncounterSelectedEventArgs>();
        protected Selector<EncounterMetadataSelectedEventArgs> MetadataSelector { get; } = new Selector<EncounterMetadataSelectedEventArgs>();

        public virtual EncounterButtonsUI EncounterButtons { get => encounterButtons; set => encounterButtons = value; }
        [SerializeField] private EncounterButtonsUI encounterButtons;
        public virtual DeleteDownloadHandler DeleteDownloadHandler { get => deleteDownloadHandler; set => deleteDownloadHandler = value; }
        [SerializeField] private DeleteDownloadHandler deleteDownloadHandler;

        MenuEncounterSelectedEventArgs ISelectedListener<MenuEncounterSelectedEventArgs>.CurrentValue => EncounterSelector.CurrentValue;
        EncounterMetadataSelectedEventArgs ISelectedListener<EncounterMetadataSelectedEventArgs>.CurrentValue => MetadataSelector.CurrentValue;


        public void AddSelectedListener(SelectedHandler<MenuEncounterSelectedEventArgs> handler)
            => EncounterSelector.AddSelectedListener(handler);
        public void AddSelectedListener(SelectedHandler<EncounterMetadataSelectedEventArgs> handler)
            => MetadataSelector.AddSelectedListener(handler);

        public void RemoveSelectedListener(SelectedHandler<EncounterMetadataSelectedEventArgs> handler)
            => MetadataSelector.RemoveSelectedListener(handler);
        public void RemoveSelectedListener(SelectedHandler<MenuEncounterSelectedEventArgs> handler)
            => EncounterSelector.RemoveSelectedListener(handler);

        public override void Select(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            gameObject.SetActive(true);
            EncounterSelector.Select(sender, eventArgs);
            MetadataSelector.Select(sender, new EncounterMetadataSelectedEventArgs(eventArgs.Encounter.GetLatestMetadata()));
        }

        public override void Hide() => gameObject.SetActive(false);
    }
}
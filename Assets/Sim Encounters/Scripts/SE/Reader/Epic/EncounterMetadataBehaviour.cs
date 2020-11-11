using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class EncounterMetadataBehaviour : MonoBehaviour
    {
        protected ISelectedListener<EncounterMetadataSelectedEventArgs> MetadataSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<EncounterMetadataSelectedEventArgs> metadataSelector)
        {
            MetadataSelector = metadataSelector;
        }

        protected virtual void Start() => MetadataSelector.AddSelectedListener(OnMetadataSelected);

        protected abstract void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs);

        protected virtual void OnDestroy() => MetadataSelector?.RemoveSelectedListener(OnMetadataSelected);
    }
}
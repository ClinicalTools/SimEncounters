using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class EncounterMetadataBehaviour : MonoBehaviour
    {
        protected ISelectedListener<EncounterMetadata> MetadataSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<EncounterMetadata> metadataSelector)
        {
            MetadataSelector = metadataSelector;
        }

        protected virtual void Start() => MetadataSelector.AddSelectedListener(OnMetadataSelected);

        protected abstract void OnMetadataSelected(object sender, EncounterMetadata metadata);

        protected virtual void OnDestroy() => MetadataSelector?.RemoveSelectedListener(OnMetadataSelected);
    }
}
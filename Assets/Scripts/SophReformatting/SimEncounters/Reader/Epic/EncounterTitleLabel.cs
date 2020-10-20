using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterTitleLabel : MonoBehaviour
    {
        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label
        {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        protected ISelectedListener<EncounterMetadata> MetadataSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<EncounterMetadata> metadataSelector)
        {
            MetadataSelector = metadataSelector;
            MetadataSelector.AddSelectedListener(OnMetadataSelected);
        }

        protected virtual void OnMetadataSelected(object sender, EncounterMetadata metadata) => Label.text = metadata.Title;

        protected virtual void OnDestroy() => MetadataSelector?.RemoveSelectedListener(OnMetadataSelected);
    }
}
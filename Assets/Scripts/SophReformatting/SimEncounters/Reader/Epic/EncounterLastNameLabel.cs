using ClinicalTools.ClinicalEncounters;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterLastNameLabel : EncounterMetadataLabel
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
        {
            if (eventArgs.Metadata is CEEncounterMetadata metadata)
                Label.text = metadata.Name.LastName;
        }
    }
}
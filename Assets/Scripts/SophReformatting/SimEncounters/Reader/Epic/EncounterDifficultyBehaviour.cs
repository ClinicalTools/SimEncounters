using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(DifficultyUI))]
    public class EncounterDifficultyBehaviour : EncounterMetadataBehaviour
    {
        private DifficultyUI difficultyUI;
        protected DifficultyUI DifficultyUI
        {
            get {
                if (difficultyUI == null)
                    difficultyUI = GetComponent<DifficultyUI>();
                return difficultyUI;
            }
        }
        protected override void OnMetadataSelected(object sender, EncounterMetadata metadata)
            => DifficultyUI.Display(metadata.Difficulty);
    }
}
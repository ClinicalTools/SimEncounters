using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class DifficultyDisplay
    {
        public DifficultyDisplay(DifficultyUI difficultyUI, Difficulty difficulty)
        {
            difficultyUI.Label.text = difficulty.ToString();
            difficultyUI.BeginnerImage.gameObject.SetActive(difficulty == Difficulty.Beginner);
            difficultyUI.IntermediateImage.gameObject.SetActive(difficulty == Difficulty.Intermediate);
            difficultyUI.AdvancedImage.gameObject.SetActive(difficulty == Difficulty.Advanced);

        }
    }
}
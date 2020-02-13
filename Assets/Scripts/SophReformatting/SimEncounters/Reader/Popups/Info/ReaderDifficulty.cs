using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDifficulty
    {
        public ReaderDifficulty(EncounterReader reader, ReaderDifficultyUI difficultyUI, Difficulty difficulty)
        {
            difficultyUI.Label.text = difficulty.ToString();
            difficultyUI.BeginnerImage.gameObject.SetActive(difficulty == Difficulty.Beginner);
            difficultyUI.IntermediateImage.gameObject.SetActive(difficulty == Difficulty.Intermediate);
            difficultyUI.AdvancedImage.gameObject.SetActive(difficulty == Difficulty.Advanced);

        }
    }
}
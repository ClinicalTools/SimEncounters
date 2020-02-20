using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class DifficultyDisplay
    {
        public DifficultyDisplay(DifficultyUI difficultyUI, Difficulty difficulty)
        {
            difficultyUI.Label.text = difficulty.ToString();
            Sprite sprite;
            if (difficulty == Difficulty.Beginner)
                sprite = difficultyUI.BeginnerSprite;
            else if (difficulty == Difficulty.Intermediate)
                sprite = difficultyUI.IntermediateSprite;
            else if (difficulty == Difficulty.Advanced)
                sprite = difficultyUI.AdvancedSprite;
            else
                sprite = null;
            difficultyUI.Image.sprite = sprite;
        }
    }
}
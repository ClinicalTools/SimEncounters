
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderInfoPopupUI : PopupUI
    {
        public virtual TextMeshProUGUI Title { get => title; set => title = value; }
        [SerializeField] private TextMeshProUGUI title;
        public virtual TextMeshProUGUI Subtitle { get => subtitle; set => subtitle = value; }
        [SerializeField] private TextMeshProUGUI subtitle;
        public virtual TextMeshProUGUI Description { get => description; set => description = value; }
        [SerializeField] private TextMeshProUGUI description;
        public virtual TextMeshProUGUI Categories { get => categories; set => categories = value; }
        [SerializeField] private TextMeshProUGUI categories;
        public virtual TextMeshProUGUI Audience { get => audience; set => audience = value; }
        [SerializeField] private TextMeshProUGUI audience;
        public virtual DifficultyUI Difficulty { get => difficulty; set => difficulty = value; }
        [SerializeField] private DifficultyUI difficulty;

        public void Display(EncounterMetadata metadata)
        {
            Title.text = metadata.Title;
            Subtitle.text = metadata.Subtitle;
            Description.text = metadata.Description;
            Categories.text = string.Join(", ", metadata.Categories);
            Audience.text = metadata.Audience;
            Difficulty.Display(metadata.Difficulty);
        }
    }
}
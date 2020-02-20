using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderInfoPopupUI : PopupUI
    {
        [SerializeField] private TextMeshProUGUI title;
        public virtual TextMeshProUGUI Title { get => title; set => title = value; }

        [SerializeField] private TextMeshProUGUI subtitle;
        public virtual TextMeshProUGUI Subtitle { get => subtitle; set => subtitle = value; }

        [SerializeField] private TextMeshProUGUI description;
        public virtual TextMeshProUGUI Description { get => description; set => description = value; }

        [SerializeField] private TextMeshProUGUI categories;
        public virtual TextMeshProUGUI Categories { get => categories; set => categories = value; }

        [SerializeField] private TextMeshProUGUI audience;
        public virtual TextMeshProUGUI Audience { get => audience; set => audience = value; }

        [SerializeField] private DifficultyUI difficulty;
        public virtual DifficultyUI Difficulty { get => difficulty; set => difficulty = value; }
    }
}
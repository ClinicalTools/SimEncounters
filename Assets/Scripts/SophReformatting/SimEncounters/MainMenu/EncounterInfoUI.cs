using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterInfoUI : MonoBehaviour
    {
        // honestly all of this should be grouped into case info ui
        [SerializeField] private TextMeshProUGUI titleLabel;
        public virtual TextMeshProUGUI TitleLabel { get => titleLabel; set => titleLabel = value; }

        [SerializeField] private TextMeshProUGUI subtitleLabel;
        public virtual TextMeshProUGUI SubtitleLabel { get => subtitleLabel; set => subtitleLabel = value; }

        [SerializeField] private TextMeshProUGUI tagsLabel;
        public virtual TextMeshProUGUI CategoriesLabel { get => tagsLabel; set => tagsLabel = value; }

        [SerializeField] private TextMeshProUGUI audienceLabel;
        public virtual TextMeshProUGUI AudienceLabel { get => audienceLabel; set => audienceLabel = value; }

        [SerializeField] private TextMeshProUGUI descriptionLabel;
        public virtual TextMeshProUGUI DescriptionLabel { get => descriptionLabel; set => descriptionLabel = value; }

        [SerializeField] private TextMeshProUGUI authorLabel;
        public virtual TextMeshProUGUI AuthorLabel { get => authorLabel; set => authorLabel = value; }

        [SerializeField] private TextMeshProUGUI dateModifiedLabel;
        public virtual TextMeshProUGUI DateModifiedLabel { get => dateModifiedLabel; set => dateModifiedLabel = value; }

        // change to difficulty UI
        [SerializeField] private DifficultyUI difficulty;
        public virtual DifficultyUI Difficulty { get => difficulty; set => difficulty = value; }
    }
}
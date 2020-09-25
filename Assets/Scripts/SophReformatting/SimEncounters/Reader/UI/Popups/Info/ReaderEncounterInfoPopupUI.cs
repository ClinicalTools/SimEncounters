using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderEncounterInfoPopupUI : BaseReaderEncounterInfoPopup
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public virtual Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
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

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
        }

        public override void ShowEncounterInfo(UserEncounter userEncounter)
        {
            gameObject.SetActive(true);
            var metadata = userEncounter.Data.Metadata;

            if (Image != null)
                Image.sprite = metadata.Sprite;
            Title.text = metadata.Title;
            Subtitle.text = metadata.Subtitle;
            Description.text = metadata.Description;
            Categories.text = string.Join(", ", metadata.Categories);
            Audience.text = metadata.Audience;
            Difficulty.Display(metadata.Difficulty);
        }
        protected virtual void Hide() => gameObject.SetActive(false);
    }
}
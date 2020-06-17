using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterInfoUI : MonoBehaviour
    {
        public virtual TextMeshProUGUI TitleLabel { get => titleLabel; set => titleLabel = value; }
        [SerializeField] private TextMeshProUGUI titleLabel;
        public virtual TextMeshProUGUI SubtitleLabel { get => subtitleLabel; set => subtitleLabel = value; }
        [SerializeField] private TextMeshProUGUI subtitleLabel;
        public virtual TextMeshProUGUI CategoriesLabel { get => tagsLabel; set => tagsLabel = value; }
        [SerializeField] private TextMeshProUGUI tagsLabel;
        public virtual TextMeshProUGUI AudienceLabel { get => audienceLabel; set => audienceLabel = value; }
        [SerializeField] private TextMeshProUGUI audienceLabel;
        public virtual TextMeshProUGUI DescriptionLabel { get => descriptionLabel; set => descriptionLabel = value; }
        [SerializeField] private TextMeshProUGUI descriptionLabel;
        public virtual TextMeshProUGUI AuthorLabel { get => authorLabel; set => authorLabel = value; }
        [SerializeField] private TextMeshProUGUI authorLabel;
        public virtual TextMeshProUGUI DateModifiedLabel { get => dateModifiedLabel; set => dateModifiedLabel = value; }
        [SerializeField] private TextMeshProUGUI dateModifiedLabel;
        public virtual DifficultyUI Difficulty { get => difficulty; set => difficulty = value; }
        [SerializeField] private DifficultyUI difficulty;
        public virtual RatingDisplayUI YourRating { get => yourRating; set => yourRating = value; }
        [SerializeField] private RatingDisplayUI yourRating;
        public virtual RatingDisplayUI AverageRating { get => averageRating; set => averageRating = value; }
        [SerializeField] private RatingDisplayUI averageRating;

        public void Display(EncounterMetadata encounterMetadata)
        {
            gameObject.SetActive(true);

            SetLabelText(AudienceLabel, encounterMetadata.Audience);
            SetLabelText(DescriptionLabel, encounterMetadata.Description);
            SetLabelText(SubtitleLabel, encounterMetadata.Subtitle);
            SetAuthor(AuthorLabel, encounterMetadata.AuthorName);
            SetTitle(TitleLabel, encounterMetadata.Title);
            SetDifficulty(Difficulty, encounterMetadata.Difficulty);
            SetDateModified(DateModifiedLabel, encounterMetadata.DateModified);
            SetCategories(CategoriesLabel, encounterMetadata.Categories);
        }

        protected virtual void SetDifficulty(DifficultyUI difficultyUI, Difficulty difficulty)
        {
            if (difficultyUI != null)
                new DifficultyDisplay(difficultyUI, difficulty);
        }
        protected virtual void SetTitle(TextMeshProUGUI label, string title)
        {
            if (label != null)
                label.text = title.Replace('_', ' ').Trim();
        }
        protected virtual void SetAuthor(TextMeshProUGUI label, string author)
        {
            if (label != null)
                label.text = $"by {author.Replace('_', ' ').Trim()}";
        }
        protected virtual void SetDateModified(TextMeshProUGUI label, long dateModified)
        {
            if (label == null)
                return;
            var time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            time = time.AddSeconds(dateModified);
            if (time > DateTime.UtcNow || time.Year < 2015)
            {
                Debug.LogError("Invalid time");
                label.text = "";
                return;
            }

            var timeString = time.ToLocalTime().ToString("MMMM d, yyyy");
            label.text = $"Last updated: {timeString}";
        }
        protected virtual string CategoryConcatenator => ", ";
        protected virtual void SetCategories(TextMeshProUGUI label, IEnumerable<string> categories)
        {
            if (label != null)
                label.text = string.Join(CategoryConcatenator, categories);
        }

        protected virtual void SetLabelText(TextMeshProUGUI label, string text)
        {
            if (label != null)
                label.text = text;
        }
    }
}
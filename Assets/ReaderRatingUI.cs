using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderRatingUI : MonoBehaviour
    {
        [SerializeField] private List<Toggle> ratingToggles;
        public List<Toggle> RatingToggles { get => ratingToggles; set => ratingToggles = value; }

        [SerializeField] private List<GameObject> stars;
        public List<GameObject> Stars { get => stars; set => stars = value; }

        [SerializeField] private Button submitRatingButton;
        public Button SubmitRatingButton { get => submitRatingButton; set => submitRatingButton = value; }

        public event Action<int> SubmitRating;

        protected virtual void Start()
        {
            for (int i = 0; i < RatingToggles.Count; i++)
            {
                var rating = i + 1;
                RatingToggles[i].onValueChanged.AddListener((isOn) => SetRating(rating, isOn));
            }
            SubmitRatingButton.onClick.AddListener(SubmitButtonPressed);
        }

        private int rating;
        public int Rating
        {
            get => rating;
            set
            {
                if (value >= 0 || RatingToggles.Count > value)
                    RatingToggles[value].isOn = true;
                SetRating(value);
            }
        }
        protected void SetRating(int rating, bool isOn = true)
        {
            if (!isOn)
                return;

            this.rating = rating;

            for (int i = 0; i < Stars.Count; i++)
                Stars[i].SetActive(i < rating);
        }

        protected void SubmitButtonPressed()
        {
            SubmitRating?.Invoke(Rating);
        }
    }
}
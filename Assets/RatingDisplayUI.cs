using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class RatingDisplayUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> stars;
        public List<GameObject> Stars { get => stars; set => stars = value; }

        public void SetRating(int rating)
        {
            for (int i = 0; i < Stars.Count; i++)
                Stars[i].SetActive(i < rating);
        }
    }
}
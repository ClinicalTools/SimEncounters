using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class RatingDisplayUI : MonoBehaviour
    {
        public List<GameObject> Stars { get => stars; set => stars = value; }
        [SerializeField] private List<GameObject> stars;

        public virtual void SetRating(int rating)
        {
            for (int i = 0; i < Stars.Count; i++)
                Stars[i].SetActive(i < rating);
        }
    }
}
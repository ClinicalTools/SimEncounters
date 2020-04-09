using UnityEngine;
using UnityEngine.UI;
using System;
using ClinicalTools.SimEncounters.Extensions;
using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class SortingOrderUI : MonoBehaviour
    {
        [SerializeField] private Toggle patientNameAscending;
        public Toggle PatientNameAscending { get => patientNameAscending; set => patientNameAscending = value; }

        [SerializeField] private Toggle patientNameDescending;
        public Toggle PatientNameDescending { get => patientNameDescending; set => patientNameDescending = value; }

        [SerializeField] private Toggle datePublishedAscending;
        public Toggle DatePublishedAscending { get => datePublishedAscending; set => datePublishedAscending = value; }

        [SerializeField] private Toggle datePublishedDescending;
        public Toggle DatePublishedDescending { get => datePublishedDescending; set => datePublishedDescending = value; }

        [SerializeField] private Toggle authorAscending;
        public Toggle AuthorAscending { get => authorAscending; set => authorAscending = value; }

        [SerializeField] private Toggle authorDescending;
        public Toggle AuthorDescending { get => authorDescending; set => authorDescending = value; }

        [SerializeField] private Toggle difficultyAscending;
        public Toggle DifficultyAscending { get => difficultyAscending; set => difficultyAscending = value; }

        [SerializeField] private Toggle difficultyDescending;
        public Toggle DifficultyDescending { get => difficultyDescending; set => difficultyDescending = value; }

        public Comparison<MenuEncounter> Comparison { get; protected set; }
        public event Action<Comparison<MenuEncounter>> SortingOrderChanged;

        public void Awake()
        {
            PatientNameAscending.isOn = true;
            Comparison = ComparePatientNameAscending;

            PatientNameAscending.AddOnSelectListener(() => SetComparer(ComparePatientNameAscending));
            PatientNameDescending.AddOnSelectListener(() => SetComparer(ComparePatientNameDescending));
            DatePublishedAscending.AddOnSelectListener(() => SetComparer(CompareDatePublishedAscending));
            DatePublishedDescending.AddOnSelectListener(() => SetComparer(CompareDatePublishedDescending));
            AuthorAscending.AddOnSelectListener(() => SetComparer(CompareAuthorAscending));
            AuthorDescending.AddOnSelectListener(() => SetComparer(CompareAuthorDescending));
            DifficultyAscending.AddOnSelectListener(() => SetComparer(CompareDifficultyAscending));
            DifficultyDescending.AddOnSelectListener(() => SetComparer(CompareDifficultyDescending));
        }

        protected void SetComparer(Comparison<MenuEncounter> comparison)
        {
            Comparison = comparison;
            SortingOrderChanged?.Invoke(comparison);
        }

        protected int ComparePatientNameAscending(MenuEncounter x, MenuEncounter y)
            => x.GetLatestMetadata().Title.CompareTo(y.GetLatestMetadata().Title);
        protected int ComparePatientNameDescending(MenuEncounter x, MenuEncounter y)
            => y.GetLatestMetadata().Title.CompareTo(x.GetLatestMetadata().Title);
        protected int CompareDatePublishedAscending(MenuEncounter x, MenuEncounter y)
            => x.GetLatestMetadata().DateModified.CompareTo(y.GetLatestMetadata().DateModified);
        protected int CompareDatePublishedDescending(MenuEncounter x, MenuEncounter y)
            => y.GetLatestMetadata().DateModified.CompareTo(x.GetLatestMetadata().DateModified);
        protected int CompareAuthorAscending(MenuEncounter x, MenuEncounter y)
            => x.GetLatestMetadata().AuthorName.CompareTo(y.GetLatestMetadata().AuthorName);
        protected int CompareAuthorDescending(MenuEncounter x, MenuEncounter y)
            => y.GetLatestMetadata().AuthorName.CompareTo(x.GetLatestMetadata().AuthorName);
        protected int CompareDifficultyAscending(MenuEncounter x, MenuEncounter y)
            => x.GetLatestMetadata().Difficulty.CompareTo(y.GetLatestMetadata().Difficulty);
        protected int CompareDifficultyDescending(MenuEncounter x, MenuEncounter y)
            => y.GetLatestMetadata().Difficulty.CompareTo(x.GetLatestMetadata().Difficulty);
    }
}
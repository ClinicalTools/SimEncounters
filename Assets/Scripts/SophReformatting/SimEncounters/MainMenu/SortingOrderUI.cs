using UnityEngine;
using UnityEngine.UI;
using System;
using ClinicalTools.SimEncounters.Extensions;

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

        public Comparison<EncounterDetail> Comparison { get; protected set; }
        public event Action<Comparison<EncounterDetail>> SortingOrderChanged;

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
        }

        protected void SetComparer(Comparison<EncounterDetail> comparison)
        {
            Comparison = comparison;
            SortingOrderChanged?.Invoke(comparison);
        }

        protected int ComparePatientNameAscending(EncounterDetail x, EncounterDetail y)
            => x.InfoGroup.GetLatestInfo().Title.CompareTo(y.InfoGroup.GetLatestInfo().Title);
        protected int ComparePatientNameDescending(EncounterDetail x, EncounterDetail y)
            => y.InfoGroup.GetLatestInfo().Title.CompareTo(x.InfoGroup.GetLatestInfo().Title);
        protected int CompareDatePublishedAscending(EncounterDetail x, EncounterDetail y)
            => x.InfoGroup.GetLatestInfo().DateModified.CompareTo(y.InfoGroup.GetLatestInfo().DateModified);
        protected int CompareDatePublishedDescending(EncounterDetail x, EncounterDetail y)
            => y.InfoGroup.GetLatestInfo().DateModified.CompareTo(x.InfoGroup.GetLatestInfo().DateModified);
        protected int CompareAuthorAscending(EncounterDetail x, EncounterDetail y)
            => x.InfoGroup.GetLatestInfo().AuthorName.CompareTo(y.InfoGroup.GetLatestInfo().AuthorName);
        protected int CompareAuthorDescending(EncounterDetail x, EncounterDetail y)
            => y.InfoGroup.GetLatestInfo().AuthorName.CompareTo(x.InfoGroup.GetLatestInfo().AuthorName);
    }
}
﻿using UnityEngine;
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

        [SerializeField] private Toggle difficultyAscending;
        public Toggle DifficultyAscending { get => difficultyAscending; set => difficultyAscending = value; }

        [SerializeField] private Toggle difficultyDescending;
        public Toggle DifficultyDescending { get => difficultyDescending; set => difficultyDescending = value; }

        public Comparison<EncounterInfo> Comparison { get; protected set; }
        public event Action<Comparison<EncounterInfo>> SortingOrderChanged;

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

        protected void SetComparer(Comparison<EncounterInfo> comparison)
        {
            Comparison = comparison;
            SortingOrderChanged?.Invoke(comparison);
        }

        protected int ComparePatientNameAscending(EncounterInfo x, EncounterInfo y)
            => x.MetaGroup.GetLatestInfo().Title.CompareTo(y.MetaGroup.GetLatestInfo().Title);
        protected int ComparePatientNameDescending(EncounterInfo x, EncounterInfo y)
            => y.MetaGroup.GetLatestInfo().Title.CompareTo(x.MetaGroup.GetLatestInfo().Title);
        protected int CompareDatePublishedAscending(EncounterInfo x, EncounterInfo y)
            => x.MetaGroup.GetLatestInfo().DateModified.CompareTo(y.MetaGroup.GetLatestInfo().DateModified);
        protected int CompareDatePublishedDescending(EncounterInfo x, EncounterInfo y)
            => y.MetaGroup.GetLatestInfo().DateModified.CompareTo(x.MetaGroup.GetLatestInfo().DateModified);
        protected int CompareAuthorAscending(EncounterInfo x, EncounterInfo y)
            => x.MetaGroup.AuthorName.CompareTo(y.MetaGroup.AuthorName);
        protected int CompareAuthorDescending(EncounterInfo x, EncounterInfo y)
            => y.MetaGroup.AuthorName.CompareTo(x.MetaGroup.AuthorName);
        protected int CompareDifficultyAscending(EncounterInfo x, EncounterInfo y)
            => x.MetaGroup.GetLatestInfo().Difficulty.CompareTo(y.MetaGroup.GetLatestInfo().Difficulty);
        protected int CompareDifficultyDescending(EncounterInfo x, EncounterInfo y)
            => y.MetaGroup.GetLatestInfo().Difficulty.CompareTo(x.MetaGroup.GetLatestInfo().Difficulty);
    }
}
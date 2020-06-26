using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class DeleteDownloadHandler : MonoBehaviour
    {
        public GameObject DeleteDownloadRow { get => deleteDownloadRow; set => deleteDownloadRow = value; }
        [SerializeField] private GameObject deleteOnlyRow;
        public GameObject DeleteOnlyRow { get => deleteOnlyRow; set => deleteOnlyRow = value; }
        [SerializeField] private GameObject deleteDownloadRow;
        public GameObject DownloadOnlyRow { get => downloadOnlyRow; set => downloadOnlyRow = value; }
        [SerializeField] private GameObject downloadOnlyRow;

        public List<Button> DownloadButtons { get => downloadButtons; set => downloadButtons = value; }
        [SerializeField] private List<Button> downloadButtons;
        public List<Button> DeleteButtons { get => deleteButtons; set => deleteButtons = value; }
        [SerializeField] private List<Button> deleteButtons;


        public event Action Delete;
        public event Action Download;

        protected virtual void Awake()
        {
            foreach (var deleteButton in DeleteButtons)
                deleteButton.onClick.AddListener(OnDelete);
            foreach (var downloadButton in DownloadButtons)
                downloadButton.onClick.AddListener(OnDownload);
        }

        public virtual void Display(MenuSceneInfo sceneInfo, MenuEncounter encounter)
        {
            var metadata = encounter.GetLatestMetadata();
            var onLocal = encounter.Metadata.ContainsKey(SaveType.Local);
            var isAuthor = metadata.AuthorAccountId == sceneInfo.User.AccountId;
            DeleteOnlyRow.SetActive(onLocal);
            DeleteDownloadRow.SetActive(!onLocal && isAuthor);
            DownloadOnlyRow.SetActive(!onLocal && !isAuthor);
        }

        protected virtual void OnDelete() => Delete?.Invoke();
        protected virtual void OnDownload() => Download?.Invoke();
    }
}
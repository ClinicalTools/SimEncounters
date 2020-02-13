using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPopupsUI : MonoBehaviour
    {
        [SerializeField] private Transform popupsParent;
        public Transform PopupsParent { get => popupsParent; set => popupsParent = value; }

        [SerializeField] private ReaderImagePopupUI imagePopupPrefab;
        public ReaderImagePopupUI ImagePopupPrefab { get => imagePopupPrefab; set => imagePopupPrefab = value; }
    }
}
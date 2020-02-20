using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class PageButtonsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pageLabel;
        public virtual TextMeshProUGUI PageLabel { get => pageLabel; set => pageLabel = value; }

        [SerializeField] private Button previousPageButton;
        public virtual Button PreviousPageButton { get => previousPageButton; set => previousPageButton = value; }

        [SerializeField] private Button nextPageButton;
        public virtual Button NextPageButton { get => nextPageButton; set => nextPageButton = value; }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFooterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pageInfoLabel;
        public virtual TextMeshProUGUI PageInfoLabel { get => pageInfoLabel; set => pageInfoLabel = value; }
        
        [SerializeField] private Button nextTabButton;
        public virtual Button NextTabButton { get => nextTabButton; set => nextTabButton = value; }

        [SerializeField] private Button nextSectionButton;
        public virtual Button NextSectionButton { get => nextSectionButton; set => nextSectionButton = value; }

        [SerializeField] private Button finishCaseButton;
        public virtual Button FinishCaseButton { get => finishCaseButton; set => finishCaseButton = value; }
    }
}
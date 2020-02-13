using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDifficultyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        public virtual TextMeshProUGUI Label { get => label; set => label = value; }

        [SerializeField] private Image beginnerImage;
        public virtual Image BeginnerImage { get => beginnerImage; set => beginnerImage = value; }

        [SerializeField] private Image intermediateImage;
        public virtual Image IntermediateImage { get => intermediateImage; set => intermediateImage = value; }

        [SerializeField] private Image advancedImage;
        public virtual Image AdvancedImage { get => advancedImage; set => advancedImage = value; }
    }
}
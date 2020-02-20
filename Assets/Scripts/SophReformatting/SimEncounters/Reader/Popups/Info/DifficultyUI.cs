using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class DifficultyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        public virtual TextMeshProUGUI Label { get => label; set => label = value; }

        [SerializeField] private Image image;
        public virtual Image Image { get => image; set => image = value; }

        [SerializeField] private Sprite beginnerSprite;
        public virtual Sprite BeginnerSprite { get => beginnerSprite; set => beginnerSprite = value; }

        [SerializeField] private Sprite intermediateSprite;
        public virtual Sprite IntermediateSprite { get => intermediateSprite; set => intermediateSprite = value; }

        [SerializeField] private Sprite advancedSprite;
        public virtual Sprite AdvancedSprite { get => advancedSprite; set => advancedSprite = value; }
    }
}
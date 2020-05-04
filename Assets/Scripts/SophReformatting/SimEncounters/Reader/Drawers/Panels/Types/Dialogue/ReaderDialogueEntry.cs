using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueEntry : BaseReaderPanel
    {
        public Image Border { get => border; set => border = value; }
        [SerializeField] private Image border;
        public List<GameObject> CharacterImages { get => characterImages; set => characterImages = value; }
        [SerializeField] private List<GameObject> characterImages;

        protected IReaderPanelDisplay PanelDisplay { get; set; }
        protected virtual IParser<Color> ColorParser { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay panelDisplay, IParser<Color> colorParser)
        {
            PanelDisplay = panelDisplay;
            ColorParser = colorParser;
        }

        private const string colorKey = "charColor";
        private const string characterKey = "characterName";
        public override void Display(UserPanel panel)
        {
            PanelDisplay.Display(panel, transform, transform);
            var data = panel.Data.Data;
            if (data.ContainsKey(colorKey))
                Border.color = ColorParser.Parse(data[colorKey]);
            if (data.ContainsKey(characterKey))
                SetCharacterImage(data[characterKey]);

            PanelDisplay.Display(panel, transform, transform);
        }

        protected virtual void SetCharacterImage(string characterName)
        {
            foreach (var characterImage in CharacterImages) {
                if (characterImage.name.ToLower().Contains(characterName.ToLower()))
                    characterImage.SetActive(true);
                else
                    characterImage.SetActive(false);
            }
        }
    }
}
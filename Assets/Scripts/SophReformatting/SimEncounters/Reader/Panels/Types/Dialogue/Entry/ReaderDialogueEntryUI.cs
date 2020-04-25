using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueEntryUI : BaseReaderPanelUI
    {
        [SerializeField] private Image border;
        public Image Border { get => border; set => border = value; }

        [SerializeField] private List<GameObject> characterImages;
        public List<GameObject> CharacterImages { get => characterImages; set => characterImages = value; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        protected virtual IParser<Color> ColorParser { get; set; }
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicPanelDrawer, IParser<Color> colorParser)
        {
            BasicPanelDrawer = basicPanelDrawer;
            ColorParser = colorParser;
        }

        private const string colorKey = "charColor";
        private const string characterKey = "characterName";
        public override void Display(UserPanel panel)
        {
            BasicPanelDrawer.Display(panel, transform, transform);
            var data = panel.Data.Data;
            if (data.ContainsKey(colorKey))
                Border.color = ColorParser.Parse(data[colorKey]);
            if (data.ContainsKey(characterKey))
                SetCharacterImage(data[characterKey]);

            BasicPanelDrawer.Display(panel, transform, transform);
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
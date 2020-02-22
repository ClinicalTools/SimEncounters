using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{

    public class ReaderDialogueEntryUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Image border;
        public Image Border { get => border; set => border = value; }

        [SerializeField] private List<GameObject> characterImages;
        public List<GameObject> CharacterImages { get => characterImages; set => characterImages = value; }

        private const string colorKey = "charColor";
        private const string characterKey = "characterName";
        protected virtual ColorConverter ColorConverter { get; } = new ColorConverter();
        public override void Initialize(ReaderScene reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);

            var data = keyedPanel.Value.Data;
            if (data.ContainsKey(colorKey))
                Border.color = ColorConverter.StringToColor(data[colorKey]);
            if (data.ContainsKey(characterKey))
                SetCharacterImage(data[characterKey]);

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            valueFieldInitializer.InitializePanelValueFields(gameObject, keyedPanel.Value);
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
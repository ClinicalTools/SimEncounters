using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueEntryDisplay : IReaderPanelDisplay
    {
        protected ReaderScene Reader { get; }
        protected ReaderDialogueEntryUI EntryUI { get; }
        protected virtual ColorParser ColorConverter { get; } = new ColorParser();
        public ReaderDialogueEntryDisplay(ReaderScene reader, ReaderDialogueEntryUI entryUI)
        {
            Reader = reader;
            EntryUI = entryUI;
        }

        private const string colorKey = "charColor";
        private const string characterKey = "characterName";
        public void Display(KeyValuePair<string, Panel> keyedPanel)
        {
            var data = keyedPanel.Value.Data;
            if (data.ContainsKey(colorKey))
                EntryUI.Border.color = ColorConverter.Parse(data[colorKey]);
            if (data.ContainsKey(characterKey))
                SetCharacterImage(data[characterKey]);

            Reader.ValueFieldInitializer.InitializePanelValueFields(EntryUI.gameObject, keyedPanel.Value);
        }

        protected virtual void SetCharacterImage(string characterName)
        {
            foreach (var characterImage in EntryUI.CharacterImages)
            {
                if (characterImage.name.ToLower().Contains(characterName.ToLower()))
                    characterImage.SetActive(true);
                else
                    characterImage.SetActive(false);
            }
        }
    }
}
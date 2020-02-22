using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueEntryDisplay : IReaderPanelDisplay
    {
        protected ReaderDialogueEntryUI EntryUI { get; }
        private const string colorKey = "charColor";
        private const string characterKey = "characterName";
        protected virtual ColorConverter ColorConverter { get; } = new ColorConverter();
        public ReaderDialogueEntryDisplay(ReaderScene reader, ReaderDialogueEntryUI entryUI, KeyValuePair<string, Panel> keyedPanel)
        {
            EntryUI = entryUI;

            var data = keyedPanel.Value.Data;
            if (data.ContainsKey(colorKey))
                EntryUI.Border.color = ColorConverter.StringToColor(data[colorKey]);
            if (data.ContainsKey(characterKey))
                SetCharacterImage(data[characterKey]);

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            valueFieldInitializer.InitializePanelValueFields(EntryUI.gameObject, keyedPanel.Value);
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
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabButton : EncounterToggle<KeyValuePair<string, Tab>>
    {
        protected virtual ReaderScene Reader { get; }
        public virtual ReaderTabToggleUI TabToggleUI { get; }
        protected virtual KeyValuePair<string, Tab> KeyedTab { get; }
        protected virtual bool IsRead { get; set; }
        public ReaderTabButton(ReaderScene reader, ReaderTabToggleUI tabToggleUI, KeyValuePair<string, Tab> keyedTab)
            : base(tabToggleUI.SelectToggle, keyedTab)
        {
            Reader = reader;
            TabToggleUI = tabToggleUI;
            KeyedTab = keyedTab;

            tabToggleUI.NameLabel.text = keyedTab.Value.Name;

            Selected += SetRead;

            CheckRead();
        }

        private void SetRead(KeyValuePair<string, Tab> keyedTab)
        {
            if (!IsRead) {
                IsRead = true;
                Reader.SceneInfo.Encounter.Status.ReadTabs.Add(keyedTab.Key);
                TabToggleUI.Visited.SetActive(true);
            }
        }

        public virtual void CheckRead()
        {
            IsRead = Reader.SceneInfo.Encounter.Status.ReadTabs.Contains(KeyedTab.Key);
            TabToggleUI.Visited.SetActive(IsRead);
        }
    }
}
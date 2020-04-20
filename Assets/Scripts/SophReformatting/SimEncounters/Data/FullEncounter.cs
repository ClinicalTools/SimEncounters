using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;

namespace ClinicalTools.SimEncounters.Data
{
    public class NewEncounterStatus
    {
        protected Dictionary<string, SectionStatus> Sections { get; } = new Dictionary<string, SectionStatus>();
        public virtual void AddSectionStatus(string key, SectionStatus status) => Sections.Add(key, status);
        public virtual SectionStatus GetSectionStatus(string key)
        {
            if (Sections.ContainsKey(key))
                return Sections[key];

            var sectionStatus = new SectionStatus();
            Sections.Add(key, sectionStatus);
            return sectionStatus;
        }
    }
    public class MiscEncounterStatusInfo
    {

    }
    public class SectionStatus
    {
        public bool Read { get; set; }
        protected Dictionary<string, TabStatus> Tabs { get; } = new Dictionary<string, TabStatus>();
        public virtual void AddTabStatus(string key, TabStatus status) => Tabs.Add(key, status);
        public virtual TabStatus GetTabStatus(string key)
        {
            if (Tabs.ContainsKey(key))
                return Tabs[key];

            var tabStatus = new TabStatus();
            Tabs.Add(key, tabStatus);
            return tabStatus;
        }
    }
    public class TabStatus
    {
        public bool Read { get; set; }
        protected Dictionary<string, PanelStatus> Panels { get; } = new Dictionary<string, PanelStatus>();
        public event Action StatusChanged;

        public virtual PanelStatus GetPanelStatus(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panelStatus = new PanelStatus();
            Panels.Add(key, panelStatus);
            return panelStatus;
        }
    }
    public class PanelStatus
    {
        public bool Read { get; set; }

        protected Dictionary<string, PanelStatus> Panels { get; } = new Dictionary<string, PanelStatus>();

        public virtual PanelStatus GetChildPanelStatus(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panelStatus = new PanelStatus();
            Panels.Add(key, panelStatus);
            return panelStatus;
        }
    }


    public class UserSection
    {
        public UserEncounter Encounter { get; }
        public Section Data { get; }
        protected SectionStatus Status { get; }
        public event Action StatusChanged;

        public UserSection(UserEncounter encounter, Section section, SectionStatus status)
        {
            Encounter = encounter;
            Data = section;
            Status = status;
        }

        protected virtual Dictionary<string, UserTab> Tabs { get; } = new Dictionary<string, UserTab>();
        public virtual List<UserTab> GetTabs()
        {
            var userPanels = new List<UserTab>();
            foreach (var tab in Data.Tabs)
                userPanels.Add(GetTab(tab.Key));

            return userPanels;
        }
        public virtual UserTab GetTab(string key)
        {
            if (Tabs.ContainsKey(key))
                return Tabs[key];

            var tab = Data.Tabs[key];
            var tabStatus = Status.GetTabStatus(key);
            var userTab = new UserTab(Encounter, tab, tabStatus);
            Tabs.Add(key, userTab);
            return userTab;
        }

        public bool IsRead() => Status.Read;
        public void SetRead(bool read)
        {
            if (Status.Read == read)
                return;
            Status.Read = read;
            StatusChanged?.Invoke();
        }
    }
    public class UserTab
    {
        public UserEncounter Encounter { get; }
        public Tab Data { get; }
        protected TabStatus Status { get; }
        public event Action StatusChanged;
        public UserTab(UserEncounter encounter, Tab tab, TabStatus status)
        {
            Encounter = encounter;
            Data = tab;
            Status = status;

        }

        public bool IsRead() => Status.Read;
        public void SetRead(bool read)
        {
            if (Status.Read == read)
                return;
            Status.Read = read;
            StatusChanged?.Invoke();
        }

        protected virtual Dictionary<string, UserPanel> Panels { get; } = new Dictionary<string, UserPanel>();
        public virtual List<UserPanel> GetPanels()
        {
            var userPanels = new List<UserPanel>();
            foreach (var panel in Data.Panels)
                userPanels.Add(GetPanel(panel.Key));

            return userPanels;
        }
        public virtual UserPanel GetPanel(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panel = Data.Panels[key];
            var panelStatus = Status.GetPanelStatus(key);
            var userPanel = new UserPanel(Encounter, panel, panelStatus);
            Panels.Add(key, userPanel);
            return userPanel;
        }
    }
    public class UserPanel
    {
        public UserEncounter Encounter { get; }
        public Panel Data { get; }
        protected PanelStatus Status { get; }
        public UserPinGroup PinGroup { get; }

        public UserPanel(UserEncounter encounter, Panel panel, PanelStatus status)
        {
            Encounter = encounter;
            Data = panel;
            Status = status;
            if (panel.Pins != null && panel.Pins.HasPin())
                PinGroup = new UserPinGroup(encounter, panel.Pins);
        }

        protected virtual Dictionary<string, UserPanel> Panels { get; } = new Dictionary<string, UserPanel>();
        public virtual List<UserPanel> GetChildPanels()
        {
            var userPanels = new List<UserPanel>();
            foreach (var panel in Data.ChildPanels)
                userPanels.Add(GetPanel(panel.Key));

            return userPanels;
        }
        public virtual UserPanel GetPanel(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panel = Data.ChildPanels[key];
            var panelStatus = Status.GetChildPanelStatus(key);
            var userPanel = new UserPanel(Encounter, panel, panelStatus);
            Panels.Add(key, userPanel);
            return userPanel;
        }
    }

    public class UserPinGroup
    {
        public UserEncounter Encounter { get; }
        public PinData Data { get; }

        public UserDialoguePin DialoguePin { get; }
        public UserQuizPin QuizPin { get; }
        public UserPinGroup(UserEncounter encounter, PinData pinGroup)
        {
            Encounter = encounter;
            Data = pinGroup;

            if (pinGroup.Dialogue != null)
                DialoguePin = new UserDialoguePin(encounter, pinGroup.Dialogue);
            if (pinGroup.Quiz != null)
                QuizPin = new UserQuizPin(encounter, pinGroup.Quiz);
        }
    }
    public class UserDialoguePin
    {
        public UserEncounter Encounter { get; }
        public DialoguePin Data { get; }
        public UserDialoguePin(UserEncounter encounter, DialoguePin dialogue)
        {
            Encounter = encounter;
            Data = dialogue;
        }
        protected virtual Dictionary<string, UserPanel> Panels { get; } = new Dictionary<string, UserPanel>();
        public virtual List<UserPanel> GetPanels()
        {
            var userPanels = new List<UserPanel>();
            foreach (var panel in Data.Conversation)
                userPanels.Add(GetPanel(panel.Key));

            return userPanels;
        }
        public virtual UserPanel GetPanel(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panel = Data.Conversation[key];
            var userPanel = new UserPanel(Encounter, panel, null);
            Panels.Add(key, userPanel);
            return userPanel;
        }
    }
    public class UserQuizPin
    {
        public UserEncounter Encounter { get; }
        public QuizPin Data { get; }
        public UserQuizPin(UserEncounter encounter, QuizPin quiz)
        {
            Encounter = encounter;
            Data = quiz;
        }
        protected virtual Dictionary<string, UserPanel> Panels { get; } = new Dictionary<string, UserPanel>();
        public virtual List<UserPanel> GetPanels()
        {
            var userPanels = new List<UserPanel>();
            foreach (var panel in Data.Questions)
                userPanels.Add(GetPanel(panel.Key));

            return userPanels;
        }
        public virtual UserPanel GetPanel(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panel = Data.Questions[key];
            var userPanel = new UserPanel(Encounter, panel, null);
            Panels.Add(key, userPanel);
            return userPanel;
        }
    }

    public class UserEncounter
    {
        public User User { get; }
        public EncounterMetadata Metadata { get; }
        public NewEncounterStatus Status { get; }
        public EncounterData Data { get; }

        public UserEncounter(User user, EncounterMetadata metadata, EncounterData data, NewEncounterStatus status)
        {
            User = user;
            Metadata = metadata;
            Data = data;
            Status = status;
        }

        protected virtual Dictionary<string, UserSection> Sections { get; } = new Dictionary<string, UserSection>();
        public virtual UserSection GetSection(string key)
        {
            if (Sections.ContainsKey(key))
                return Sections[key];

            var section = Data.Content.Sections[key];
            var sectionStatus = Status.GetSectionStatus(key);
            var userSection = new UserSection(this, section, sectionStatus);
            Sections.Add(key, userSection);
            return userSection;
        }
    }
    public class FullEncounter
    {
        public EncounterMetadata Metadata { get; }
        public EncounterDetailedStatus Status { get; }
        public EncounterData Data { get; }

        public FullEncounter(EncounterMetadata metadata, EncounterData data, EncounterDetailedStatus status)
        {
            Metadata = metadata;
            Data = data;
            Status = status;
        }
    }
}
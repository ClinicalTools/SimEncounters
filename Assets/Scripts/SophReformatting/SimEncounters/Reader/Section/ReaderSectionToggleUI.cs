using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionToggleUI : MonoBehaviour
    {
        [SerializeField] private EncounterToggleBehaviour selectToggle;
        public EncounterToggleBehaviour SelectToggle { get => selectToggle; set => selectToggle = value; }

        [SerializeField] private Image image;
        public Image Image { get => image; set => image = value; }

        [SerializeField] private Image icon;
        public Image Icon { get => icon; set => icon = value; }

        [SerializeField] private TextMeshProUGUI nameLabel;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }

        [SerializeField] private GameObject visited;
        public GameObject Visited { get => visited; set => visited = value; }

        public Action Selected;
        public Action Unselected;

        protected virtual void Awake()
        {
            SelectToggle.Selected += Selected;
            Selected += () => SetColor(KeyedSection.Value.Color, true);
            SelectToggle.Unselected += Unselected;
            Unselected += () => SetColor(KeyedSection.Value.Color, false);
        }

        protected EncounterSceneInfo SceneInfo { get; set; }
        protected KeyValuePair<string, Section> KeyedSection { get; set; }
        public void Display(EncounterSceneInfo sceneInfo, KeyValuePair<string, Section> keyedSection)
        {
            SceneInfo = sceneInfo;
            KeyedSection = keyedSection;

            var section = keyedSection.Value;
            SetColor(section.Color, false);
            var icons = sceneInfo.Encounter.Data.Images.Icons;
            if (icons.ContainsKey(section.IconKey))
                Icon.sprite = icons[section.IconKey];

            CheckRead();
        }
        public void Select() => SelectToggle.Select();

        protected virtual void CheckRead()
        {
            if (SceneInfo == null || KeyedSection.Value == null)
                return;

            var isRead = true;
            foreach (var tab in KeyedSection.Value.Tabs) {
                if (SceneInfo.Encounter.Status.ReadTabs.Contains(tab.Key))
                    continue;

                isRead = false;
                break;
            }
            Visited.SetActive(isRead);
        }

        protected virtual void SetColor(Color color, bool isOn)
        {
            if (isOn) {
                Image.color = color;
                Icon.color = Color.white;
                NameLabel.color = Color.white;
            } else {
                Image.color = Color.white;
                Icon.color = color;
                NameLabel.color = color;
            }
        }
    }
}

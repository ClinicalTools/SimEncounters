using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPopupPanelCreator : BaseWriterPanelCreator
    {
        public GameObject Popup { get => popup; set => popup = value; }
        [SerializeField] private GameObject popup;
        public List<Button> HideButtons { get => hideButtons; set => hideButtons = value; }
        [SerializeField] private List<Button> hideButtons;
        public Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;
        public Button ShowButton { get => showButton; set => showButton = value; }
        [SerializeField] private Button showButton;
        public TMP_Dropdown OptionsDropdown { get => optionsDropdown; set => optionsDropdown = value; }
        [SerializeField] private TMP_Dropdown optionsDropdown;

        public override event Action<BaseWriterPanel> AddPanel;

        protected virtual void Awake()
        {
            ShowButton.onClick.AddListener(Show);
            AddButton.onClick.AddListener(Add);
            foreach (var hideButton in HideButtons)
                hideButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        protected List<OptionWriterPanel> Options { get; set; }
        public override void Initialize(List<OptionWriterPanel> options)
        {
            OptionsDropdown.options.Clear();
            Options = options;
            foreach (var option in options)
                OptionsDropdown.options.Add(new TMP_Dropdown.OptionData(option.DisplayText));
        }

        protected virtual void Show()
        {
            if (Options.Count <= 0) {
                Debug.LogError("No panel options.");
                return;
            }
            if (Options.Count == 1) {
                var prefab = Options[0].PanelPrefab;
                AddPanel?.Invoke(prefab);
                return;
            }


            Popup.gameObject.SetActive(true);
        }

        protected virtual void Add()
        {
            var selectedOption = Options[OptionsDropdown.value];
            AddPanel?.Invoke(selectedOption.PanelPrefab);
        }
    }
}
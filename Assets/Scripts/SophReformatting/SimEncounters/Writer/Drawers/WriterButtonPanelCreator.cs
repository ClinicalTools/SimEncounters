using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterButtonPanelCreator : BaseWriterPanelCreator
    {
        public Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;

        public override event Action<BaseWriterPanel> AddPanel;

        protected virtual void Awake()
        {
            AddButton.onClick.AddListener(Add);
        }

        protected List<OptionWriterPanel> Options { get; set; }
        public override void Initialize(List<OptionWriterPanel> options)
        {
            Options = options;
            if (options.Count > 1)
                Debug.LogError("ButtonPanelCreator shouldn't be used when there are multiple options. Consider using PopupPanelCreator instead.");
        }

        protected virtual void Add()
        {
            var selectedOption = Options[0];
            AddPanel?.Invoke(selectedOption.PanelPrefab);
        }
    }
}
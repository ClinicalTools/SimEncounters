using ClinicalTools.SimEncounters.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionCreatorPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button CreateButton { get => createButton; set => createButton = value; }
        [SerializeField] private Button createButton;

        public TMP_InputField NameField { get => nameField; set => nameField = value; }
        [SerializeField] private TMP_InputField nameField;
        public BaseColorEditor Color { get => color; set => color = value; }
        [SerializeField] private BaseColorEditor color;
        public IconSelectorUI IconSelector { get => iconSelector; set => iconSelector = value; }
        [SerializeField] private IconSelectorUI iconSelector;

        protected virtual Color DefaultColor { get; } = new Color(.9216f, .3012f, .3608f);

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            CreateButton.onClick.AddListener(AddSection);
        }

        protected WaitableResult<Section> CurrentWaitableSection { get; set; }
        public virtual WaitableResult<Section> CreateSection()
        {
            CurrentWaitableSection?.SetError("New popup opened");
            CurrentWaitableSection = new WaitableResult<Section>();

            gameObject.SetActive(true);
            Color.Display(DefaultColor);

            return CurrentWaitableSection;
        }

        protected virtual void AddSection()
        {
            var name = NameField.text;
            var icon = IconSelector.Value;
            var color = Color.GetValue();

            var section = new Section(name, icon, color);
            CurrentWaitableSection.SetResult(section);
            CurrentWaitableSection = null;

            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableSection != null) {
                CurrentWaitableSection.SetError("Canceled");
                CurrentWaitableSection = null;
            }
            gameObject.SetActive(false);
        }
    }
}
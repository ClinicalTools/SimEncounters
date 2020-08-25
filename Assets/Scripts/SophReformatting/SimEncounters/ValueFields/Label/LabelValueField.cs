using TMPro;

namespace ClinicalTools.SimEncounters
{
    public class LabelValueField : BaseValueField
    {
        public override string Name => name;
        public override string Value => Label.text;

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        public override void Initialize() { }
        public override void Initialize(string value) => Label.text = value;
    }
}
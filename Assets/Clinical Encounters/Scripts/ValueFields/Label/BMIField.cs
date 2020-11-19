using ClinicalTools.SimEncounters;
using TMPro;
using UnityEngine;

namespace ClinicalTools.ClinicalEncounters
{
    public class BMIField : BaseField
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

        public TMP_InputField HeightValue { get => heightValue; set => heightValue = value; }
        [SerializeField] private TMP_InputField heightValue;
        public TMP_InputField WeightValue { get => weightValue; set => weightValue = value; }
        [SerializeField] private TMP_InputField weightValue;

        protected virtual void Awake()
        {
            HeightValue.onValueChanged.AddListener(HeightUpdated);
            WeightValue.onValueChanged.AddListener(WeightUpdated);
        }

        private float heightSq, weight;
        protected virtual void HeightUpdated(string weightText)
        {
            if (float.TryParse(weightText, out var height))
                heightSq = Mathf.Pow(height * 0.0254f, 2f);
            else
                heightSq = 0;
            
            UpdateBMI();
        }
        protected virtual void WeightUpdated(string weightText)
        {
            if (float.TryParse(weightText, out weight))
                weight *= 0.453592f;
            else
                weight = 0;

            UpdateBMI();
        }
        protected virtual void UpdateBMI() => Label.text = GetBMIText();
        protected string GetBMIText()
        {
            if (heightSq == 0 || weight == 0)
                return "";

            var bmi = weight / heightSq;
            return bmi.ToString("f1") + " kg/m²";
        }
    }
}
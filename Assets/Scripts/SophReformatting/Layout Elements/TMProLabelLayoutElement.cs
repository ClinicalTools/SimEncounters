using TMPro;
using UnityEngine;

namespace ClinicalTools.Layout
{
    [ExecuteAlways]
    public class TMProLabelLayoutElement : BaseLayoutElement
    {
        public override IDimensionLayout Width => WidthValues;
        public override IDimensionLayout Height => HeightValues;
        public PreferredDimensionLayout WidthValues { get => widthValues; set => widthValues = value; }
        [SerializeField] private PreferredDimensionLayout widthValues = new PreferredDimensionLayout();
        public PreferredDimensionLayout HeightValues { get => heightValues; set => heightValues = value; }
        [SerializeField] private PreferredDimensionLayout heightValues = new PreferredDimensionLayout();

        protected override void Initialize()
        {
            base.Initialize();

            var label = GetComponent<TextMeshProUGUI>();
            WidthValues.SetPreferredGetter(() => label.preferredWidth);
            HeightValues.SetPreferredGetter(() => label.preferredHeight);

            WidthValues.ValueChanged += () => InvokeValueChanged();
            HeightValues.ValueChanged += () => InvokeValueChanged();
        }
    }
}
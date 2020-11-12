using UnityEngine;

namespace ClinicalTools.Layout
{

    [ExecuteAlways]
    public class LayoutElement : BaseLayoutElement
    {
        public override IDimensionLayout Width => WidthValues;
        public override IDimensionLayout Height => HeightValues;
        [field: SerializeField] public DimensionLayout WidthValues { get; set; } = new DimensionLayout();
        [field: SerializeField] public DimensionLayout HeightValues { get; set; } = new DimensionLayout();

        protected override void Initialize()
        {
            base.Initialize();

            WidthValues.ValueChanged += () => InvokeValueChanged();
            HeightValues.ValueChanged += () => InvokeValueChanged();


            Debug.LogWarning("LayoutElement");
        }
    }
}
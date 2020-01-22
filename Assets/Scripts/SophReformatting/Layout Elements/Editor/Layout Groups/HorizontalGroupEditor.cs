using UnityEditor;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    [CustomEditor(typeof(HorizontalGroup))]
    public class HorizontalGroupEditor : GroupEditor
    {
        protected override Padding GetHorizontalPadding(LayoutGroup layoutGroup)
            => ((HorizontalGroup)layoutGroup).HorizontalPadding;

        protected override Padding GetVerticalPadding(LayoutGroup layoutGroup)
            => ((HorizontalGroup)layoutGroup).VerticalPadding;

        protected override SpacedPadding GetSpacedPadding(LayoutGroup layoutGroup)
            => ((HorizontalGroup)layoutGroup).HorizontalPadding;
    }
}
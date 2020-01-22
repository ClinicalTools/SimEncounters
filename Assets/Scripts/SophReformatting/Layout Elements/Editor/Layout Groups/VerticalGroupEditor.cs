using UnityEditor;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    [CustomEditor(typeof(VerticalGroup))]
    public class VerticalGroupEditor : GroupEditor
    {
        protected override Padding GetHorizontalPadding(LayoutGroup layoutGroup)
            => ((VerticalGroup)layoutGroup).HorizontalPadding;

        protected override Padding GetVerticalPadding(LayoutGroup layoutGroup)
            => ((VerticalGroup)layoutGroup).VerticalPadding;

        protected override SpacedPadding GetSpacedPadding(LayoutGroup layoutGroup)
            => ((VerticalGroup)layoutGroup).VerticalPadding;
    }
}
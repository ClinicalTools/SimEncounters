using UnityEditor;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    public abstract class GroupEditor : Editor
    {
        protected abstract Padding GetHorizontalPadding(LayoutGroup layoutGroup);
        protected abstract Padding GetVerticalPadding(LayoutGroup layoutGroup);
        protected abstract SpacedPadding GetSpacedPadding(LayoutGroup layoutGroup);

        public override VisualElement CreateInspectorGUI()
        {
            var layoutGroup = (LayoutGroup)target;

            var paddingUI = new PaddingEditorUI(GetHorizontalPadding(layoutGroup), GetVerticalPadding(layoutGroup));
            paddingUI.Updated += ValueUpdated;
            var spacing = new SpacingEditorUI(GetSpacedPadding(layoutGroup));
            spacing.Updated += ValueUpdated;
            var dimensionsUI = new LayoutGroupDimensionsEditorUI(layoutGroup);
            dimensionsUI.Updated += ValueUpdated;

            var rootElement = new VisualElement();
            rootElement.Add(paddingUI.Element);
            rootElement.Add(spacing.Element);
            rootElement.Add(dimensionsUI.Element);

            return rootElement;
        }

        protected virtual void ValueUpdated()
        {
            serializedObject.Update();
        }
    }
}
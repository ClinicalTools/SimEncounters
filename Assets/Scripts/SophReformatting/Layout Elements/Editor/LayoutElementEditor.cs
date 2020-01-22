using UnityEditor;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    [CustomEditor(typeof(LayoutElement))]
    public class LayoutElementEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var layoutElement = (LayoutElement)target;

            var layoutElementUI = new LayoutElementEditorUI(layoutElement.WidthValues, layoutElement.HeightValues);
            layoutElementUI.Updated += ValueUpdated;

            return layoutElementUI.Element;
        }

        protected virtual void ValueUpdated()
        {
            serializedObject.Update();
        }
    }
}
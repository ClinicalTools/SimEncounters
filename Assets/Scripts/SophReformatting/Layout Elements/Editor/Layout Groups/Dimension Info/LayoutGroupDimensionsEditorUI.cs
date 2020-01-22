using ClinicalTools.EditorElements;
using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ClinicalTools.Layout
{
    public class LayoutGroupDimensionsEditorUI : IEditorElement
    {
        public VisualElement Element { get; } = new VisualElement();
        public event Action Updated;

        private readonly CreateEditorUIElements createEditorUIElements = new CreateEditorUIElements();

        public LayoutGroupDimensionsEditorUI(LayoutGroup layoutGroup)
        {
            var childAlignmentElement = CreateChildAlign(layoutGroup);
            var controlSizeElement = CreateControlSizeElement(layoutGroup);
            var expandElement = CreateExpandElement(layoutGroup);
            var fitChildElement = CreateFitChildElement(layoutGroup);
            var layoutElementFoldout = createEditorUIElements.CreateFoldout("Layout Element");
            var layoutElementUI = new LayoutElementEditorUI(layoutGroup.GroupWidth.DimensionLayout, layoutGroup.GroupHeight.DimensionLayout);
            layoutElementUI.Updated += Updated;

            Element.Add(childAlignmentElement);
            Element.Add(controlSizeElement.Element);
            Element.Add(expandElement.Element);
            Element.Add(fitChildElement.Element);
            Element.Add(layoutElementFoldout);
            layoutElementFoldout.Add(layoutElementUI.Element);
        }

        public EnumField CreateChildAlign(LayoutGroup layoutGroup)
        {
            var childAlignField = createEditorUIElements.CreateEnumField("Child Alignment", layoutGroup.ChildAnchor);
            childAlignField.RegisterValueChangedCallback((changeEvent) => SetAlign(layoutGroup, changeEvent.newValue));

            return childAlignField;
        }
        public void SetAlign(LayoutGroup layoutGroup, Enum value)
        {
            layoutGroup.ChildAnchor = (UnityEngine.TextAnchor)value;
            Updated?.Invoke();
        }

        public DimensionsTogglePair CreateExpandElement(LayoutGroup layoutGroup)
        {
            var width = layoutGroup.GroupWidth;
            var height = layoutGroup.GroupHeight;

            var expandPair = new DimensionsTogglePair("Child Force Expand", width.ExpandChild, height.ExpandChild);
            expandPair.WidthValueChanged += (value) => SetExpand(width, value);
            expandPair.HeightValueChanged += (value) => SetExpand(height, value);

            return expandPair;
        }
        public void SetExpand(LayoutGroupDimension layoutGroupDimension, bool expand)
        {
            layoutGroupDimension.ExpandChild = expand;
            Updated?.Invoke();
        }

        public DimensionsTogglePair CreateControlSizeElement(LayoutGroup layoutGroup)
        {
            var width = layoutGroup.GroupWidth;
            var height = layoutGroup.GroupHeight;

            var controlSizePair = new DimensionsTogglePair("Control Child Size", width.ControlChild, height.ControlChild);
            controlSizePair.WidthValueChanged += (value) => SetControlSize(width, value);
            controlSizePair.HeightValueChanged += (value) => SetControlSize(height, value);

            return controlSizePair;
        }
        public void SetControlSize(LayoutGroupDimension layoutGroupDimension, bool controlSize)
        {
            layoutGroupDimension.ControlChild = controlSize;
            Updated?.Invoke();
        }

        public DimensionsTogglePair CreateFitChildElement(LayoutGroup layoutGroup)
        {
            var width = layoutGroup.GroupWidth;
            var height = layoutGroup.GroupHeight;

            var fitChildPair = new DimensionsTogglePair("Fit Children", width.FitChild, height.FitChild);
            fitChildPair.WidthValueChanged += (value) => SetFitChild(width, value);
            fitChildPair.HeightValueChanged += (value) => SetFitChild(height, value);

            return fitChildPair;
        }
        public void SetFitChild(LayoutGroupDimension layoutGroupDimension, bool fitChild)
        {
            layoutGroupDimension.FitChild = fitChild;
            Updated?.Invoke();
        }
    }
}
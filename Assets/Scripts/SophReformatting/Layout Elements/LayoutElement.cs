using System;
using UnityEngine;

namespace ClinicalTools.Layout
{
    [ExecuteAlways]
    public class LayoutElement : UIElement, ILayoutElement
    {
        public event Action ValueChanged;
        public virtual IDimensionLayout Width => WidthValues;
        public virtual IDimensionLayout Height => HeightValues;
        [field: SerializeField] public DimensionLayout WidthValues { get; set; } = new DimensionLayout();
        [field: SerializeField] public DimensionLayout HeightValues { get; set; } = new DimensionLayout();

        private LayoutGroup parent;

        protected override void Awake()
        {
            Initialize();

            base.Awake();
        }

        private bool initialized = false;
        protected virtual void Update()
        {
            if (!initialized)
                Initialize();
            var newSiblingIndex = transform.GetSiblingIndex();
            if (newSiblingIndex != siblingIndex) {
                siblingIndex = newSiblingIndex;
                ValueChanged?.Invoke();
            }
        }

        private int siblingIndex = -1;
        public void Initialize()
        {
            parent = transform.parent.GetComponent<LayoutGroup>();
            if (parent != null)
                parent.AddChild(this);

            siblingIndex = transform.GetSiblingIndex();
            WidthValues.ValueChanged += () => ValueChanged?.Invoke();
            HeightValues.ValueChanged += () => ValueChanged?.Invoke();

            initialized = true;
        }

        protected override void OnDestroy()
        {
            if (parent != null)
                parent.RemoveChild(this);
        }

        public void UpdateSize(float width, float height) { }
    }
}
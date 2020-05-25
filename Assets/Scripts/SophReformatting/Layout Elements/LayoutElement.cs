using System;
using UnityEngine;

namespace ClinicalTools.Layout
{
    public abstract class BaseLayoutElement : UIElement, ILayoutElement
    {
        public virtual event Action ValueChanged;
        public abstract IDimensionLayout Width { get; }
        public abstract IDimensionLayout Height { get; }

        private bool ignoreLayout;
        public virtual void SetIgnoreLayout(bool ignoreLayout)
        {
            if (this.ignoreLayout == ignoreLayout || parent == null)
                return;

            this.ignoreLayout = ignoreLayout;
            if (ignoreLayout)
                parent.RemoveChild(this);
            else
                parent.AddChild(this);
        }

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
                InvokeValueChanged();
            }
        }

        private int siblingIndex = -1;
        protected virtual void Initialize()
        {
            var parentTransform = transform.parent;
            if (parentTransform != null)
                parent = parentTransform.GetComponent<LayoutGroup>();
            if (parent != null)
                parent.AddChild(this);

            siblingIndex = transform.GetSiblingIndex();

            initialized = true;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (parent != null)
                return;

            var parentTransform = transform.parent;
            if (parentTransform != null)
                parent = parentTransform.GetComponent<LayoutGroup>();
            if (parent != null)
                parent.AddChild(this);
        }
        protected override void OnDestroy()
        {
            if (parent != null)
                parent.RemoveChild(this);

            base.OnDestroy();
        }
        protected override void OnDisable()
        {
            if (parent != null)
                parent.RemoveChild(this);
            parent = null;

            base.OnDisable();
        }

        public void UpdateSize(float width, float height) { }

        protected virtual void InvokeValueChanged() => ValueChanged?.Invoke();
    }

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
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.Layout
{
    [ExecuteAlways]
    public class LayoutImage : UIElement, ILayoutElement
    {
        public event Action ValueChanged;
        public virtual IDimensionLayout Width => WidthValues;
        public virtual IDimensionLayout Height => HeightValues;
        [field: SerializeField] public bool PreserveAspectRatio { get; set; }
        [field: SerializeField] public Image Image { get; set; }
        [field: SerializeField] public DimensionLayout WidthValues { get; set; } = new DimensionLayout();
        [field: SerializeField] public DimensionLayout HeightValues { get; set; } = new DimensionLayout();

        private LayoutGroup parent;

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

        public void UpdateSprite(Sprite sprite) {
            Image.sprite = sprite;
            //if (WidthValues.Preferred && HeightValues.Preferred)
            //float preferredAspect = WidthValues.Preferred= get
        }

        public void UpdateSize(float width, float height) { }
    }
}
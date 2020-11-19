using UnityEngine;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public abstract class ColorBehaviour : MonoBehaviour
    {
        public ColorType ColorType { get => colorType; set => colorType = value; }
        [SerializeField] private ColorType colorType;

        protected virtual void Awake() => UpdateColor();
        protected virtual void Update()
        {
            if (previousType != ColorType)
                UpdateColor();
        }

        private ColorType previousType;
        protected virtual void UpdateColor()
        {
            previousType = ColorType;
            SetColor(ColorManager.GetColor(ColorType));
        }

        protected abstract void SetColor(Color color);
    }
}
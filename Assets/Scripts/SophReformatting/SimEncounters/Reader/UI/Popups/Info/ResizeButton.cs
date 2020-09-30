using ClinicalTools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ResizeButton : MonoBehaviour
    {
        public bool Enlarge { get => enlarge; set => enlarge = value; }
        [SerializeField] private bool enlarge;

        private Button button;
        public Button Button {
            get {
                if (button == null)
                    button = GetComponent<Button>();
                return button;
            }
        }

        protected virtual void Awake() => Button.onClick.AddListener(Resize);

        protected virtual void Resize()
        {
            CanvasResizer.Instance.ResizeValue += GetResizeValue();
            Update();
        }

        private float lastResizeValue = -1f;
        protected virtual void Update()
        {
            if (lastResizeValue == CanvasResizer.Instance.ResizeValue)
                return;

            lastResizeValue = CanvasResizer.Instance.ResizeValue;
            Button.interactable = Enlarge ? lastResizeValue < .95f : lastResizeValue > .05f;
        }

        private const float ResizeValue = .1f;
        protected float GetResizeValue() => Enlarge ? ResizeValue : -ResizeValue;
    }
}
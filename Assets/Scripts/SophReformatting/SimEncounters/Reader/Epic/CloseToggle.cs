using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ClinicalTools.UI.Extensions;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Toggle))]
    public class CloseToggle : MonoBehaviour
    {
        private Toggle toggle;
        protected Toggle Toggle
        {
            get {
                if (toggle == null)
                    toggle = GetComponent<Toggle>();
                return toggle;
            }
        }

        protected ICloseHandler CloseHandler { get; set; }
        [Inject] public virtual void Inject(ICloseHandler closeHandler) => CloseHandler = closeHandler;

        protected virtual void Awake() => Toggle.AddOnSelectListener(Close);
        protected virtual void Close() => CloseHandler?.Close(this);
    }
}
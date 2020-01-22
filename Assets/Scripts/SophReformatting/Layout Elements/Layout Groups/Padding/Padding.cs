using System;
using UnityEngine;

namespace ClinicalTools.Layout
{
    [Serializable]
    public class Padding
    {
        public event Action ValueChanged;
        protected void InvokeValueChanged() => ValueChanged?.Invoke();

        [SerializeField] private float start;
        public virtual float Start {
            get { return start; }
            set {
                start = value;
                ValueChanged?.Invoke();
            }
        }
        [SerializeField] private float end;
        public virtual float End {
            get { return end; }
            set {
                end = value;
                ValueChanged?.Invoke();
            }
        }
    }
}
using System;
using UnityEngine;

namespace ClinicalTools.Layout
{
    [Serializable]
    public class SpacedPadding : Padding
    {
        [SerializeField] private float spacing;
        public virtual float Spacing {
            get { return spacing; }
            set {
                spacing = value;
                InvokeValueChanged();
            }
        }
    }
}
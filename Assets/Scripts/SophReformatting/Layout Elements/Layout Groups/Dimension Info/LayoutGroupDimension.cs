using System;
using UnityEngine;

namespace ClinicalTools.Layout
{
    [Serializable]
    public class LayoutGroupDimension
    {
        public event Action ValueChanged;

        [SerializeField] private DimensionLayout dimensionLayout = new DimensionLayout();
        public DimensionLayout DimensionLayout => dimensionLayout;
        public LayoutGroupDimension()
        {
            DimensionLayout.ValueChanged += () => ValueChanged?.Invoke();
        }

        [SerializeField] private bool fitChild = true;
        public bool FitChild {
            get { return fitChild; }
            set {
                fitChild = value;
                ValueChanged?.Invoke();
            }
        }

        [SerializeField] private bool controlChild = false;
        public bool ControlChild {
            get { return controlChild; }
            set {
                controlChild = value;
                ValueChanged?.Invoke();
            }
        }

        [SerializeField] private bool expandChild = false;
        public bool ExpandChild {
            get { return expandChild; }
            set {
                expandChild = value;
                ValueChanged?.Invoke();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.SimEncounters.UI
{
    public class TransformA : UIBehaviour
    {
        public event Action<Transform> RectTransformDimensionsChange;
        
        protected override void OnRectTransformDimensionsChange()
        {
            RectTransformDimensionsChange?.Invoke(transform);

            base.OnRectTransformDimensionsChange();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.UI
{
    public class BetterSizeFitters : TransformA
    {
        [field: SerializeField] public virtual DimentionLayout Width { get; } = new DimentionLayout();
        [field: SerializeField] public virtual DimentionLayout Height { get; } = new DimentionLayout();


        // Start is called before the first frame update
        private void Awake()
        {
            var layoutElements = GetComponentInChildren<LayoutElement>();
        }

        private void OnTransformChildrenChanged()
        {

        }
    }
}
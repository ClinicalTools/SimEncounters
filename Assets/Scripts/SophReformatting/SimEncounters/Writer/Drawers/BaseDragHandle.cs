using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseDragHandle : MonoBehaviour
    {
        public abstract event Action StartDragging;
    }
}
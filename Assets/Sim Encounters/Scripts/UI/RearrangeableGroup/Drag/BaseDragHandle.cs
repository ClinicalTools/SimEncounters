using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseDragHandle : MonoBehaviour
    {
        public abstract event Action StartDragging;
    }
}
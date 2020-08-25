using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseColorEditor : MonoBehaviour
    {
        public abstract event Action<Color> ValueChanged;

        public abstract void Display(Color color);
        public abstract Color GetValue();
    }
}
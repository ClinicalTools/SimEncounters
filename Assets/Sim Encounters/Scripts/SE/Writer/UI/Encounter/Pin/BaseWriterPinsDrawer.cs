﻿using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterPinsDrawer : MonoBehaviour
    {
        // !!!! do this
        public virtual void Display(PinGroup pinData) { }
        public abstract void Display(Encounter encounter, PinGroup pinData);

        public abstract PinGroup Serialize();
    }
}
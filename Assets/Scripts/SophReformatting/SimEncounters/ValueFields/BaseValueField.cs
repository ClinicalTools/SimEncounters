
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseField : MonoBehaviour
    {
        public abstract string Name { get; }
        public abstract string Value { get; }
    }
    public abstract class BaseValueField : BaseField
    {
        public abstract void Initialize();
        public abstract void Initialize(string value);
    }

    public abstract class BaseEncounterField : BaseField
    {
        public abstract void Initialize(Encounter encounter);
        public abstract void Initialize(Encounter encounter, string value);
    }
}
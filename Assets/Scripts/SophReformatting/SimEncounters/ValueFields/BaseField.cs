using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseField : MonoBehaviour, INamedField
    {
        public abstract string Name { get; }
        public abstract string Value { get; }
    }
}
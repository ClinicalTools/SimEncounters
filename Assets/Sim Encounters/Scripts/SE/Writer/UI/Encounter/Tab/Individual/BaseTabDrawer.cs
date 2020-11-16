using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseTabDrawer : MonoBehaviour
    {
        public abstract void Display(Encounter encounter, Tab tab);
        public abstract Tab Serialize();

        public class Factory : PlaceholderFactory<Object, BaseTabDrawer> { }
    }
}
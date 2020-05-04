using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseUserTabDrawer : MonoBehaviour
    {
        public abstract void Display(UserTab tab);
    }
}
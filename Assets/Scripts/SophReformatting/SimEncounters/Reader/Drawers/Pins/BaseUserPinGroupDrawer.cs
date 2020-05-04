using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseUserPinGroupDrawer : MonoBehaviour
    {
        public abstract void Display(UserPinGroup userPanel);
    }
}
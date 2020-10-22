using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserPinGroupDrawer : MonoBehaviour
    {
        public abstract void Display(UserPinGroup userPanel);

        public class Pool : MonoMemoryPool<BaseUserPinGroupDrawer> { }
    }
}
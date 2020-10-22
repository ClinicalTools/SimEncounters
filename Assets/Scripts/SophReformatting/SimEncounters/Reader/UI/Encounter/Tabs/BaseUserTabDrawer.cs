using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserTabDrawer : MonoBehaviour, IUserTabDrawer
    {
        public abstract void Display(UserTabSelectedEventArgs eventArgs);

        public class Factory : PlaceholderFactory<string, BaseUserTabDrawer> { }
    }
}
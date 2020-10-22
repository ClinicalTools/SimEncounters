using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderPanel : MonoBehaviour
    {
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        public abstract void Display(UserPanel panel);

        public class Factory : PlaceholderFactory<Object, BaseReaderPanel> { }
    }
}
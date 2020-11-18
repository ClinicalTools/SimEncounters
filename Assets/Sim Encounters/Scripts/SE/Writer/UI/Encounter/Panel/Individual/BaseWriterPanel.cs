using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterPanel : MonoBehaviour
    {
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        public abstract void Display(Encounter encounter);
        public abstract void Display(Encounter encounter, Panel panel);

        public abstract Panel Serialize();


        public class Factory : PlaceholderFactory<UnityEngine.Object, BaseWriterPanel> { }
    }
}
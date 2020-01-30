using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SceneUI : MonoBehaviour
    {
        [SerializeField] private Transform popupsParent;
        public virtual Transform PopupsParent { get => popupsParent; set => popupsParent = value; }
    }
}
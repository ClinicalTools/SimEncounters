using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderImage : MonoBehaviour, IReaderValueField
    {
        public string Name => name;
        public string Value { get; protected set; }

        protected Image Image { get; set; }

        protected virtual void Awake()
        {
            Image = GetComponent<Image>();
        }

        public virtual void Initialize(EncounterReader reader) { }

        public virtual void Initialize(EncounterReader reader, string value)
        {
            var sprites = reader.Encounter.Images.Sprites;
            if (sprites.ContainsKey(value))
                Image.sprite = sprites[value];
        }
    }
}
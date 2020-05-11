using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ColorImageValueField : MonoBehaviour, IValueField
    {
        public string Name => name;
        public string Value { get; set; }

        protected Image Image { get; set; }
        protected IParser<Color> ColorParser { get; set; }
        [Inject] public virtual void Inject(IParser<Color> colorParser) => ColorParser = colorParser;

        protected virtual void Awake() => Image = GetComponent<Image>();

        public void Initialize() { }

        public void Initialize(string value)
        {
            Value = value;
            Image.color = ColorParser.Parse(value);
        }
    }
}
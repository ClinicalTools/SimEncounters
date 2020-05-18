using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ColorImageValueField : BaseValueField
    {
        public override string Name => name;
        public override string Value => value;
        private string value = null;

        protected Image Image { get; set; }
        protected IParser<Color> ColorParser { get; set; }
        [Inject] public virtual void Inject(IParser<Color> colorParser) => ColorParser = colorParser;

        protected virtual void Awake() => Image = GetComponent<Image>();

        public override void Initialize() { }

        public override void Initialize(string value)
        {
            this.value = value;
            Image.color = ColorParser.Parse(value);
        }
    }
}
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class HiddenValueField : BaseValueField
    {
        public override string Name => name;
        public override string Value => value;
        private string value = null;

        public override void Initialize() { }
        public override void Initialize(string value) => this.value = value;
    }
}
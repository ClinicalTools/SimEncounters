using System;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public abstract class EncounterButton<T> : ISelectable<T>
    {
        public event Action<T> Selected;

        protected T Value { get; }

        public EncounterButton(Button button, T value)
        {
            Value = value;
            button.onClick.AddListener(() => Selected?.Invoke(Value));
        }

        public virtual void Select() => Selected?.Invoke(Value);
    }
}
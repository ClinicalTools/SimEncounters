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
            button.onClick.AddListener(() => Selected?.Invoke(value));
        }
        public EncounterButton(Toggle toggle, T value)
        {
            Value = value;
            toggle.onValueChanged.AddListener(
                (selected) => {
                    if (selected)
                        Selected?.Invoke(value);
                }
            );
        }

        public virtual void Select() => Selected?.Invoke(Value);
    }
}
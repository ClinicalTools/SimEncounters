using System;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class EncounterButton<T> : ISelectable<T>
    {
        public event Action<T> Selected;

        public EncounterButton(Button button, T value)
        {
            button.onClick.AddListener(() => Selected?.Invoke(value));
        }
    }
}
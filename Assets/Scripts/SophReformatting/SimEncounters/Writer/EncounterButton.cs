using System;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
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
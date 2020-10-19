using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class SelectorBehaviour<T> : MonoBehaviour, ISelector<T>
    {
        public event SelectedHandler<T> Selected;

        protected virtual object CurrentSender { get; set; }
        protected virtual T CurrentValue { get; set; }

        public virtual void Select(object sender, T value)
        {
            if (CurrentSender == sender && CurrentValue.Equals(value))
                return;
            CurrentSender = sender;
            CurrentValue = value;
            Selected?.Invoke(sender, value);
        }

        public virtual void AddSelectedListener(SelectedHandler<T> handler)
        {
            Selected += handler;
            if (CurrentValue != null)
                handler(CurrentSender, CurrentValue);
        }

        public virtual void RemoveSelectedListener(SelectedHandler<T> handler) => Selected -= handler;
    }
    public class Selector<T> : ISelector<T>
    {
        public event SelectedHandler<T> Selected;

        protected virtual object CurrentSender { get; set; }
        protected virtual T CurrentValue { get; set; }

        public virtual void Select(object sender, T value)
        {
            if (CurrentSender == sender && CurrentValue.Equals(value))
                return;
            CurrentSender = sender;
            CurrentValue = value;
            Selected?.Invoke(sender, value);
        }

        public virtual void AddSelectedListener(SelectedHandler<T> handler)
        {
            Selected += handler;
            if (CurrentValue != null)
                handler(CurrentSender, CurrentValue);
        }

        public virtual void RemoveSelectedListener(SelectedHandler<T> handler) => Selected -= handler;
    }
}
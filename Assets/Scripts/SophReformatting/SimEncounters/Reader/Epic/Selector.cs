using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class SelectorBehaviour<T> : MonoBehaviour, ISelector<T>
    {
        protected virtual Selector<T> Selector { get; } = new Selector<T>();
        protected virtual object CurrentSender => Selector.CurrentSender;
        protected virtual T CurrentValue => Selector.CurrentValue;

        public virtual void Select(object sender, T value) => Selector.Select(sender, value);

        public virtual void AddEarlySelectedListener(SelectedHandler<T> handler)
            => Selector.AddEarlySelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<T> handler)
            => Selector.AddSelectedListener(handler);
        public virtual void AddLateSelectedListener(SelectedHandler<T> handler)
            => Selector.AddLateSelectedListener(handler);

        public virtual void RemoveEarlySelectedListener(SelectedHandler<T> handler) 
            => Selector.RemoveEarlySelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<T> handler) 
            => Selector.RemoveSelectedListener(handler);
        public virtual void RemoveLateSelectedListener(SelectedHandler<T> handler) 
            => Selector.RemoveLateSelectedListener(handler);
    }
    public class Selector<T> : ISelector<T>
    {
        public event SelectedHandler<T> EarlySelected;
        public event SelectedHandler<T> Selected;
        public event SelectedHandler<T> LateSelected;

        public object CurrentSender { get; protected set; }
        public T CurrentValue { get; protected set; }

        public virtual void Select(object sender, T value)
        {
            if (CurrentSender == sender && CurrentValue.Equals(value))
                return;
            CurrentSender = sender;
            CurrentValue = value;
            EarlySelected?.Invoke(sender, value);
            Selected?.Invoke(sender, value);
            LateSelected?.Invoke(sender, value);
        }

        public virtual void AddEarlySelectedListener(SelectedHandler<T> handler)
        {
            Selected += handler;
            InitiateHandler(handler);
        }
        public virtual void AddSelectedListener(SelectedHandler<T> handler)
        {
            Selected += handler;
            InitiateHandler(handler);
        }
        public virtual void AddLateSelectedListener(SelectedHandler<T> handler)
        {
            Selected += handler;
            InitiateHandler(handler);
        }

        protected virtual void InitiateHandler(SelectedHandler<T> handler)
        {
            if (CurrentValue != null)
                handler(CurrentSender, CurrentValue);
        }

        public virtual void RemoveEarlySelectedListener(SelectedHandler<T> handler) => EarlySelected -= handler;
        public virtual void RemoveSelectedListener(SelectedHandler<T> handler) => Selected -= handler;
        public virtual void RemoveLateSelectedListener(SelectedHandler<T> handler) => LateSelected -= handler;
    }
}
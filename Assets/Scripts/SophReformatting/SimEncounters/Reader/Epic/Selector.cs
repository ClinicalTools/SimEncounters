namespace ClinicalTools.SimEncounters
{
    public class Selector<T> : ISelector<T>
    {
        public event SelectedHandler<T> Selected;

        public object CurrentSender { get; protected set; }
        public T CurrentValue { get; protected set; }

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
            InitiateHandler(handler);
        }
        protected virtual void InitiateHandler(SelectedHandler<T> handler)
        {
            if (CurrentValue != null)
                handler(CurrentSender, CurrentValue);
        }

        public virtual void RemoveSelectedListener(SelectedHandler<T> handler) => Selected -= handler;
    }
}
using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters
{
    public abstract class ButtonGroup<T>
    {
        protected virtual OrderedCollection<T> Values { get; }

        /// <summary>
        /// <see cref="CreateInitialButtons"/> is expected to be called in the constructor of the child.
        /// </summary>
        protected ButtonGroup(OrderedCollection<T> values)
        {
            Values = values;
        }

        protected virtual void CreateInitialButtons(OrderedCollection<T> values)
        {
            foreach (var value in values)
                AddSelectButton(value.Value);

            if (values.Count > 0)
                Select(values[0].Value);
        }

        protected virtual void Add(T value)
        {
            Values.Add(value);
            AddSelectButton(value);
        }

        protected virtual void AddSelectButton(T value)
        {
            var sectionButton = AddButton(value);
            sectionButton.Selected += Select;
        }

        protected abstract ISelectable<T> AddButton(T value);

        protected abstract void Select(T value);
    }
}
using ClinicalTools.SimEncounters.Collections;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public abstract class ButtonGroup<T>
    {
        protected virtual OrderedCollection<T> Values { get; }

        protected virtual List<ISelectable<T>> SelectButtons { get; } = new List<ISelectable<T>>();

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
                SelectButtons.Add(AddSelectButton(value.Value));

            if (SelectButtons.Count > 0)
                SelectButtons[0].Select();
        }

        protected virtual void Add(T value)
        {
            Values.Add(value);
            SelectButtons.Add(AddSelectButton(value));
        }

        protected virtual ISelectable<T> AddSelectButton(T value)
        {
            var sectionButton = AddButton(value);
            sectionButton.Selected += Select;
            return sectionButton;
        }

        protected abstract ISelectable<T> AddButton(T value);

        protected abstract void Select(T value);
    }
}
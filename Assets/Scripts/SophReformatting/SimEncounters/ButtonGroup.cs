using ClinicalTools.SimEncounters.Collections;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public abstract class ButtonGroup<T>
    {
        protected virtual OrderedCollection<T> Values { get; }

        protected virtual List<ISelectable<KeyValuePair<string, T>>> SelectButtons { get; } = new List<ISelectable<KeyValuePair<string, T>>>();

        protected virtual int FirstButtonIndex => 0;

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
                SelectButtons.Add(AddSelectButton(value));

            if (SelectButtons.Count > 0)
                SelectButtons[FirstButtonIndex].Select();
        }

        protected virtual void Add(KeyValuePair<string, T> value)
        {
            Values.Add(value);
            SelectButtons.Add(AddSelectButton(value));
        }

        protected virtual ISelectable<KeyValuePair<string, T>> AddSelectButton(KeyValuePair<string, T> value)
        {
            var sectionButton = AddButton(value);
            sectionButton.Selected += Select;
            return sectionButton;
        }

        protected abstract ISelectable<KeyValuePair<string, T>> AddButton(KeyValuePair<string, T> value);

        protected abstract void Select(KeyValuePair<string, T> value);
    }
}
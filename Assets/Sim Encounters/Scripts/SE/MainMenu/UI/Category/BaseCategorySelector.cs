using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCategorySelector : MonoBehaviour,
        ISelectedListener<CategorySelectedEventArgs>
    {
        protected virtual Selector<CategorySelectedEventArgs> Selector { get; } = new Selector<CategorySelectedEventArgs>();

        public virtual CategorySelectedEventArgs CurrentValue => Selector.CurrentValue;
        public virtual void AddSelectedListener(SelectedHandler<CategorySelectedEventArgs> handler)
            => Selector.AddSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<CategorySelectedEventArgs> handler)
            => Selector.RemoveSelectedListener(handler);

        public abstract void Display(MenuSceneInfo sceneInfo, IEnumerable<Category> categories);
        public abstract void Show();
        public abstract void Hide();
    }
}
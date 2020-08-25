using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCategorySelector : MonoBehaviour
    {
        public abstract event Action<Category> CategorySelected;
        public abstract void Display(MenuSceneInfo sceneInfo, IEnumerable<Category> categories);
        public abstract void Show();
        public abstract void Hide();
    }
}
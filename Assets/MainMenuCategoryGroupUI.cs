using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuCategoryGroupUI : MonoBehaviour
    {
        [SerializeField] private Transform optionsParent;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }

        [SerializeField] private MainMenuCategoryUI categoryPrefab;
        public MainMenuCategoryUI CategoryPrefab { get => categoryPrefab; set => categoryPrefab = value; }


        public event Action<string> CategorySelected;

        public void Display(IEnumerable<string> categories)
        {
            foreach (var category in categories) {
                var categoryUI = Instantiate(CategoryPrefab, OptionsParent);
                categoryUI.Selected += () => CategorySelected?.Invoke(category);
                categoryUI.Display(category);
            }
        }

        public void Clear()
        {
            foreach (Transform option in OptionsParent)
                Destroy(option.gameObject);
        }
    }
}
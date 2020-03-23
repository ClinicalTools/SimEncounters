using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuCategoryGroupUI : MonoBehaviour
    {
        [SerializeField] private Transform optionsParent;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }

        [SerializeField] private MainMenuCategorySelectorUI categoryPrefab;
        public MainMenuCategorySelectorUI CategoryPrefab { get => categoryPrefab; set => categoryPrefab = value; }


        public event Action<string> CategorySelected;

        protected List<MainMenuCategorySelectorUI> CategoryUIs { get; } = new List<MainMenuCategorySelectorUI>();
        public void Display(Dictionary<string, Category> categories)
        {
            foreach (var categoryUI in CategoryUIs)
                Destroy(categoryUI.gameObject);
            CategoryUIs.Clear();

            var categoryNames = new List<string>(categories.Keys);
            categoryNames.Sort();

            foreach (var categoryName in categoryNames) {
                var categoryUI = Instantiate(CategoryPrefab, OptionsParent);
                categoryUI.Selected += () => CategorySelected?.Invoke(categoryName);
                categoryUI.Display(categoryName, categories[categoryName]);
                CategoryUIs.Add(categoryUI);
            }
        }

        public void Clear()
        {
            foreach (Transform option in OptionsParent)
                Destroy(option.gameObject);
            CategoryUIs.Clear();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
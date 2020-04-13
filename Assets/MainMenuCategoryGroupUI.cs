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


        public event Action<Category> CategorySelected;

        protected List<MainMenuCategorySelectorUI> CategoryUIs { get; } = new List<MainMenuCategorySelectorUI>();
        public void Display(List<Category> categories)
        {
            foreach (var categoryUI in CategoryUIs)
                Destroy(categoryUI.gameObject);
            CategoryUIs.Clear();

            categories.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));

            foreach (var category in categories) {
                var categoryUI = Instantiate(CategoryPrefab, OptionsParent);
                categoryUI.Selected += () => CategorySelected?.Invoke(category);
                categoryUI.Display(category.Name, category);
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
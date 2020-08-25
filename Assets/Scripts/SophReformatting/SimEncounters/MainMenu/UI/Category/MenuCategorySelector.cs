
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{

    public class MenuCategorySelector : BaseCategorySelector
    {
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }
        [SerializeField] private Transform optionsParent;
        public MainMenuCategorySelectorUI CategoryPrefab { get => categoryPrefab; set => categoryPrefab = value; }
        [SerializeField] private MainMenuCategorySelectorUI categoryPrefab;

        public override event Action<Category> CategorySelected;

        protected List<MainMenuCategorySelectorUI> CategoryUIs { get; } = new List<MainMenuCategorySelectorUI>();
        protected IEnumerable<Category> CurrentCategories { get; set; }
        public override void Display(MenuSceneInfo sceneInfo, IEnumerable<Category> categories)
        {
            gameObject.SetActive(true);
            if (CurrentCategories == categories)
                return;
            CurrentCategories = categories;
            
            foreach (var categoryUI in CategoryUIs)
                Destroy(categoryUI.gameObject);
            CategoryUIs.Clear();

            var sortedCategories = SortCategories(categories);
            foreach (var category in sortedCategories)
                ShowCategoryButton(category);
        }

        protected virtual IEnumerable<Category> SortCategories(IEnumerable<Category> categories)
        {
            var sortedCategories = new List<Category>(categories);
            sortedCategories.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));
            return sortedCategories;
        }

        protected virtual void ShowCategoryButton(Category category)
        {
            var categoryUI = Instantiate(CategoryPrefab, OptionsParent);
            categoryUI.Selected += () => CategorySelected?.Invoke(category);
            categoryUI.Display(category.Name, category);
            CategoryUIs.Add(categoryUI);
        }

        public override void Show() => gameObject.SetActive(true);

        public override void Hide() => gameObject.SetActive(false);
    }
}
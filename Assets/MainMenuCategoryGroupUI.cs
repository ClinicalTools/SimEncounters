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

        public List<MainMenuCategoryUI> CategoryUIs = new List<MainMenuCategoryUI>();
        public void Display(IEnumerable<string> categories)
        {
            foreach (var categoryUI in CategoryUIs)
                Destroy(categoryUI.gameObject);

            foreach (var category in categories) {
                var categoryUI = Instantiate(CategoryPrefab, OptionsParent);
                categoryUI.Selected += () => CategorySelected?.Invoke(category);
                categoryUI.Display(category);
                CategoryUIs.Add(categoryUI);
            }

            Show();
        }

        public void Clear()
        {
            foreach (Transform option in OptionsParent)
                Destroy(option.gameObject);
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
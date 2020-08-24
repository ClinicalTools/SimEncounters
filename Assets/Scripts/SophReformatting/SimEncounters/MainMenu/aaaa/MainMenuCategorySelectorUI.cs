
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuCategorySelectorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI categoryLabel;
        public TextMeshProUGUI CategoryLabel { get => categoryLabel; set => categoryLabel = value; }

        [SerializeField] private Button selectButton;
        public Button SelectButton { get => selectButton; set => selectButton = value; }


        [SerializeField] private GameObject inProgressObject;
        public virtual GameObject InProgressObject { get => inProgressObject; set => inProgressObject = value; }

        [SerializeField] private GameObject completedObject;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }

        public event Action Selected;

        public void Display(string categoryName, Category category)
        {
            CategoryLabel.text = categoryName;
            SelectButton.onClick.AddListener(() => Selected?.Invoke());

            if (category.IsCompleted())
                CompletedObject.SetActive(true);
        }
    }
}
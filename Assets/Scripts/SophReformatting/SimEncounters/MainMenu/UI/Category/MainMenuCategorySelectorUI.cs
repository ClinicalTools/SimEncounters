using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuCategorySelectorUI : MonoBehaviour
    {
        public TextMeshProUGUI CategoryLabel { get => categoryLabel; set => categoryLabel = value; }
        [SerializeField] private TextMeshProUGUI categoryLabel;
        public Button SelectButton { get => selectButton; set => selectButton = value; }
        [SerializeField] private Button selectButton;

        public virtual GameObject InProgressObject { get => inProgressObject; set => inProgressObject = value; }
        [SerializeField] private GameObject inProgressObject;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }
        [SerializeField] private GameObject completedObject;

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
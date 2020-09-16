﻿using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class MobileReaderTabDrawerSelector : BaseUserTabDrawer
    {
        public Transform TabParent { get => tabParent; set => tabParent = value; }
        [SerializeField] private Transform tabParent;
        public TMP_Text TabName { get => tabName; set => tabName = value; }
        [SerializeField] private TMP_Text tabName;

        protected BaseUserTabDrawer CurrentTabDrawer { get; set; }
        public override void Display(UserTab tab)
        {
            if (CurrentTabDrawer != null)
                Destroy(CurrentTabDrawer.gameObject);

            TabName.text = tab.Data.Name;
            var prefab = GetTabPrefab(tab.Data);
            CurrentTabDrawer = Instantiate(prefab, TabParent);
            CurrentTabDrawer.Display(tab);
        }

        protected virtual BaseUserTabDrawer GetTabPrefab(Tab tab)
        {
            var tabFolder = $"soph/se/Mobile/Reader/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            return tabPrefabGameObject.GetComponent<BaseUserTabDrawer>();
        }
    }
}
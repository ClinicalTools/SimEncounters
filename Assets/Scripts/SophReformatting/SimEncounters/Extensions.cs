using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Extensions
{
    public static class Extensions
    {
        private static readonly Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void AddOnSelectListener(this Toggle toggle, UnityAction action)
        {
            toggle.onValueChanged.AddListener(
                (selected) => { 
                    if (selected) action(); 
                }
            );
        }
    }
}
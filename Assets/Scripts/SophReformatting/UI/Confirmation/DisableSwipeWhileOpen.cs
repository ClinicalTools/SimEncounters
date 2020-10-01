﻿using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class DisableSwipeWhileOpen : MonoBehaviour
    {
#if MOBILE
        protected SwipeManager SwipeManager { get; set; }
        [Inject]
        public virtual void Inject(SwipeManager swipeManager) => SwipeManager = swipeManager;

        protected virtual void OnEnable() => SwipeManager.AllowSwipe = false;
        protected virtual void OnDisable() => SwipeManager.AllowSwipe = true;
#endif
    }
}
﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseTooltip : MonoBehaviour
    {
        public abstract void Show();
    }

    public class Tooltip : BaseTooltip, IPointerDownHandler
    {
        public CanvasGroup Group { get => group; set => group = value; }
        [SerializeField] private CanvasGroup group;

        public virtual void OnPointerDown(PointerEventData eventData) => Group.alpha = 0;

        public override void Show()
        {
            if (HideEnumerator != null)
                StopCoroutine(HideEnumerator);
            HideEnumerator = Hide();
            StartCoroutine(HideEnumerator);
        }

        private const float MAX_ALPHA = .95f;
        private const float WAIT_SECONDS = 2;
        private const float HIDE_SECONDS = 1;
        private const float ALPHA_SCALER = MAX_ALPHA / HIDE_SECONDS;
        protected virtual IEnumerator HideEnumerator { get; set; }
        public virtual IEnumerator Hide()
        {
            Group.alpha = MAX_ALPHA;
            Group.interactable = true;
            Group.blocksRaycasts = true;

            yield return new WaitForSeconds(WAIT_SECONDS);
            while (Group.alpha > 0) {
                Group.alpha -= Time.deltaTime * ALPHA_SCALER;
                yield return null;
            }
            Group.interactable = false;
            Group.blocksRaycasts = false;
        }

        public virtual void OnDisable() => Group.alpha = 0;
    }
}
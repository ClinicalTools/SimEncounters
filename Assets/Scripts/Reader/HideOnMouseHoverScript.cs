using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HideOnMouseHoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {
	
	/**
	 * This class hides objects with a canvas group when the mouse is hovered over them
	 */

	public void OnPointerEnter(PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().alpha = 0;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().alpha = 1;
	}
}

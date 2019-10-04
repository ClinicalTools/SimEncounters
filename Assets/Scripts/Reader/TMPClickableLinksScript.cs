using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TMPClickableLinksScript : MonoBehaviour, IPointerClickHandler {

	private TextMeshProUGUI tmp;
	private TextMeshProUGUI parent = null;

	// Use this for initialization
	void Awake () {
		tmp = gameObject.GetComponent<TextMeshProUGUI>();
		if (tmp.isLinkedTextComponent) {
			foreach(TextMeshProUGUI ugui in GetComponentInParent<ReaderEntryManagerScript>().GetComponentsInChildren<TextMeshProUGUI>()) {
				if (!ugui.isLinkedTextComponent) {
					parent = ugui;
					break;
				}
			}
		}
	}

	void Start()
	{
		StringBuilder s = new StringBuilder();
		int excessLinkChars = "<link=''></link>".Length; //This is used for calculating where to put the closing underline bracket
		int endIdx;
		TMP_LinkInfo link;
		//foreach (TMP_LinkInfo link in tmp.textInfo.linkInfo) {
		for(int i = tmp.textInfo.linkCount - 1; i >= 0; i--) {
			link = tmp.textInfo.linkInfo[i];
			if (parent == null) {
				s = s.Insert(0, tmp.text);
			} else {
				s = s.Insert(0, parent.text);
			}

			//s = s.Insert(0, link.textComponent.text);
			endIdx = link.linkIdFirstCharacterIndex - "<link='".Length + link.linkIdLength + link.linkTextLength + excessLinkChars;
			s = s.Insert(endIdx, "</u>");
			s = s.Insert(link.linkIdFirstCharacterIndex - "<link='".Length, "<u>");

			if (parent == null) {
				tmp.text = s.ToString();
			} else {
				parent.text = s.ToString();
			}
			s.Clear();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		int linkIdx = TMP_TextUtilities.FindIntersectingLink(tmp, Input.mousePosition, GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera);
		if (linkIdx != -1) {
			Application.OpenURL(tmp.textInfo.linkInfo[linkIdx].GetLinkID());
		}
	}
}

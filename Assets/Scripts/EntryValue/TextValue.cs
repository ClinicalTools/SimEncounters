using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EntryValue
{
	public class TextValue : DeprecatedEntryValue<string>
	{
		public TMPro.TextMeshProUGUI textObj;

		public override void SetValue(string text)
		{
			text = UnEscape(text);
			if (!isDeprecated) {
				//Primary Value
				textObj.text = text;
			} else {
				//Any deprecated value
				SetDeprecatedValue(text);
			}
		}

		public override string GetValue()
		{
			if (!isDeprecated) {
				//Primary Value
				return Escape(textObj.text);
			} else {
				//Any deprecated value
				return Escape(GetDeprecatedValue());
			}
		}

		public override bool MatchConditions(GameObject obj)
		{
			if (obj == null) return false;

			//Check primary value
			if (obj.GetComponent<TMPro.TextMeshProUGUI>() != null) return true;

			//Check deprecated
			return CheckDeprecated(obj);
		}

#if UNITY_EDITOR
		private void OnEnable()
		{
			if (textObj == null) {
				textObj = GetComponent<TMPro.TextMeshProUGUI>();
			}

			if (textObj == null && CheckDeprecated(gameObject)) {
				deprecatedObject = gameObject;
			}
		}
#endif	

		protected override void InstantiateDeprecated()
		{
			dTypes = new DeprecatedType<string>[] {
				new DeprecatedType<string>(
					typeof(Text),					//Type
					(t => (t as Text).text),		//Getter
					((u, val) => {					//Setter
						(u as Text).text = val;
					})),
				new DeprecatedType<string>(
					typeof(TMPro.TextMeshProUGUI),	//Type
					(t => (t as Text).text),		//Getter
					((u, val) => {					//Setter
						(u as Text).text = val;
					})),
			};
		}

		/*
		protected override bool CheckDeprecated(GameObject obj)
		{
			if (dTypes == null) {
				InstantiateDeprecated();
			}
			int i = 0;
			foreach(DeprecatedType<string> type in dTypes) {
				if (type.MatchConditions(obj)) {
					deprecatedIdx = i;
					isDeprecated = true;
					break;
				}
				i++;
			}

			return isDeprecated;
		}

		
		private void SetDeprecatedValue(string text)
		{
			if (dTypes == null) InstantiateDeprecated();

			dTypes[deprecatedIdx].SetValue(
				deprecatedObject.GetComponent(dTypes[deprecatedIdx].objectType),
				text);
		}

		private string GetDeprecatedValue()
		{
			if (dTypes == null) InstantiateDeprecated();

			return dTypes[deprecatedIdx].GetValue(
				deprecatedObject.GetComponent(dTypes[deprecatedIdx].objectType));
		}*/
	}
}
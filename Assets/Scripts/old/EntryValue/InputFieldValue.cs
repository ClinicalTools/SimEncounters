using UnityEngine;
using UnityEngine.UI;

namespace EntryValue
{
	public class InputFieldValue : DeprecatedEntryValue<string>
	{
		public TMPro.TMP_InputField inputObj;

		//Uses text escaping

		public override void SetValue(string text)
		{
			text = UnEscape(text);
			if (!isDeprecated) {
				inputObj.text = text;
				inputObj.onEndEdit.Invoke(text);
			} else {
				SetDeprecatedValue(text);
			}
		}

		public override string GetValue()
		{
			if (!isDeprecated) {
				return Escape(inputObj.text);
			} else {
				return Escape(GetDeprecatedValue());
			}
		}

		public override bool MatchConditions(GameObject obj)
		{
			if (obj == null) return false;
			if (obj.GetComponent<TMPro.TMP_InputField>() != null) return true;
			return CheckDeprecated(obj);
		}

#if UNITY_EDITOR
		private void OnEnable()
		{
			if (inputObj == null) {
				inputObj = GetComponent<TMPro.TMP_InputField>();
			}

			if (inputObj == null && CheckDeprecated(gameObject)) {
				deprecatedObject = gameObject;
			}
		}
#endif

		//Deprecated Types
		protected override void InstantiateDeprecated()
		{
			//UnityWebRequest.EscapeURL(child.gameObject.GetComponent<InputField>().text);
			dTypes = new DeprecatedType<string>[] {
				new DeprecatedType<string>(
					//Type
					typeof(InputField),
					//Getter
					(t => {
						return (t as InputField).text;
					}),
					//Setter
					((u, val) => {
						(u as InputField).text = val;
						(u as InputField).onEndEdit.Invoke(val);
					}))
			};
		}

		/*
		protected override bool CheckDeprecated(GameObject obj)
		{
			if (dTypes == null) {
				InstantiateDeprecated();
			}
			int i = 0;
			foreach (DeprecatedType<string> type in dTypes) {
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
		}
		*/
	}
}
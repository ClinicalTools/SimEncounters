using UnityEngine;
using UnityEngine.UI;

namespace EntryValue
{
	public class DropdownValue : DeprecatedEntryValue<string>
	{
		public TMPro.TMP_Dropdown dropdownObj;

		/// <summary>
		/// Sets the value of the dropdown
		/// </summary>
		/// <param name="text">The value text</param>
		public override void SetValue(string text)
		{
			text = UnEscape(text);
			if (!isDeprecated) {
				//Primary value
				int indexValue = 0;
				foreach (TMPro.TMP_Dropdown.OptionData myOptionData in dropdownObj.options) {
					if (myOptionData.text.Equals(text)) {
						break;
					}
					indexValue++;
				}
				dropdownObj.value = indexValue;
			} else {
				//Any deprecated value
				SetDeprecatedValue(text);
			}
		}

		/// <summary>
		/// Get the value for this dropdown
		/// </summary>
		/// <returns>The text of the dropdown</returns>
		public override string GetValue()
		{
			//TODO CHECK FOR ERROR WITH CAPTION TEXT NOT BEING INITIATED
			if (!isDeprecated) {
				//Primary value
				return Escape(dropdownObj.captionText.text);
			} else {
				//Any deprecated value
				return Escape(GetDeprecatedValue());
			}
		}

		public override bool MatchConditions(GameObject obj)
		{
			if (obj == null) return false;
			if (obj.GetComponent<TMPro.TMP_Dropdown>() != null) return true;
			return CheckDeprecated(obj);
		}

#if UNITY_EDITOR
		private void OnEnable()
		{
			if (dropdownObj == null) {
				dropdownObj = GetComponent<TMPro.TMP_Dropdown>();
			}

			if (dropdownObj == null && CheckDeprecated(gameObject)) {
				deprecatedObject = gameObject;
			}
		}
#endif

		protected override void InstantiateDeprecated()
		{
			//TODO CHECK FOR ERROR WITH CAPTION TEXT NOT BEING INITIATED
			dTypes = new DeprecatedType<string>[] {
				new DeprecatedType<string>(
					typeof(Dropdown),							//Type
					(t => (t as Dropdown).captionText.text),	//Getter
					((u, val) => {								//Setter
						int indexValue = 0;
						foreach (Dropdown.OptionData myOptionData in (u as Dropdown).options) {
							if (myOptionData.text.Equals(val)) {
								break;
							}
							indexValue++;
						}
						(u as Dropdown).value = indexValue;
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
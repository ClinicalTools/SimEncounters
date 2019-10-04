using UnityEngine;

namespace EntryValue
{
	public class ToggleValue : EntryValue
	{
#if UNITY_EDITOR
		private void OnEnable()
		{
			if (toggleObj == null) {
				toggleObj = GetComponent<UnityEngine.UI.Toggle>();
			}
		}
#endif

		public UnityEngine.UI.Toggle toggleObj;

		public override void SetValue(string text)
		{
			if (text == "" || (text == null)) {
				toggleObj.isOn = false;
			} else {
				toggleObj.isOn = bool.Parse(text);
			}
		}

		public override string GetValue()
		{
			return toggleObj.isOn.ToString();
		}

		public override string GetValueWithXML()
		{
			if (toggleObj.isOn) {
				return base.GetValueWithXML();
			}
			return "";
		}

		public override bool MatchConditions(GameObject obj)
		{
			return obj.GetComponent<UnityEngine.UI.Toggle>() != null;
		}
	}
}
using UnityEngine;

namespace EntryValue
{
	public class PinValue : EntryValue
	{
		public Transform pinObj;
		public bool nested;

		[SerializeField]
		private Transform labEntryParent;
		[SerializeField]
		private PinType pinType;

		private string pinKey = "";
		private string oldPinKey = "";
		private string tabName;

		public enum PinType
		{
			None,
			Dialogue,
			Quiz
		}

		private void Start()
		{
		}
		
		private void InitKeys()
		{
		}

		public override string GetValue()
		{
			return "";
		}

		public override void SetValue(string text)
		{
			//No way to set a value for pins.
			//This is handled by PinOnClickScript for the button call
			//and the dictionaries hold previously saved data
			return;
		}

		public override string GetValueWithXML()
		{
			return "";
		}

		public override string GetTag()
		{
			return pinType.ToString() + "Pin";
		}

		public override bool MatchConditions(GameObject obj)
		{
			return 
				obj.name.Equals("DialoguePin") ||
				obj.name.Equals("QuizPin");
		}

#if UNITY_EDITOR
		private void OnEnable()
		{
			if (pinObj == null) pinObj = transform;
			if (labEntryParent == null) {
				Transform tempChild = pinObj;
				while (labEntryParent == null && tempChild != null) {
					if (tempChild.name.StartsWith("LabEntry:")) {
						if (labEntryParent == null) {
							labEntryParent = tempChild;
						} else {
							nested = true;
							break;
						}
					}
					tempChild = tempChild.parent;
				}
			}
			if (pinType == PinType.None && pinObj != null) {
				switch(pinObj.name) {
					case "DialoguePin":
						pinType = PinType.Dialogue;
						break;
					case "QuizPin":
						pinType = PinType.Quiz;
						break;
					default:
						pinType = PinType.None;
						break;
				}
			}
			if (labEntryParent == null) {
				Debug.LogWarning("Don't forget to assign the Lab Entry Parent and Nested variables!", gameObject);
			}
		}
#endif
	}
}

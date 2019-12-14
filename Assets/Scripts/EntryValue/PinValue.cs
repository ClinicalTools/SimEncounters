using ClinicalTools.SimEncountersOld;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

		WriterHandler Ds => WriterHandler.WriterInstance;
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
			tabName = Ds?.transform.GetComponent<TabManager>().getCurrentTab() + "Tab";
		}
		
		private void InitKeys()
		{
			pinKey = "";
			oldPinKey = "";
			Transform tChild = pinObj;
			while (tChild.parent) {
				if (tChild.name.StartsWith("LabEntry")) {
					pinKey = "LabEntry: " + tChild.GetSiblingIndex() + pinKey;
					oldPinKey = tChild.name + oldPinKey;
				} else if (tabName.Length == 0 && tChild.name.EndsWith("Tab")) {
					//We need to do this because TabManager.getCurrentTab()
					//Doesn't get the value we need. It can be updated
					tabName = tChild.name;
				}
				tChild = tChild.parent;
			}

			string sectionAndTab =
				Ds.transform.GetComponent<TabManager>().GetCurrentSectionKey() +
				"/" + tabName + "/";

			pinKey = sectionAndTab + pinKey;
			oldPinKey = sectionAndTab + oldPinKey;
		}

		public override string GetValue()
		{
			//Get the new and old keys for the pin
			if (pinKey == "" && oldPinKey == "") {
				InitKeys();
			}


			//Save/Load data
			switch (pinType) {
				case PinType.Dialogue:
					if (Ds.GetDialogues().ContainsKey(oldPinKey)) {
						//Get dialogue xml
						string dialogueXML = Ds.GetDialogues()[oldPinKey];

						//Remove the old dialogue if needed
						if (!Ds.GetDialogues().ContainsKey(pinKey)) {
							Ds.GetDialogues().Remove(oldPinKey);
						}
						Ds.CorrectlyOrderedDialogues.Add(pinKey, dialogueXML.Replace(oldPinKey, pinKey));

						return dialogueXML.Replace(oldPinKey, pinKey);
					}
					break;
				case PinType.Quiz:
					if (Ds.GetQuizes().ContainsKey(oldPinKey)) {
						string quizXML = Ds.GetQuizes()[oldPinKey]; //Quiz data


						//If there was no quiz where this quiz was moved to
						if (!Ds.GetQuizes().ContainsKey(pinKey)) {
							Ds.GetQuizes().Remove(oldPinKey);
						}
						if (!Ds.CorrectlyOrderedQuizes.ContainsKey(pinKey)) {
							Ds.CorrectlyOrderedQuizes.Add(pinKey, quizXML.Replace(oldPinKey, pinKey));
						} else {
							Ds.CorrectlyOrderedQuizes[pinKey] = quizXML.Replace(oldPinKey, pinKey);
						}
						return quizXML.Replace(oldPinKey, pinKey);
					}
					break;
			}

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
			if (pinKey == "" && oldPinKey == "") {
				InitKeys();
			}

			switch (pinType) {
				case PinType.Dialogue:
					if (Ds.GetDialogues().ContainsKey(oldPinKey)) {
						return base.GetValueWithXML();
					}
					break;
				case PinType.Quiz:
					if (Ds.GetQuizes().ContainsKey(oldPinKey)) {
						return base.GetValueWithXML();
					}
					break;
			}

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

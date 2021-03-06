﻿using System.Collections;
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

		DataScript ds => DataScript.instance;
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
			tabName = ds?.transform.GetComponent<TabManager>().getCurrentTab() + "Tab";
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
				ds.transform.GetComponent<TabManager>().getCurrentSection() +
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
					if (ds.GetDialogues().ContainsKey(oldPinKey)) {
						//Get dialogue xml
						string dialogueXML = ds.GetDialogues()[oldPinKey];

						//Remove the old dialogue if needed
						if (!ds.GetDialogues().ContainsKey(pinKey)) {
							ds.GetDialogues().Remove(oldPinKey);
						}
						ds.correctlyOrderedDialogues.Add(pinKey, dialogueXML.Replace(oldPinKey, pinKey));

						return dialogueXML.Replace(oldPinKey, pinKey);
					}
					break;
				case PinType.Quiz:
					if (ds.GetQuizes().ContainsKey(oldPinKey)) {
						string quizXML = ds.GetQuizes()[oldPinKey]; //Quiz data


						//If there was no quiz where this quiz was moved to
						if (!ds.GetQuizes().ContainsKey(pinKey)) {
							ds.GetQuizes().Remove(oldPinKey);
						}
						if (!ds.correctlyOrderedQuizes.ContainsKey(pinKey)) {
							ds.correctlyOrderedQuizes.Add(pinKey, quizXML.Replace(oldPinKey, pinKey));
						} else {
							ds.correctlyOrderedQuizes[pinKey] = quizXML.Replace(oldPinKey, pinKey);
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
					if (ds.GetDialogues().ContainsKey(oldPinKey)) {
						return base.GetValueWithXML();
					}
					break;
				case PinType.Quiz:
					if (ds.GetQuizes().ContainsKey(oldPinKey)) {
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

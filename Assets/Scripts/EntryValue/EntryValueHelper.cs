using UnityEngine;
using System;

namespace EntryValue
{
	/// <summary>
	/// Editor helper class to auto-assign the correct value script
	/// </summary>
	[ExecuteInEditMode]
	public class EntryValueHelper : MonoBehaviour
	{
		private System.Type[] potentialValueTypes = new System.Type[] {
			typeof(DateValue),
			typeof(InputFieldValue),
			typeof(TextValue),
			typeof(ImageValue),
			typeof(DropdownValue),
			typeof(ToggleValue),
			typeof(PinValue)
		};

#if UNITY_EDITOR
		private void OnEnable()
		{
			//Itterate over types in potentialValueTypes and check against
			//each one to determine which one is the best fit for the current
			//gameobject.

			/*
			//Hide				  Override
			EntryValue ev = new InputFieldValue();

			InputFieldValue iv = new InputFieldValue();

			//To override, class must be abstract or virtual
			//New can hide any method, but the base is still accessable
			//through casting

			//For case 1
			if (true || ev.MatchConditions(null)) {
				//Casted as ev, only have base access
				//new --> ev
				//override --> iv
				ev.MatchConditions(null);
			}

			//For case 2
			if (true || iv.MatchConditions(null)) {
				//As iv, have access to both

				//Points to iv
				iv.MatchConditions(null);

				//Points to base
				((EntryValue)iv).MatchConditions(null);

				//iv exclusive
				iv.MatchConditions2(null);
			}
			*/

			if (GetComponent<EntryValue>() != null) {
				DestroyThis();
				return;
			}

			EntryValue eVal;
			foreach (System.Type type in potentialValueTypes) {
				eVal = gameObject.AddComponent(type) as EntryValue;
				//eVal = (EntryValue)Activator.CreateInstance(type);
				if (eVal.MatchConditions(gameObject)) {
					//gameObject.AddComponent(type);
					DestroyThis();
					return;
				}
				DestroyImmediate(eVal);
			}
		}

		private void DestroyThis()
		{
			DestroyImmediate(GetComponent<EntryValueHelper>());
		}
#endif
	}
}
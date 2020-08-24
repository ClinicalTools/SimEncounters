using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EntryValue
{
	public abstract class DeprecatedEntryValue<T> : EntryValue
	{
		public bool isDeprecated;
		public GameObject deprecatedObject;
		protected int deprecatedIdx;

		protected DeprecatedType<T>[] dTypes;

		protected abstract void InstantiateDeprecated();

		/// <summary>
		/// Checks if there are any deprecated objects which apply to the
		/// specified GameObject
		/// </summary>
		/// <param name="obj">The GameObject to check</param>
		/// <returns>True if there's a match</returns>
		protected virtual bool CheckDeprecated(GameObject obj)
		{
			InstantiateDeprecated();
			int i = 0;
			foreach (DeprecatedType<T> type in dTypes) {
				if (type.MatchConditions(obj)) {
					deprecatedIdx = i;
					isDeprecated = true;
					break;
				}
				i++;
			}

			return isDeprecated;
		}

		protected virtual void SetDeprecatedValue(T text)
		{
			if (dTypes == null) InstantiateDeprecated();

			dTypes[deprecatedIdx].SetValue(
				deprecatedObject.GetComponent(dTypes[deprecatedIdx].objectType),
				text);
		}

		protected virtual T GetDeprecatedValue()
		{
			if (dTypes == null) InstantiateDeprecated();

			return dTypes[deprecatedIdx].GetValue(
				deprecatedObject.GetComponent(dTypes[deprecatedIdx].objectType));
		}
	}
}
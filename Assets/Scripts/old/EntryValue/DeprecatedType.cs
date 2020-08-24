using UnityEngine;

namespace EntryValue
{
	public class DeprecatedType<T>
	{
		public System.Type objectType;
		//public delegate T ReturnVal2<U>(U obj) where U : UnityEngine.Object;
		public delegate T GetVal(UnityEngine.Object obj);
		public delegate void SetVal(UnityEngine.Object obj, T val);

		private readonly GetVal getVal;
		private readonly SetVal setVal;

		public DeprecatedType(System.Type inputType, GetVal getVal, SetVal setVal)
		{
			objectType = inputType;
			this.getVal = getVal;
			this.setVal = setVal;
		}

		/// <summary>
		/// Get the value from a designated object
		/// </summary>
		/// <param name="obj">Any script / Component to get data from</param>
		/// <returns>The value T</returns>
		public T GetValue(UnityEngine.Object obj)
		{
			return getVal(obj);
		}

		/// <summary>
		/// Set the value for a designated object
		/// </summary>
		/// <param name="obj">Any script / Component to get data from</param>
		/// <param name="val">The value to set</param>
		public void SetValue(UnityEngine.Object obj, T val)
		{
			setVal(obj, val);
		}

		/// <summary>
		/// Whether or not the specified GameObject contains the
		/// component this DeprecatedType was instantiated with
		/// </summary>
		/// <param name="obj">GameObject to check components for</param>
		/// <returns>True if the component is found</returns>
		public bool MatchConditions(GameObject obj)
		{
			//If a condition needs to be more complicated than a component check
			//Then this could require some reworking
			return obj.GetComponent(objectType) != null;
		}
	}
}

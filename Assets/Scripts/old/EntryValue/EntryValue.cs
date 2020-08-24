using UnityEditor;
using UnityEngine;

namespace EntryValue
{
	[ExecuteInEditMode]
	public abstract class EntryValue : MonoBehaviour
	{
		[SerializeField]
		private string valueTag = null;
		
		/// <summary>
		/// Sets the value of this entry
		/// </summary>
		/// <param name="text">The XML describing the value to set</param>
		public abstract void SetValue(string text);

		/// <summary>
		/// Gets the value of this entry
		/// </summary>
		/// <returns>The value with no XML formatting</returns>
		public abstract string GetValue();

		public virtual string GetValueWithXML()
		{
			return "<" + GetTag() + ">" + GetValue() + "</" + GetTag() + ">";
		}

		public virtual string GetTag()
		{
			return valueTag;
		}

		/// <summary>
		/// Whether or not this entry applies to the specified GameObject.
		/// This is mostly used for development
		/// </summary>
		/// <param name="obj">The GameObject to check</param>
		/// <returns>True if a match</returns>
		public abstract bool MatchConditions(GameObject obj);

		[ContextMenu("TestingWoo")]
		public void Test()
		{
			print(GetValueWithXML());
			SetValue("Hello");
			print(GetValueWithXML());
		}

		protected string UnEscape(string text)
		{
			return UnityEngine.Networking.UnityWebRequest.UnEscapeURL(text);
		}

		protected string Escape(string text)
		{
			return UnityEngine.Networking.UnityWebRequest.EscapeURL(text);
		}
	}
}
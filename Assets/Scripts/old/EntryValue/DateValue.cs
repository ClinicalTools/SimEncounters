using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntryValue
{
	[RequireComponent(typeof(DateFieldEnforcer))]
	public class DateValue : InputFieldValue
	{
		[SerializeField]
		public Date date;

		/// <summary>
		/// Called from a dropdown with Days, Weeks, Months, and Year
		/// </summary>
		/// <param name="val"></param>
		public void SetTimeSpanMultiplier(int val)
		{
			switch(val) {
				case 0:
					val = (int)Date.TimeSpanMultiplier.Days;
					break;
				case 1:
					val = (int)Date.TimeSpanMultiplier.Weeks;
					break;
				case 2:
					val = (int)Date.TimeSpanMultiplier.Months;
					break;
				case 3:
					val = (int)Date.TimeSpanMultiplier.Years;
					break;
			}
			date.SetTimeSpanMultiplier(val);
		}

		public void SetTimeSpanMultiplier(string val)
		{
			date.SetTimeSpanMultiplier(val);
		}

		public void SetTimeDirection(int i)
		{
			date.timeDirection = (Date.TimeDirection)i;
		}

		public override string GetValue()
		{
			//return base.GetValue();
			return date.ToString();
		}

		public override bool MatchConditions(GameObject obj)
		{
			return
				obj.GetComponent<DateFieldEnforcer>() &&
				base.MatchConditions(obj);
		}

		public override void SetValue(string text)
		{
			date = Date.Parse(text);
			base.SetValue(date.GetTimeText());
		}
	}
}

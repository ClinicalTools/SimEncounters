using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntryValue
{
	[System.Serializable]
	public class Date
	{
		private static class TimeFormats
		{
			private static string[] formats = new string[] {
				"MM-dd-yy",
				"MM-dd-yyyy",
				"dd-MM-yy",
				"dd-MM-yyyy",
			};

			/// <summary>
			/// Month, Day, Year ('19)
			/// </summary>
			public static string mmddyy = formats[0];

			/// <summary>
			/// Month, Day, Year (2019)
			/// </summary>
			public static string mmddyyyy = formats[1];

			/// <summary>
			/// Day, Month, Year ('19)
			/// </summary>
			public static string ddmmyy = formats[2];

			/// <summary>
			/// Day, Month, Year (2019)
			/// </summary>
			public static string ddmmyyyy = formats[3];

			/// <summary>
			/// Matches the string to a defined date format
			/// </summary>
			/// <param name="s">The string to check</param>
			/// <param name="contains">Whether to check for contains vs equals</param>
			/// <returns>The matching format</returns>
			public static string MatchDirection(string s, bool contains = false)
			{
				if (contains) {
					foreach (string format in formats) {
						if (s.Contains(format)) return format;
					}
				} else {
					foreach (string format in formats) {
						if (format.Equals(s)) return format;
					}
				}
				throw new System.Exception("Format not accepted: " + s);
			}
		}

		/**
		 * Specify format with variable name and representing string
		 * Ideally, have the formats be available for dropdowns in editor
		 * and in game. Check input fields and autocomplete based on format
		 * 
		 * TimeSpan represents durations of time so would be better here
		 * DateTimeOffset represents a datetime + offset from UTC, good for hard time
		 * 
		 * Date format:
		 *   Divider char
		 *   (Future/Past/Hard date)
		 *   Display Style
		 */

		//Visual display only
		public char divider = '/';
		[SerializeField]
		private string displayStyle = TimeFormats.mmddyyyy;

		public TimeDirection timeDirection;

		public System.DateTimeOffset offsetTime;
		public System.TimeSpan timeSpan;

		public enum TimeDirection
		{
			Hard = 1,
			Past = 0,
			Future = 2,
		}

		public Date(char divider, string displayStyle, TimeDirection timing, string time)
		{
			this.divider = divider;
			this.displayStyle = displayStyle;
			timeDirection = timing;
			SetTime(time);
		}

		public Date() { }

		public string GetFormat()
		{
			return displayStyle.Replace('-', divider);
		}

		public void SetTime(string s)
		{
			switch (timeDirection) {
				case TimeDirection.Hard:
					//offsetTime
					if (!System.DateTimeOffset.TryParse(s, out offsetTime)) {
						throw new System.Exception("Could not parse date value " + s);
					}
					break;
				default:
					//Timespan
					float val = float.Parse(s);
					val *= (int)spanMultiplier;
					int days = (int)val;
					if (!System.TimeSpan.TryParse(days.ToString(), out timeSpan)) {
						throw new System.Exception("Could not parse timespan value " + s);
					}
					break;
			}
		}

		private TimeSpanMultiplier spanMultiplier = TimeSpanMultiplier.Days;
		public enum TimeSpanMultiplier
		{
			Days = 1,
			Weeks = 7,
			Months = 30,
			Years = 365
		}

		public void SetTimeSpanMultiplier(int val)
		{
			spanMultiplier = (TimeSpanMultiplier)val;
			//Adjust displayed value to reflect change i.e. 14 days --> 2 weeks
		}

		public void SetTimeSpanMultiplier(string val)
		{
			spanMultiplier = (TimeSpanMultiplier) System.Enum.Parse(typeof(TimeSpanMultiplier), val);
		}

		/// <summary>
		/// Returns the DateValue as a value ready for XML.
		/// </summary>
		/// <returns>Divider, (int)TimeDirection, display style, time</returns>
		public override string ToString()
		{
			string returnText = "";
			returnText += divider.ToString();
			returnText += (int)timeDirection;
			returnText += displayStyle;
			returnText += GetTimeText();

			return returnText;
		}

		public static Date Parse(string s)
		{
			Date d = new Date();
			d.divider = s[0];
			d.timeDirection = (TimeDirection)int.Parse(s[1].ToString());
			int splitPoint = System.Text.RegularExpressions.Regex.Split(s.Substring(2), "[0-9]")[0].Length;
			d.displayStyle = TimeFormats.MatchDirection(s.Substring(2, splitPoint));
			string timeString = s.Substring(d.displayStyle.Length + 2);
			d.SetTime(timeString);

			return d;
		}

		public string GetTimeText()
		{
			if (timeDirection == TimeDirection.Hard) {
				return string.Format("{0:" + GetFormat() + "}", offsetTime);
			} else {
				return timeSpan.Days.ToString();
			}
		}
	}
}
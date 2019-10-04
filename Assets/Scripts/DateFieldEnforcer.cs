using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[ExecuteInEditMode]
public class DateFieldEnforcer : MonoBehaviour
{
	TMPro.TMP_InputField input;

	EntryValue.Date date;

	// Start is called before the first frame update
	public string text;
	public string parseText;
	public string formatText;

	private static string specialFormatCharacters =
		"dfFghHKmMstyz:/";

	void Start()
    {
		input = GetComponent<TMPro.TMP_InputField>();
		date = GetComponent<EntryValue.DateValue>().date;

		//print(ValidateDate("12/4/")); //MM/dd

		//Test("12/4/2019");
		//Test("12/04/2019");
		//Test("13/04/2019");
		//Test("1/2/2020");

		input.onEndEdit.AddListener(UpdateDate);
		//input.onEndEdit.AddListener(ValidateDate);
		//input.onValueChanged.AddListener(ValidateDate);
	}

	public void UpdateDate(string s)
	{
		date.SetTime(s);
	}

	[ContextMenu("Test")]
	private void Test3()
	{
		if (DateTimeOffset.TryParseExact(text, parseText, CultureInfo.CurrentCulture, DateTimeStyles.None, out offset))
			print(offset.Month + ", " + offset.Day);

		DateTime dt = new DateTime();
		dt.AddMonths(int.Parse(text));
		print(String.Format(formatText, dt));
	}

	private void Test2(string s, string format)
	{
		DateTimeFormatInfo info = CultureInfo.InvariantCulture.DateTimeFormat;
		info.ShortDatePattern = date.GetFormat();
		if (!System.DateTimeOffset.TryParseExact(s, format,
						CultureInfo.InvariantCulture.DateTimeFormat,
						DateTimeStyles.None, out offset)) {
			print("Failed " + s + ": " + format);
		}
	}

	private void Test(string test)
	{
		string dateInput = "";
		foreach(char c in test) {
			dateInput += c;
			dateInput = ValidateDate(dateInput);
			print(dateInput);
		}
	}

	System.DateTimeOffset offset;
	System.TimeSpan timespan;

	private string ValidateDate(string s)
	{
		//Add in dividers automatically
		//Autocorrects things, like 1 => 01
		//Use date.DisplayStyle to format
		/**
		 * 12/4/2019
		 * 1
		 * 12
		 * 12/
		 * 12/4
		 * 12/4/
		 * 12/4/2
		 * 12/4/20
		 * 12/4/201
		 * 12/4/2019
		 */

		string displayS = date.GetFormat().Substring(0, s.Length);
		if (displayS.Length == 1 && specialFormatCharacters.Contains(displayS)) {
			displayS = "%" + displayS;
		}

		switch(date.timeDirection) {
			case EntryValue.Date.TimeDirection.Hard:
				if (!System.DateTimeOffset.TryParseExact(
						s,
						displayS,
						CultureInfo.InvariantCulture.DateTimeFormat,
						DateTimeStyles.None,
						out offset)) {
					//Couldn't parse, remove last char
					//input.text = s.Remove(s.Length - 1);
					return s.Remove(s.Length - 1);

				}
				break;
			default:
				if (!System.TimeSpan.TryParseExact(
						s,
						displayS,
						CultureInfo.InvariantCulture.DateTimeFormat,
						TimeSpanStyles.None,
						out timespan)) {
					//Couldn't parse, remove last char
					//input.text = s.Remove(s.Length - 1);
					return s.Remove(s.Length - 1);
				}
				break;
		}
		return s;
	}
}

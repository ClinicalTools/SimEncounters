using System;
using TMPro;
using UnityEngine;

public class UpdateAge : MonoBehaviour {

    public TMP_InputField Month, Day, Year, Age;

    // Update is called once per frame
    public void ChangedAge()
    {
        if (!string.IsNullOrEmpty(Age.text) && !string.IsNullOrEmpty(Day.text) && !string.IsNullOrEmpty(Month.text)) {
            var month = int.Parse(Month.text);
            var day = int.Parse(Day.text);
            var age = int.Parse(Age.text);

            if (month < DateTime.Today.Month || (month == DateTime.Today.Month && day < DateTime.Today.Day)) {
                Year.text = $"{DateTime.Today.Year - age}";
            } else {
                Year.text = $"{DateTime.Today.Year - (age + 1)}";
            }
        }
    }

    public void ChangedBirthday()
    {
        if (!string.IsNullOrEmpty(Year.text) && !string.IsNullOrEmpty(Day.text) && !string.IsNullOrEmpty(Month.text)) {
            var month = int.Parse(Month.text);
            var day = int.Parse(Day.text);
            var year = int.Parse(Year.text);

            if (month < DateTime.Today.Month || (month == DateTime.Today.Month && day < DateTime.Today.Day)) {
                Age.text = $"{DateTime.Today.Year - year}";
            } else {
                Age.text = $"{DateTime.Today.Year - (year + 1)}";
            }
        }
    }
}
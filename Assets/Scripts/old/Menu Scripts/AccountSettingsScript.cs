using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccountSettingsScript : MonoBehaviour {

	public GameObject panel;

	public TextMeshProUGUI accountId;
	public TextMeshProUGUI email;

	public GameObject firstTimeNotice;

	public TMP_InputField firstName;
	public TMP_InputField lastName;
	public TMP_Dropdown title;
	public TMP_InputField currentPassword;
	public TMP_InputField newPassword;
	public TMP_InputField confirmPassword;

	public LoginManager accountManager;

	public void Submit()
	{
		accountManager.UpdateAccountInfo();
	}

	public void PopulateFields()
	{
		accountId.text = GlobalData.username;
		email.text = GlobalData.email;

		firstName.text = GlobalData.userFirstName;
		lastName.text = GlobalData.userLastName;
		title.value = title.options.FindIndex(option => option.text.Equals(GlobalData.userTitle));

		currentPassword.text = "";
		newPassword.text = "";
		confirmPassword.text = "";
	}

}

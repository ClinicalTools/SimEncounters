using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegistrationScript : MonoBehaviour {

	public TMP_InputField UName;
    public TMP_InputField PWord;
    public TMP_InputField PWordConfirmation;
    public TMP_InputField EText;

	public LoginManager lm;
	public ServerControls server;

	private void Start()
	{
		if (lm == null) {
			lm = GetComponent<LoginManager>();
		}

		if (server == null) {
			server = GetComponent<ServerControls>();
		}
	}

	/**
	 * @Logged out
	 * Registers an account
	 */
	public void Register()
	{
		GlobalData.username = UName.text;
		GlobalData.password = PWord.text;
		Debug.Log(GlobalData.email);
		GlobalData.email = EText.text;
		Debug.Log(GlobalData.email);

        if (GlobalData.username.Equals("") || GlobalData.password.Equals("") || GlobalData.email.Equals(""))
        {
            print("Please enter credentials");
            lm.ShowMessage("Please enter credentials", true);
            return;
        }

        if (GlobalData.password != PWordConfirmation.text)
        {
            lm.ShowMessage("Passwords do not match", true);
            return;
        }

		if (!Regex.IsMatch(GlobalData.email, "^(.)+[@](.)+[.](.)+$")) {
			lm.ShowMessage("Email not valid", true);
			print("Email not valid");
			return;
		}

		StartCoroutine(server.Register());
	}

	/**
	 * @Logged out
	 * Resends the account activation email
	 */
	public void ResendActivationEmail()
	{
		GlobalData.username = UName.text;
		GlobalData.password = PWord.text;
		GlobalData.email = EText.text;
		StartCoroutine(server.ResendActivationEmail());
	}

	public void ResendActivationEmail(Text email)
	{
		GlobalData.email = email.text;
		StartCoroutine(server.ResendActivationEmail());
	}

	/**
	 * @Logged out
	 * Call to send an email comtaining a remplacement password
	 */
	public void ForgotPassword()
	{
		GlobalData.username = UName.text;
		GlobalData.email = EText.text;
		StartCoroutine(server.ForgotPassword());
	}

    public void ResetFields()
    {
        UName.text = "";
        PWord.text = "";
        PWordConfirmation.text = "";
        EText.text = "";
}
}

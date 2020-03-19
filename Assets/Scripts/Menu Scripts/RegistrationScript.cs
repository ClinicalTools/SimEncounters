using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ClinicalTools.SimEncounters.MainMenu;
using ClinicalTools.SimEncounters;

public class RegistrationScript : MonoBehaviour
{

    [SerializeField] private MessageHandler messageHandler;
    public MessageHandler MessageHandler { get; set; }



    public TMP_InputField UName;
    public TMP_InputField PWord;
    public TMP_InputField PWordConfirmation;
    public TMP_InputField EText;

    private void Start()
    {
        MessageHandler = MessageHandler.Instance;
    }

    /**
	 * @Logged out
	 * Registers an account
	 */
    public void Register()
    {
        GlobalData.username = UName.text;
        GlobalData.password = PWord.text;
        GlobalData.email = EText.text;

        if (GlobalData.username.Equals("") || GlobalData.password.Equals("") || GlobalData.email.Equals("")) {
            MessageHandler.ShowMessage("Please enter credentials", true);
            return;
        }

        if (GlobalData.password != PWordConfirmation.text) {
            MessageHandler.ShowMessage("Passwords do not match", true);
            return;
        }

        if (!Regex.IsMatch(GlobalData.email, "^(.)+[@](.)+[.](.)+$")) {
            MessageHandler.ShowMessage("Email not valid", true);
            return;
        }

        var registerUser = new RegisterUser(new WebAddress(), MessageHandler);
        registerUser.Register(UName.text, PWord.text, EText.text);
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
        var resendEmail = new ResendEmail(new WebAddress(), MessageHandler);
        resendEmail.Resend(EText.text);
    }

    public void ResendActivationEmail(Text email)
    {
        GlobalData.email = email.text;
        var resendEmail = new ResendEmail(new WebAddress(), MessageHandler);
        resendEmail.Resend(email.text);
    }

    /**
	 * @Logged out
	 * Call to send an email comtaining a remplacement password
	 */
    public void ForgotPassword()
    {
        GlobalData.username = UName.text;
        GlobalData.email = EText.text;
        //StartCoroutine(server.ForgotPassword());
    }

    public void ResetFields()
    {
        UName.text = "";
        PWord.text = "";
        PWordConfirmation.text = "";
        EText.text = "";
    }
}

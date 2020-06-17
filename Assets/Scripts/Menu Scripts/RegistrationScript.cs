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
    public MessageHandler MessageHandler { get => messageHandler; set => messageHandler = value; }
    [SerializeField] private MessageHandler messageHandler;

    public TMP_InputField UName;
    public TMP_InputField PWord;
    public TMP_InputField PWordConfirmation;
    public TMP_InputField EText;

    private void Start()
    {
       // MessageHandler = MessageHandler.Instance;
    }

    /**
	 * @Logged out
	 * Registers an account
	 */
    public void Register()
    {
        if (string.IsNullOrWhiteSpace(UName.text) || string.IsNullOrWhiteSpace(PWord.text) || string.IsNullOrWhiteSpace(EText.text)) {
            //MessageHandler.ShowMessage("Please enter credentials", true);
            return;
        }

        if (PWord.text != PWordConfirmation.text) {
            //MessageHandler.ShowMessage("Passwords do not match", true);
            return;
        }

        if (!Regex.IsMatch(EText.text, "^(.)+[@](.)+[.](.)+$")) {
            //MessageHandler.ShowMessage("Email not valid", true);
            return;
        }

        //var registerUser = new RegisterUser(new UrlBuilder(), MessageHandler);
        //registerUser.Register(UName.text, PWord.text, EText.text);
    }

    /**
	 * @Logged out
	 * Resends the account activation email
	 */
    public void ResendActivationEmail()
    {
        if (string.IsNullOrWhiteSpace(EText.text)) {
            //MessageHandler.ShowMessage("Please enter your email", true);
            return;
        }
        //var resendEmail = new ResendEmail(new UrlBuilder(), MessageHandler);
        //resendEmail.Resend(EText.text);
    }
    
    /**
	 * @Logged out
	 * Call to send an email comtaining a remplacement password
	 */
    public void ForgotPassword(TMP_InputField userField)
    {
        if (string.IsNullOrWhiteSpace(userField.text) && string.IsNullOrWhiteSpace(userField.text)) {
            //MessageHandler.ShowMessage("Please enter your username or email", true);
            return;
        }

        /*
        var resetPassword = new ResetPassword(new UrlBuilder(), MessageHandler);
        if (Regex.IsMatch(userField.text, "^(.)+[@](.)+[.](.)+$"))
            resetPassword.Reset(userField.text, "");
        else
            resetPassword.Reset("", userField.text);*/
    }

    public void ResetFields()
    {
        UName.text = "";
        PWord.text = "";
        PWordConfirmation.text = "";
        EText.text = "";
    }
}

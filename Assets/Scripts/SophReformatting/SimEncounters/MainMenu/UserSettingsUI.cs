using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class UserSettingsUI : PopupUI
    {
        [SerializeField] private TextMeshProUGUI usernameLabel;
        public TextMeshProUGUI UsernameLabel { get => usernameLabel; set => usernameLabel = value; }

        [SerializeField] private TextMeshProUGUI emailLabel;
        public TextMeshProUGUI EmailLabel { get => emailLabel; set => emailLabel = value; }


        [SerializeField] private TMP_InputField firstNameField;
        public TMP_InputField FirstNameField { get => firstNameField; set => firstNameField = value; }

        [SerializeField] private TMP_InputField lastNameField;
        public TMP_InputField LastNameField { get => lastNameField; set => lastNameField = value; }

        [SerializeField] private TMP_Dropdown honorificDropdown;
        public TMP_Dropdown HonorificDropdown { get => honorificDropdown; set => honorificDropdown = value; }

        [SerializeField] private TMP_InputField newPasswordField;
        public TMP_InputField NewPasswordField { get => newPasswordField; set => newPasswordField = value; }

        [SerializeField] private TMP_InputField confirmNewPasswordField;
        public TMP_InputField ConfirmNewPasswordField { get => confirmNewPasswordField; set => confirmNewPasswordField = value; }

        [SerializeField] private TMP_InputField currentPasswordField;
        public TMP_InputField CurrentPasswordField { get => currentPasswordField; set => currentPasswordField = value; }

        [SerializeField] private Button submitButton;
        public Button SubmitButton { get => submitButton; set => submitButton = value; }
    }
}
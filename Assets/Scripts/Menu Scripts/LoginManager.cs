using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{

    //TODO
    /* Replace transform references with editor variable references
	 * 
	 */

    //private UserData user;
    private ServerControls server;

    public Transform LoginInfoGroup;
    public TMP_InputField UName;
    public TMP_InputField PWord;
    public TMP_InputField EText;
    public Toggle stayLoggedIn;

    public Transform loggedInButtons;
    public Transform loggedOutButtons;
    public Transform submitButton;

    public bool loggedIn = false;
    public AccountSettingsScript accountInfo;

    public TextMeshProUGUI usernameText;

    private string serverAddress = GlobalData.serverAddress + "Login.php";
    private Queue<string> casesToDownload;

    EventSystem system;

    // Use this for initialization
    void Start()
    {
        //user = transform.GetComponent<UserData> ();
        server = GameObject.Find("GaudyBG").transform.GetComponent<ServerControls>();
        if (server == null) {
            server = GetComponent<ServerControls>();
        }
        casesToDownload = new Queue<string>(10);
        system = EventSystem.current;

        //The following things toggle on/off depending on whether or not demo is false/true respectively
        if (GlobalData.demo) {
            transform.Find("SidePanel/TopPanel/LogInButton").GetComponent<Button>().interactable = !GlobalData.demo;
            transform.Find("SidePanel/TopPanel/SettingsPanel/SettingsImage").GetComponent<Button>().interactable = !GlobalData.demo;
            transform.Find("ContentPanel/TopRightControls/ImportButton").gameObject.SetActive(!GlobalData.demo);
            loggedOutButtons.Find("LoginButton").GetComponent<Button>().interactable = !GlobalData.demo;
            loggedOutButtons.Find("RegisterButton").GetComponent<Button>().interactable = !GlobalData.demo;
            loggedOutButtons.Find("ForgotPasswordButton").GetComponent<Button>().interactable = !GlobalData.demo;
            GameObject.Find("BigLoginPanel/SceneSwitch/ReturnToLogin").gameObject.SetActive(!GlobalData.demo);
            transform.Find("SidePanel/ReturnToLogin").gameObject.SetActive(!GlobalData.demo);
            server.infoPanel.Find("ButtonRow").gameObject.SetActive(!GlobalData.demo);
            transform.Find("SidePanel/TopPanel/LogInButton/UserText").GetComponent<TextMeshProUGUI>().text = "Guest";
            GlobalData.filePath = Application.streamingAssetsPath + GlobalData.menuFilePath;
            server.RemoveMenuItems();
            LoginGuest();
            //StartCoroutine(server.DownloadMenuItems());
            return;
        }

        GlobalData.recommendedCases = null;
        if (!GlobalData.username.Equals("")) {
            loggedIn = true;
            server.DisableLoginScreen();

            StartCoroutine(server.DownloadMenuItems());
        } else {
            //transform.Find("SplashScreen").gameObject.SetActive(false);
            GlobalData.username = "";
            GlobalData.password = "";
            GlobalData.email = "";
        }
        GlobalData.stayLoggedIn = PlayerPrefs.GetInt("StayLoggedIn", 0) > 0;

        if (!loggedIn) {
            //transform.Find ("Logout button").GetComponent<Button> ().interactable = false;
        }
        if (!LoginInfoGroup) {
            LoginInfoGroup = transform.Find("Login Info");
            loggedInButtons = transform.Find("LoggedInButtons");
            loggedOutButtons = transform.Find("LoggedOutButtons");
            submitButton = loggedInButtons.Find("SubmitButton");
            submitButton.gameObject.SetActive(false);
        }
        HandleCommandLineArguments();
        if (!loggedIn && GlobalData.stayLoggedIn) {
            StartCoroutine(CheckPreviousLoginSession());
        } else {
            // Splash screen shouldn't be shown when coming out of a case
            GameObject.Find("SplashScreen")?.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    // This provides tabbing and enter functionality when logging in
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && system.currentSelectedGameObject != null) {
            Selectable next;
            if (Input.GetKey(KeyCode.LeftShift))
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            else
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null) {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            //else Debug.Log("next nagivation element not found");

        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (system.currentSelectedGameObject == null) {
                return;
            }
            Toggle t;
            if ((t = system.currentSelectedGameObject.GetComponent<Toggle>()) != null) {
                t.isOn = !t.isOn;
            }

            //Prevent trying to spam login
            if (system.currentSelectedGameObject.transform.IsChildOf(LoginInfoGroup) && loggedOutButtons.Find("LoginButton").GetComponent<Button>().interactable) {
                Login();
            }
        }
    }

    /**
	 * @Logged out
	 * Logs a user in
	 */
    public void Login()
    {
        if (!GlobalData.role.Equals(GlobalData.Roles.Guest) && loggedIn) {
            ShowMessage("Already logged in", true);
            print("Already logged in");
            return;
        }
        if (UName.text.Equals("") || PWord.text.Equals("")) {
            print("Please enter credentials");
            ShowMessage("Please enter credentials", true);
            return;
        }

        if (Regex.IsMatch(UName.text, "^.+?@.+?[.].+?$")) {
            GlobalData.email = UName.text;
        } else {
            GlobalData.username = UName.text;
        }
        GlobalData.password = PWord.text;
        GlobalData.stayLoggedIn = !stayLoggedIn || stayLoggedIn.isOn;
        PlayerPrefs.SetInt("StayLoggedIn", GlobalData.stayLoggedIn ? 1 : 0);
        PlayerPrefs.Save();

        SetLoginButtonsActive(false);
        StartCoroutine(server.Login());
    }

    public void SetLoginButtonsActive(bool b)
    {
        loggedOutButtons.Find("LoginButton").GetComponent<Button>().interactable = b;
        loggedOutButtons.Find("LoginGuestButton").GetComponent<Button>().interactable = b;
        loggedOutButtons.Find("RegisterButton").GetComponent<Button>().interactable = b;
    }

    /**
	 * @Logged out
	 * Logs a user in as a guest
	 */
    public void LoginGuest()
    {
        if (server == null) {
            Start();
        }
        GlobalData.username = "Guest";
        GlobalData.stayLoggedIn = false;
        PlayerPrefs.SetInt("StayLoggedIn", GlobalData.stayLoggedIn ? 1 : 0);
        PlayerPrefs.Save();

        print("logging in");
        if (loggedIn) {
            print("Already logged in");
            return;
        }
        loggedIn = true;
        StartCoroutine(server.Login());
    }

    /**
	 * This is to be called to either set the game as a demo or to reset the demo
	 */
    public void RestartDemo(Toggle tog)
    {
        Logout();
        GameObject.Find("BigLoginPanel").gameObject.SetActive(true);
        GameObject.Find("BigLoginPanel/SceneSwitch").gameObject.SetActive(false);
        GameObject.Find("BigLoginPanel/LoginPanel").gameObject.SetActive(true);
        transform.Find("SidePanel/ReturnToLogin").gameObject.SetActive(true);
        if (tog.isOn) {
            GlobalData.demo = true;
            Start();
        } else {
            GlobalData.demo = false;
            Start();
        }
    }

    private void HandleCommandLineArguments()
    {
        //Check for command line arguments
        //I would recommend sticking with the order as follows:
        //tags, autologin, application, launchcase
        List<string> args = new List<string>(Environment.GetCommandLineArgs());
        if (GlobalData.GDS.args.Length > 0) {
            args = new List<string>(GlobalData.GDS.args.Split(new string[] { " " }, StringSplitOptions.None));
        }
        print(string.Join(",", args));
        if (args.Count > 1) {
            foreach (string arg in args) {
                switch (arg) {
                    case "-tags":
                        string tags = "";
                        int i = args.FindIndex(s => s.Equals(arg)) + 1;
                        while (i < args.Count && !args[i].StartsWith("-")) {
                            tags += args[i] + " ";
                            i++;
                        }
                        print(tags);
                        SetStartTags(tags.Trim());
                        break;
                    case "-login":
                        int k = args.FindIndex(s => s.Equals(arg)) + 1;
                        UName.text = args[k];
                        PWord.text = args[k + 1];
                        Login();
                        break;
                    case "-autologin":
                        LoginGuest();
                        break;
                    case "-application":
                        if (!loggedIn) {
                            Debug.LogError("Must call -autologin before choosing an application.");
                            break;
                        }
                        if (args[args.FindIndex(s => s.Equals(arg)) + 1].Equals("Writer")) {
                            GameObject.Find("BigLoginPanel/SceneSwitch/CaseWriter").GetComponent<Button>().onClick.Invoke();
                            GlobalData.resourcePath = "Writer";
                        } else if (args[args.FindIndex(s => s.Equals(arg)) + 1].Equals("Reader")) {
                            GameObject.Find("BigLoginPanel/SceneSwitch/CaseReader").GetComponent<Button>().onClick.Invoke();
                            GlobalData.resourcePath = "Reader";
                        }
                        break;
                    case "-launchcase":
                        if (!string.Join("", args).Contains("login")) {
                            Debug.LogError("Must call -autologin before launching a case.");
                            break;
                        }
                        int j = args.FindIndex(s => s.Equals(arg)) + 1;
                        StartCoroutine(LoadCase(args[j]));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Checks to see if a user had an active logged in session
    /// within the past 2 weeks
    /// </summary>
    private IEnumerator CheckPreviousLoginSession()
    {
        //Save the user's device ID for auto-login in the future
        if (SystemInfo.deviceUniqueIdentifier != SystemInfo.unsupportedIdentifier) {
            if (PlayerPrefs.HasKey("RemoveDevice")) {
                yield return StartCoroutine(ClearDeviceID());
            }
            WWWForm form = new WWWForm();
            form.AddField("ACTION", "checkSession");
            form.AddField("deviceid", SystemInfo.deviceUniqueIdentifier);

			string[] responseSplit;
			using (UnityWebRequest webRequest = UnityWebRequest.Post(GetServerAddress(), form)) {
				yield return webRequest.SendWebRequest();
				print(webRequest.downloadHandler.text);
				responseSplit = webRequest.downloadHandler.text.Split(new string[] { "--" }, StringSplitOptions.None);
			}

            if (responseSplit[0].Equals("Connection Granted")) {
                StartCoroutine(server.SuccessfulLogin(responseSplit));
            } else if (responseSplit.Length == 1 && string.IsNullOrEmpty(responseSplit[0])) {
                StartCoroutine(server.OfflineLogin());
            } else {
                StartCoroutine(server.CloseSplash());
            }
        } else {
            StartCoroutine(server.CloseSplash());
        }
    }

    /// <summary>
    /// Loads a case from command line arguments
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    private IEnumerator LoadCase(string filename)
    {
        //server.ShowLoadingScreen();
        //Show custom splash screen while downloading cases
        int checks = 50;
        int i = 0;
        while (!loggedIn) {
            yield return new WaitForSecondsRealtime(.2f);
            i++;
            if (i >= checks) {
                Debug.LogError("Did not log in a timely manner. Returning");
                yield break;
            }
        }

        while (server.isDownloadingAllCases || !server.GetListsAreLoaded()) {
            yield return null;
        }

        GlobalData.caseObj = server.GetCaseByFilename(filename.Replace("_", " "));
        if (GlobalData.caseObj == null) {
            GlobalData.caseObj = server.GetCaseByRecordNumber(filename);
        }
        if (GlobalData.caseObj == null) {
            Debug.LogError("No case with the provided info found");
            server.loadingScreen.gameObject.SetActive(false);
            yield break;
        }
        if (!GlobalData.caseObj.IsPublic() && GlobalData.caseObj.accountId != GlobalData.accountId) {
            Debug.Log("User does not have access to provided case");
            server.loadingScreen.gameObject.SetActive(false);
            yield break;
        }

        GameObject.Find("BigLoginPanel").gameObject.SetActive(true);
        GameObject.Find("BigLoginPanel/LoginPanel").gameObject.SetActive(false);
        GameObject.Find("BigLoginPanel/SceneSwitch").gameObject.SetActive(false);
        GameObject.Find("BigLoginPanel/ShortcutSplashScreen").gameObject.SetActive(true);
        transform.Find("SidePanel/ViewSuggestedCase").gameObject.SetActive(true);
        GameObject.Find("BigLoginPanel/ShortcutSplashScreen/StartCaseButton").GetComponent<Button>().onClick.AddListener(
            delegate {
                server.ShowLoadingScreen();
                GlobalData.fileName = GlobalData.caseObj.filename;
                GlobalData.filePath = server.GetLocalSavesFolderPath();
                if (GlobalData.caseObj.IsTemplate()) {
                    GlobalData.createCopy = true;
                }

                string levelToLoad = "Writer";
                if (!server.GetIsWriter()) {
                    levelToLoad = "CassReader";
                }

                StartCoroutine(GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().OpenScene(levelToLoad));
            });

        /*GlobalData.fileName = GlobalData.caseObj.filename;
		GlobalData.filePath = server.GetLocalSavesFolderPath();
		if (GlobalData.caseObj.IsTemplate()) {
			GlobalData.createCopy = true;
		}
		StartCoroutine(GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().OpenScene(levelToLoad));*/

    }

    /**
	 * @Logged in
	 * Button to display the information of who is logged in, or no if nobody is logged in
	 */
    public void LoggedIn(Text t)
    {
        if (loggedIn) {
            t.text = "Yes " + GlobalData.username + ", " + GlobalData.password + ", " + GlobalData.email + ", " + GlobalData.role;
        } else {
            t.text = "No";
        }
    }

    public void ResetFields()
    {
        UName.text = "";
        PWord.text = "";
    }

    //------------------------------------
    //The following has been moved to it's own script due to the registration panel being separated
    #region old

    /**
	 * @Logged out
	 * Registers an account
	 */
    public void OLDRegister()
    {
        GlobalData.username = UName.text;
        GlobalData.password = PWord.text;
        Debug.Log(GlobalData.email);
        GlobalData.email = EText.text;
        Debug.Log(GlobalData.email);
        if (!Regex.IsMatch(GlobalData.email, "^.+?[@].+?[.].+?$")) {
            ShowMessage("Email not valid", true);
            print("Email not valid");
            return;
        }
        if (GlobalData.username.Equals("") || GlobalData.password.Equals("") || GlobalData.email.Equals("")) {
            print("Please enter credentials");
            ShowMessage("Please enter credentials", true);
            return;
        }
        StartCoroutine(server.Register());
    }

    /**
	 * @Logged out
	 * Resends the account activation email
	 */
    public void OLDResendActivationEmail()
    {
        GlobalData.username = UName.text;
        GlobalData.password = PWord.text;
        GlobalData.email = EText.text;
        StartCoroutine(server.ResendActivationEmail());
    }

    /**
	 * @Logged out
	 * Call to send an email comtaining a remplacement password
	 */
    public void OLDForgotPassword()
    {
        GlobalData.username = UName.text;
        GlobalData.email = EText.text;
        StartCoroutine(server.ForgotPassword());
    }
    #endregion
    //--------------------------------------

    /**
	 * @Logged in
	 * Updates the user's password
	 */
    public void UpdatePassword()
    {
        StartCoroutine(server.UpdatePassword());
    }

    /**
	 * @Logged out
	 * Call to send an email comtaining a remplacement password
	 */
    public void ForgotPassword()
    {
        if (Regex.IsMatch(UName.text, "^.+?@.+?[.].+?$")) {
            GlobalData.email = UName.text;
        } else {
            GlobalData.username = UName.text;
        }
        StartCoroutine(server.ForgotPassword());
    }

    /**
	 * @Logged out
	 * Call to send an email comtaining a remplacement password
	 */
    public void ForgotPassword(Text text)
    {
        if (Regex.IsMatch(text.text, "^.+?@.+?[.].+?$")) {
            GlobalData.email = text.text;
        } else {
            GlobalData.username = text.text;
        }
        StartCoroutine(server.ForgotPassword());
    }

    /**
	 * @Logged in
	 * Displays the fields so the user can update their password
	 */
    public void ShowUpdateFields()
    {
        LoginInfoGroup.gameObject.SetActive(true);
        //submitButton.gameObject.SetActive (true);
        //updateButton.gameObject.SetActive (false);
    }

    public Transform settingsMenu, moreSettings;
    /**
	 * @Logged in
	 * Displays the settings menu, only showing the "more settings" button if the user is logged in.
	 */
    public void ShowSettingsMenu()
    {
        settingsMenu.gameObject.SetActive(true);
        moreSettings.gameObject.SetActive(loggedIn && GlobalData.username != "Guest" && GlobalData.username != "");
    }

    /**
	 * @Logged in
	 * Logs the user out
	 */
    public void Logout()
    {
        if (!GlobalData.role.Equals(GlobalData.Roles.Guest) && !GlobalData.offline) {
            Tracker.Upload(true);
        }

        StartCoroutine(ClearDeviceID());

        PlayerPrefs.SetInt("StayLoggedIn", 0);
        PlayerPrefs.Save();

        GlobalData.username = "";
        GlobalData.password = "";
        GlobalData.email = "";
        loggedIn = false;
        GlobalData.role = "";
        GlobalData.userFirstName = "";
        GlobalData.userLastName = "";
        GlobalData.userTitle = "--";
        //loggedInButtons.gameObject.SetActive (false);
        //loggedOutButtons.gameObject.SetActive (true);
        LoginInfoGroup.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        transform.Find("SidePanel/TopPanel/LogInButton/UserText").GetComponent<TextMeshProUGUI>().text = "Login";
        transform.Find("SidePanel/OpenSaveLocationButton").gameObject.SetActive(false);
        transform.Find("ContentPanel/SectionButtonsPanel/YourCases CaseSelector/Scroll View/DownloadingCasesNotification").gameObject.SetActive(true);
        server.infoPanel.gameObject.SetActive(false);
        server.RemoveMenuItems(); //Destroy any cases in the menu
        server.ClearMenuItems(); //Deletes all possible cases to view
        server.DisposeCaseDownload();
        if (server.showingCourses) {
            transform.Find("SidePanel/MainPanel/ButtonsPanel/CoursesToggle").GetComponent<Toggle>().isOn = false;
        }
        if (server.menuPageButtons) {
            server.menuPageButtons.gameObject.SetActive(false);
        }

        //updateButton.gameObject.SetActive (false);
        //transform.Find ("Delete Account/InputField").gameObject.SetActive (false);
    }

    public void Reconnect()
    {
        StartCoroutine(TryReconnect());
    }
    private IEnumerator TryReconnect()
    {
        if (SystemInfo.deviceUniqueIdentifier != SystemInfo.unsupportedIdentifier) {
            if (PlayerPrefs.HasKey("RemoveDevice")) {
                yield return StartCoroutine(ClearDeviceID());
            }
            WWWForm form = new WWWForm();
            form.AddField("ACTION", "checkSession");
            form.AddField("deviceid", SystemInfo.deviceUniqueIdentifier);

			string[] responseSplit;
			using (UnityWebRequest webRequest = UnityWebRequest.Post(GetServerAddress(), form)) {
				yield return webRequest.SendWebRequest();
				print(webRequest.downloadHandler.text);
				responseSplit = webRequest.downloadHandler.text.Split(new string[] { "--" }, StringSplitOptions.None);
			}

			if (responseSplit[0].Equals("Connection Granted")) {
                GlobalData.offline = false;
                StartCoroutine(server.SuccessfulLogin(responseSplit));
                ShowMessage("Successfully reconnected to the server.");
            } else if (responseSplit.Length == 1 && string.IsNullOrEmpty(responseSplit[0])) {
                ShowMessage("Could not reach the server.", true);
            } else {
                ShowMessage("Please logout and reenter your credentials.", true);
            }
        } else {
            ShowMessage("Could not reconnect to the server.", true);
        }
    }

    private IEnumerator ClearDeviceID()
    {
        if (!GlobalData.offline) {
            WWWForm form = new WWWForm();
            form.AddField("ACTION", "clearDevice");
            form.AddField("deviceid", SystemInfo.deviceUniqueIdentifier);
			string outputText;
			using (UnityWebRequest webRequest = UnityWebRequest.Post(GetServerAddress(), form)) {
				yield return webRequest.SendWebRequest();
				outputText = webRequest.downloadHandler.text;
				print(outputText);

				if (outputText.Equals("No device id found in table")) {
					print("No device found");
					PlayerPrefs.DeleteKey("RemoveDevice");
				} else if (webRequest.error != null || !outputText.Equals("Successfully cleared device id")) {
					print("Could not clear device id:\n" + outputText);
					PlayerPrefs.SetString("RemoveDevice", "True");
				} else {
					print("Successfully cleared device id");
					PlayerPrefs.DeleteKey("RemoveDevice");
				}
			}
        } else {
            print("Offline, so could not clear device id");
            PlayerPrefs.SetString("RemoveDevice", "True");
        }
    }

    /**
	 * Bans a user. Must be logged in as an admin to do so
	 */
    public void BanUser(Text t)
    {
        if (loggedIn && GlobalData.role.Equals("Admin")) {
            if (t.text != null && !t.text.Equals("")) {
                StartCoroutine(server.Ban(t.text));
            }
        } else {
            print("You do not have the ability to ban");
        }
    }

    /**
	 * Remove account as specified by an admin
	 */
    public void RemoveAccount(Text username)
    {
        string userN = username.text;
        if (userN.Equals("")) {
            userN = GlobalData.username;
        }
        StartCoroutine(server.RemoveAccount(userN));
        username.text = "";
        username.transform.parent.GetComponent<InputField>().text = "";
    }

    public void ShowUpdatePanel()
    {
        accountInfo.PopulateFields();
        accountInfo.panel.SetActive(true);

    }

    public void UpdateAccountInfo()
    {
        if (accountInfo.currentPassword.text.Length == 0) {
            print("Please enter your current password to change information");
            ShowMessage("Please enter your current password to change information", true);
            return;
        }

        if (!accountInfo.newPassword.text.Equals("") && accountInfo.newPassword.text.GetHashCode() != accountInfo.confirmPassword.text.GetHashCode()) {
            print("New password does not match");
            ShowMessage("New password does not match", true);
            return;
        }

        if (GlobalData.offline) {

            print("Cannot change information while offline");
            ShowMessage("Cannot change information while offline", true);
            return;
        }

        //We check the current password to make sure it's correct before updating fields
        StartCoroutine(server.UpdateAccountInfo());
    }

    /**
	 * Removes the selected case from the database
	 */
    public void RemoveCase(TextMeshProUGUI filename)
    {
        if (filename.text.Equals("")) {
            print("No filename specified");
            ShowMessage("No filename specified", true);
            return;
        }
        if (!loggedIn) {
            print("Please login to delete a case from your account!");
            ShowMessage("Please login to delete a case from your account!", true);
            return;
        }

        // If we're not working on our case, we can just remove the local save without worry of messing anything up, since it can always be redownloaded
        if (GlobalData.caseObj.accountId != GlobalData.accountId) {
            print("Deleting local");
            StartCoroutine(server.RemoveCase(filename.text, false, true));
            return;
        }

        /*string panelName = "ConfirmActionBG";
        GameObject pinPanelPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/" + panelName)) as GameObject;
        pinPanelPrefab.transform.SetParent(GameObject.Find("GaudyBG").transform, false);
        ConfirmationScript cs = pinPanelPrefab.GetComponent<ConfirmationScript>(); //GameObject.Find ("GaudyBG").transform.Find ("ConfirmActionBG").GetComponent<ConfirmationScript> ();
        cs.gameObject.SetActive (true);
        cs.actionText.text = "Are you sure you want to remove this case?";
        cs.obj = filename.gameObject;
        cs.args = new object[] { filename };
        cs.AnyParamMethodToConfirm = ApprovedRemoveCase;*/

        //Open up confirmation for what to load.
        GameObject confirm = GameObject.Find("GaudyBG").transform.Find("Popups/LocalOrServerConfirmation").gameObject;
        var confirmDialogue = confirm.transform.Find("ConfirmActionPanel/Content/Row0/ActionValue").GetComponent<TextMeshProUGUI>();
        Button serverButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/ServerButton").GetComponent<Button>();
        Button localButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/LocalButton").GetComponent<Button>();
        Button bothButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/BothButton").GetComponent<Button>();
        bothButton.gameObject.SetActive(true);

        serverButton.onClick.RemoveAllListeners();
        localButton.onClick.RemoveAllListeners();
        bothButton.onClick.RemoveAllListeners();

        string formattedFilename = filename.text.Replace("_", " ");

        if (server.GetCaseByFilename(formattedFilename).accountId == GlobalData.accountId) {
            serverButton.gameObject.SetActive(true);
            bothButton.gameObject.SetActive(true);
            serverButton.onClick.AddListener(delegate {
                print("Delete server");
                confirm.SetActive(false);
                StartCoroutine(server.RemoveCase(filename.text, true, false));
            });

            bothButton.onClick.AddListener(delegate {
                print("Delete both");
                confirm.SetActive(false);
                StartCoroutine(server.RemoveCase(filename.text, true, true));
            });

        } else {
            confirm.transform.Find("ConfirmActionPanel/Content/Row0/ActionValue").GetComponent<TextMeshProUGUI>().text = "Delete local download?";
            serverButton.gameObject.SetActive(false);
            bothButton.gameObject.SetActive(false);
        }

        localButton.onClick.AddListener(delegate {
            print("Delete local");
            confirm.SetActive(false);
            StartCoroutine(server.RemoveCase(filename.text, false, true));
            serverButton.gameObject.SetActive(true);
            bothButton.gameObject.SetActive(true);
        });

        confirm.SetActive(true);

        if (GlobalData.caseObj.localOnly) {
            serverButton.gameObject.SetActive(false);
            bothButton.gameObject.SetActive(false);
            localButton.gameObject.SetActive(true);

            confirmDialogue.text = "Permanently delete this local case?";
        } else if (!GlobalData.caseObj.downloaded) {
            serverButton.gameObject.SetActive(true);
            bothButton.gameObject.SetActive(false);
            localButton.gameObject.SetActive(false);

            confirmDialogue.text = "Permanently delete this case from the server?";
        } else {
            serverButton.gameObject.SetActive(true);
            bothButton.gameObject.SetActive(true);
            localButton.gameObject.SetActive(true);

            confirmDialogue.text = "Delete from Server only, Local only, or Both?";
        }

        //StartCoroutine (server.RemoveCase (filename.text));
    }

    /**
	 * Begins downloading a case from the menu using the text filename
	 */
    public void DownloadCase(TextMeshProUGUI filename)
    {
        DownloadCase(filename.text);
    }

    /**
	 * Begins downloading a case from the menu using a string for filename
	 */
    public void DownloadCase(string filename)
    {
        if (!loggedIn) {
            print("You are not logged in. Cannot download case");
            ShowMessage("You are not logged in. Cannot download case", true);
            return;
        }
        print(filename);
        print(server.DownloadingCase());
        if (!casesToDownload.Contains(filename) && !server.DownloadingCase().Equals(filename.Replace(" ", "_"))) {
            casesToDownload.Enqueue(filename);
            if (!server.IsDownloading()) {
                print("Starting download");
                StartCoroutine(server.DownloadCase());
            } else {
                print("Already downloading");
            }
        } else {
            print("Case already in download queue");
        }
    }

    /**
	 * This returns the filename of the next case to download
	 * Works like a queue. Pops item when called
	 */
    public string GetCaseToDownload()
    {
        if (casesToDownload.Count > 0) {
            return casesToDownload.Dequeue();
        } else {
            return null;
        }
    }

    #region SHOW_MESSAGE

    private GameObject notification;
    private bool fade;
    /**
	 * Use this to show a confirmation that the case was saved successfully. Defaults to non-error messages
	 */
    public void ShowMessage(string message)
    {
        ShowMessage(message, false);
    }

    /**
	 * Use this to show a confirmation that the case was saved successfully
	 */
    public void ShowMessage(string message, bool error)
    {
        if (transform.Find("ContentPanel") != null && transform.Find("ContentPanel").GetComponent<FilePickerScript>().levelName.Equals("CassReader")) {
            return;
        }
        Transform parentTransform = GameObject.Find("TopCanvas")?.transform;
        if (parentTransform == null) {
            parentTransform = GameObject.Find("GaudyBG").transform;
        }
        if (!error) {
            if (parentTransform.Find("NotificationPanel") == null) {
                Destroy(notification);
                notification = Instantiate(Resources.Load("Writer/Prefabs/NotificationMessage") as GameObject, parentTransform);
                notification.name = "NotificationPanel";
            }
        } else if (parentTransform.Find("ErrorPanel") == null) {
            Destroy(notification);
            notification = Instantiate(Resources.Load("Writer/Prefabs/ErrorMessage") as GameObject, parentTransform);
            notification.name = "ErrorPanel";
        }
        CancelInvoke("Fade");
        fade = false;
        notification.GetComponent<CanvasGroup>().alpha = 1;
        notification.SetActive(true);
        TextMeshProUGUI messageText = notification.transform.Find("BG/Message").GetComponent<TextMeshProUGUI>();
        messageText.text = message;
        Invoke("Fade", 5f);
        NextFrame.Function(delegate { fade = true; });

    }

    /**
	 * Used temporarily as a delay for the save confirmation message
	 */
    private void Fade()
    {
        if (notification.GetComponent<CanvasGroup>().alpha > 0) {
            if (!fade) {
                return;
            }
            //t.color = new Color (t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime / 6f);
            notification.GetComponent<CanvasGroup>().alpha = (notification.GetComponent<CanvasGroup>().alpha - Time.deltaTime / 3f);
            NextFrame.Function(Fade);
            return;
        }
        Destroy(notification);
    }

    #endregion

    /**
	 * Returns the address of the login server
	 */
    public string GetServerAddress()
    {
        return serverAddress;
    }

    /**
	 * Shows the correct help screen on the main menu
	 */
    public void ShowHelp()
    {
        if (server.GetIsWriter()) {
            transform.Find("WriterQuickStart").gameObject.SetActive(true);
        } else {
            transform.Find("ReaderQuickStart").gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// Sets the tags on menu load (From command line arguments)
    /// </summary>
    /// <param name="tags"></param>
    public void SetStartTags(string tags)
    {
        if (transform.Find("SidePanel/MainPanel/YourCasesMenuPanel/InputField1Line") != null) {
            transform.Find("SidePanel/MainPanel/YourCasesMenuPanel/InputField1Line").GetComponent<InputField>().text = tags;
        }
        //Alternatively, only pass in popular tags and have them selected under Category
    }

    //-------------DEMO-------------------
    #region DEMO_METHODS
    public void RevertDemoFiles()
    {
        StartCoroutine(RevertFilesCoroutine("OriginalCopies"));
    }

    public void RevertECGCFiles()
    {
        StartCoroutine(RevertFilesCoroutine("ECGCCases"));
    }

    public IEnumerator RevertFilesCoroutine(string folder)
    {
        string copies = Application.streamingAssetsPath + "/DemoCases/" + folder;
        string cases = Application.streamingAssetsPath + "/DemoCases/Cases";

        Directory.Delete(cases, true);
        Directory.CreateDirectory(cases);

        if (Directory.Exists(copies)) {
            string[] files = Directory.GetFiles(copies);
            string filename;
            foreach (string s in files) {
                if (s.EndsWith(".meta")) {
                    continue;
                }
                filename = Path.GetFileName(s);
                File.Copy(s, Path.Combine(cases, filename), true);
            }
        }
        //UnityEditor.FileUtil.ReplaceDirectory(Application.streamingAssetsPath + "/DemoCases/OriginalCopies", Application.streamingAssetsPath + "/DemoCases/Cases");
        server.RemoveMenuItems();
        yield return StartCoroutine(server.DownloadMenuItems());
    }
    #endregion
}

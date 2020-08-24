using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/**
 * This class is the meat behind the main menu's interactions
 * It's mostly responsible for handling all cases (including sorting, searching, and displaying information)
 * and for communicating with the server for login/registration purposes.
 * It is also used to register/login from within the writer.
 */
public class ServerControls : MonoBehaviour
{

    //Public variables
    public int casesPerPageLimit = 18;  //Limit to how many cases can be shown to the user at one time
    public Transform menuPageButtons;   //Reference to the page buttons
    public Transform infoPanel;         //Reference to the right-side info panel
    public GameObject loadingScreen;    //Reference to the loading screen
    public GameObject downloadingCases; // Reference to the downloading cases notification
    public Transform sideSearchPanel;   //Reference to the side panel used for searching
    public bool showOwnedCasesOnly;     //Whether or not to show owned cases or all public cases
    public Dropdown caseSortDropdown;   //Reference to the dropdown which governs sorting order
    public ToggleGroup casesSortToggleGroup;

    public GameObject firstWritePanel;

    //Private variables
    //Login variables
    private LoginManager lm;        //Reference to the LoginManager script
    public RegistrationScript registrationScript;        //Reference to the LoginManager script
    private string serverAddress;   //The address for the server (to gain access to the PHP scripts)

    //Case variables
    private Transform caseExampleParent;    //The parent transform for the cases to spawn under
    private MenuCase selected;              //The currently selected case
    private List<MenuCase> caseItems;       //A list of all regular cases
    private List<MenuCase> templateItems;   //A list of all template cases
    private bool showingCases;              //Whether or not the user is viewing regular cases or templates
    private bool disposeDownload;           //The WWW used to download cases (Stored as a local variable to cancel it upon logout)
    public bool isDownloadingAllCases;      //Whether or not cases are downloading after somebody logged in
    public string readerLevelName;                // What level to load 
    public bool showingCourses {
        get;
        private set;
    }

    //Misc variables related to cases
    [SerializeField]
    private bool isList;                    //Whether or not the user is viewing cases in the list view
    [SerializeField]
    private bool downloading;               //Whether or not a case is downloading from the download queue
    private string downloadingCase;         //The name of the case which is downloading from the download queue
    private int menuPage;                   //The page that the user is viewing (0 if less than casesPerPageLimit cases are shown)
    private bool selectingAllTags;          //Will be used to avoid running "DisplayMenuItems" a bajillion times
    private Button selectedButton;          //Stores the selected read/edit button (for handling naming the patient)
    private bool isWriter;                  //Determines the mode the user is in between Writer and Reader
    private bool listsAreLoaded;            //Used as a flag to denote that the lists are done loading downloaded cases
    public bool forceReader;                //Whether or not to bypass the "Select Mode" screen and only provide the reader
    private string menuFilePath;

    //For double clicking
    private bool doubleClickable;           //If the selected case is available for double clicking
    private bool firstClick;                //If the first click happened less than doubleClickTime before the second click
    private float doubleClickTime = .8f;    //The amount of time the user has to double click

    // Use this for initialization
    void Start()
    {
        lm = transform.GetComponent<LoginManager>();
        //GlobalData = transform.GetComponent<GlobalData> ();
        serverAddress = lm.GetServerAddress();
        caseExampleParent = GameObject.Find("GaudyBG").transform.Find("ContentPanel/SectionButtonsPanel/YourCases CaseSelector/Scroll View/Content/Viewport/GridContent");
        //infoPanel = GameObject.Find ("GaudyBG").transform.Find ("ContentPanel/SectionButtonsPanel/CaseOverviewPanel/InformationPanel");
        caseItems = new List<MenuCase>();
        templateItems = new List<MenuCase>();
        downloading = false;
        downloadingCase = "";
        menuPage = 0;
        showingCases = true;
        selectingAllTags = false;
        showOwnedCasesOnly = true;
        listsAreLoaded = false;
        menuFilePath = Application.streamingAssetsPath + "/MenuCases.txt";
        if (PlayerPrefs.GetInt("OnApplicationQuit", -1) == -1 || PlayerPrefs.GetInt("OnApplicationQuit", -1) == 0) { //If the key doesn't exist
            PlayerPrefs.SetInt("OnApplicationQuit", 1);
            Application.quitting += delegate {
                PlayerPrefs.SetInt("OnApplicationQuit", 0);
                PlayerPrefs.Save();
                DeleteMenuFile();
            };
            PlayerPrefs.Save();
        } else {
            Application.quitting += delegate {
                PlayerPrefs.SetInt("OnApplicationQuit", 0);
                PlayerPrefs.Save();
                DeleteMenuFile();
            };
            PlayerPrefs.SetInt("OnApplicationQuit", 1);
            PlayerPrefs.Save();
        }

        ToggleListView(); //Start in list view
    }

    public void UpdateCases()
    {
        StartCoroutine(UpdateCasesCoroutine());
    }

    private IEnumerator UpdateCasesCoroutine()
    {
		using (UnityWebRequest webRequest = UnityWebRequest.Get(GlobalData.serverAddress + "Track.php?action=updateRatings")) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text);
		}
    }

    //-------------------------------------------------------------------------------------------
    #region --------------------------------General Login Account Features-------------------------------


    public IEnumerator Login()
    {
        print("Logging in...");
        if (GlobalData.username.ToLower().Equals("guest") || (GlobalData.username.Equals("") && GlobalData.email.Equals(""))) {
            GlobalData.role = GlobalData.Roles.Guest;
            GlobalData.accountId = 0;
            GlobalData.username = "Guest";
            lm.loggedIn = true;
            print("Successfully logged in");
            //lm.ShowMessage ("Successfully logged in");
            //lm.loggedInButtons.gameObject.SetActive (true);
            //lm.loggedOutButtons.gameObject.SetActive (false);
            lm.LoginInfoGroup.gameObject.SetActive(false);
            //transform.Find("LoginPanel").gameObject.SetActive(false);

            DisableLoginScreen();
            if (!GlobalData.demo) {
                GlobalData.filePath = GetLocalSavesFolderPath();
                transform.Find("SidePanel/TopPanel/LogInButton/UserText").GetComponent<TextMeshProUGUI>().text = "Guest";
            }

            lm.SetLoginButtonsActive(true);
            StartCoroutine(DownloadMenuItems());

            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("ACTION", "login");
        //print(GlobalData.email + ", " + GlobalData.username);
        if (GlobalData.email != null && !GlobalData.email.Equals("") && GlobalData.username.Equals("")) {
            form.AddField("email", GlobalData.email);
        } else {
            form.AddField("username", GlobalData.username);
        }
        form.AddField("password", GlobalData.password);
        GameObject.Find("BigLoginPanel/LoginPanel/BackgroundPanel/Login/LoggingInNotification").gameObject.SetActive(true);

		string[] serverResponseSplit;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text);
			serverResponseSplit = webRequest.downloadHandler.text.Split(new string[] { "--" }, StringSplitOptions.None);
		}

        GameObject.Find("BigLoginPanel/LoginPanel/BackgroundPanel/Login/LoggingInNotification").gameObject.SetActive(false);
        //Connection granted--role--accId--username--email
        //firstname--lastname--title--cases
        if (serverResponseSplit.Length > 0) {
            if (serverResponseSplit[0].Equals("Connection Granted")) {
                StartCoroutine(SuccessfulLogin(serverResponseSplit));
            } else if (serverResponseSplit[0].Equals("Not Validated")) {
                lm.SetLoginButtonsActive(true);
                print("Please validate your email first");
                lm.ShowMessage("Please validate your email first", true);
            } else if (serverResponseSplit.Length == 1 && string.IsNullOrEmpty(serverResponseSplit[0])) {
                lm.SetLoginButtonsActive(true);
                lm.ShowMessage("Could not connect to the server.", true);
                PlayerPrefs.SetInt("StayLoggedIn", 0);
                PlayerPrefs.Save();
                //StartCoroutine(OfflineLogin());
            } else {
                lm.SetLoginButtonsActive(true);
                lm.ShowMessage(string.Join("", serverResponseSplit), true);
                PlayerPrefs.SetInt("StayLoggedIn", 0);
                PlayerPrefs.Save();
            }
        } else {
            lm.SetLoginButtonsActive(true);
            print("Nothing returned from request. Unable to login.");
            lm.ShowMessage("Nothing returned from request. Unable to login.", true);
        }

    }

    public IEnumerator SuccessfulLogin(string[] serverResponseSplit)
    {
        GlobalData.role = serverResponseSplit[1];
        GlobalData.accountId = int.Parse(serverResponseSplit[2]);
        GlobalData.username = serverResponseSplit[3];
        GlobalData.email = serverResponseSplit[4];
        GlobalData.userFirstName = serverResponseSplit[6];
        GlobalData.userLastName = serverResponseSplit[7];
        GlobalData.userTitle = serverResponseSplit[8];
        GlobalData.filePath = GetLocalSavesFolderPath();

        PlayerPrefs.SetString("role", GlobalData.role);
        PlayerPrefs.SetInt("accountId", GlobalData.accountId);
        PlayerPrefs.SetString("username", GlobalData.username);
        PlayerPrefs.SetString("email", GlobalData.email);
        PlayerPrefs.SetString("userFirstName", GlobalData.userFirstName);
        PlayerPrefs.SetString("userLastName", GlobalData.userLastName);
        PlayerPrefs.SetString("userTitle", GlobalData.userTitle);

        //Load local tracked data
        Tracker.ReloadLocalData();
        Tracker.LoadData();

        //Save the user's device ID for auto-login in the future
        if (SystemInfo.deviceUniqueIdentifier != SystemInfo.unsupportedIdentifier) {
            //Upload player's deviceUniqueIdentifier and the time accessed (Time can be done in php)
            WWWForm form = new WWWForm();
            form.AddField("ACTION", "saveLogin");
            form.AddField("username", GlobalData.username);
            form.AddField("deviceid", SystemInfo.deviceUniqueIdentifier);
			using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
				yield return webRequest.SendWebRequest();
				print(webRequest.downloadHandler.text); //For debugging
			}
        }


        lm.loggedIn = true;
        print("Successfully logged in");
        Tracker.UpdateUserID();
        //lm.ShowMessage ("Successfully logged in");
        //lm.updateButton.gameObject.SetActive (true);

        // Clear login details
        lm.ResetFields();
        registrationScript.ResetFields();
        lm.LoginInfoGroup.gameObject.SetActive(false);


        Debug.Log(GlobalData.role);
        if (GlobalData.role.Equals(GlobalData.Roles.Admin)) {
            //transform.Find ("Delete Account/InputField").gameObject.SetActive (true);
        }
        if (!SceneManager.GetActiveScene().name.Equals("Writer")) {
            DisableLoginScreen();
            lm.SetLoginButtonsActive(true);

            if (serverResponseSplit[5].Equals("0")) { //First time logging in, so display the prompt to enter information
                lm.accountInfo.firstTimeNotice.SetActive(true);
                lm.accountInfo.currentPassword.transform.parent.gameObject.SetActive(false);
                lm.ShowUpdatePanel();
                lm.accountInfo.currentPassword.text = GlobalData.password;
            } else {
                lm.accountInfo.firstTimeNotice.SetActive(false);
                lm.accountInfo.currentPassword.transform.parent.gameObject.SetActive(true);
            }

            StartCoroutine(DownloadMenuItems());
        } else {
            GameObject.Find("GaudyBG").transform.Find("SaveCaseBG").gameObject.SetActive(true);
            GameObject.Find("GaudyBG").transform.Find("QuickStartRegistration").gameObject.SetActive(false);
            GameObject.Find("GaudyBG").transform.Find("SidePanel/MainPanel/MenuPanel/ShowCaseInReader").gameObject.SetActive(true);

            //StartCoroutine(CloseSplash());
        }
    }


    public IEnumerator OfflineLogin()
    {
        GlobalData.offline = true;

        GlobalData.role = PlayerPrefs.GetString("role");
        GlobalData.accountId = PlayerPrefs.GetInt("accountId");
        GlobalData.username = PlayerPrefs.GetString("username");
        GlobalData.email = PlayerPrefs.GetString("email");
        GlobalData.userFirstName = PlayerPrefs.GetString("userFirstName");
        GlobalData.userLastName = PlayerPrefs.GetString("userLastName");
        GlobalData.userTitle = PlayerPrefs.GetString("userTitle");
        GlobalData.filePath = GetLocalSavesFolderPath();

        //Load local tracked data
        Tracker.ReloadLocalData();
        //Tracker.LoadData();

        lm.loggedIn = true;
        print("Successfully logged in");
        Tracker.UpdateUserID();
        //lm.ShowMessage ("Successfully logged in");
        //lm.updateButton.gameObject.SetActive (true);

        // Clear login details
        lm.ResetFields();
        registrationScript.ResetFields();
        lm.LoginInfoGroup.gameObject.SetActive(false);


        Debug.Log(GlobalData.role);
        if (GlobalData.role.Equals(GlobalData.Roles.Admin)) {
            //transform.Find ("Delete Account/InputField").gameObject.SetActive (true);
        }
        if (!SceneManager.GetActiveScene().name.Equals("Writer")) {
            DisableLoginScreen();
            lm.SetLoginButtonsActive(true);

            lm.accountInfo.firstTimeNotice.SetActive(false);
            lm.accountInfo.currentPassword.transform.parent.gameObject.SetActive(true);

            StartCoroutine(DownloadMenuItems());
        } else {
            GameObject.Find("GaudyBG").transform.Find("SaveCaseBG").gameObject.SetActive(true);
            GameObject.Find("GaudyBG").transform.Find("QuickStartRegistration").gameObject.SetActive(false);
            GameObject.Find("GaudyBG").transform.Find("SidePanel/MainPanel/MenuPanel/ShowCaseInReader").gameObject.SetActive(true);

            //StartCoroutine(CloseSplash());
        }

        print("Could not connect to the server. Started in offline mode.");
        lm.ShowMessage("Could not connect to the server. Started in offline mode.", true);

        yield return null;
    }

    public IEnumerator CloseSplash()
    {
        var splash = GameObject.Find("SplashScreen");
        if (splash != null) {
            if (isDownloadingAllCases) {
                while (isDownloadingAllCases) {
                    yield return null;
                }
            }
            /*if (downloadingCases != null)
            {
                while (downloadingCases.activeSelf)
                    yield return 0;
            }*/

            var splashGroup = splash.GetComponent<CanvasGroup>();

            while (splashGroup.alpha > 0) {
                yield return .01;
                splashGroup.alpha -= .02f;
            }
        }

        yield return null;
    }

    public void DisableLoginScreen()
    {
        //lm.loggedInButtons.gameObject.SetActive(true);
        //lm.loggedOutButtons.gameObject.SetActive(false);
        //transform.Find("LoginPanel").gameObject.SetActive(false);
        if (!forceReader) {
            if (GameObject.Find("BigLoginPanel") != null) {
                //transform.Find("BigLoginPanel").gameObject.SetActive(false);
                GameObject.Find("BigLoginPanel").transform.Find("SceneSwitch").gameObject.SetActive(true);
                GameObject.Find("GaudyBG").transform.Find("AccountButton").gameObject.SetActive(true);
                GameObject.Find("BigLoginPanel/LoginPanel").gameObject.SetActive(false);
            }
        } else {
            SetIsWriter(false);
            SpawnMenuItems();
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/BrowseCases").gameObject.SetActive(true);
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/TemplateCases").gameObject.SetActive(false);
            GameObject.Find("BigLoginPanel").gameObject.SetActive(false);
        }
        StartCoroutine(CloseSplash());
        //transform.Find("SplashScreen").gameObject.SetActive(false);

        transform.Find("SidePanel/TopPanel/LogInButton/UserText").GetComponent<TextMeshProUGUI>().text = "User";

        if (GlobalData.GDS && GlobalData.GDS.developer) {
            //transform.Find("SidePanel/OpenSaveLocationButton").gameObject.SetActive(true);
            if (GlobalData.GDS.developerWindow) {
                GlobalData.GDS.developerWindow.SetActive(true);
            }
        }
    }

    public IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("ACTION", "register");

        form.AddField("username", GlobalData.username);
        form.AddField("password", GlobalData.password);
        form.AddField("email", GlobalData.email);
        lm.ShowMessage("Registering...");

		string[] serverResponseSplit;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text); //For debugging
			serverResponseSplit = SplitServerResponse(webRequest.downloadHandler.text, "--");
		}

		if (serverResponseSplit.Length > 0 && serverResponseSplit[0].Equals("Connection Granted")) {
            print("Success. Please check email (or spam folder) for verification");
            lm.ShowMessage("Success. Please check email (or spam folder) for verification");

            if (SceneManager.GetActiveScene().name.Equals("Writer")) {
                transform.Find("AwaitEmailConfirmation").gameObject.SetActive(true);
                transform.Find("Register").gameObject.SetActive(false);
            }
        } else {
            var error = "Unable to register";

            // Add the error to the error message
            if (serverResponseSplit.Length > 0) {
                error += ".\n" + serverResponseSplit[0];
            }
            print(error);
            lm.ShowMessage(error, true);
        }
    }

	private string[] SplitServerResponse(string text, string divider, StringSplitOptions splitOption = StringSplitOptions.RemoveEmptyEntries)
	{
		return text.Split(new string[] { divider }, splitOption);
	}

    public IEnumerator ResendActivationEmail()
    {
        if (GlobalData.email.Equals("")) {
            print("Please enter your email");
            lm.ShowMessage("Please enter your email", true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("ACTION", "resendActivation");

        form.AddField("email", GlobalData.email);
        lm.ShowMessage("Sending...");
        print("Sending... (to " + serverAddress + ")");

		string[] serverResponseSplit;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text); //For debugging
			serverResponseSplit = SplitServerResponse(webRequest.downloadHandler.text, "--");
		}

        if (serverResponseSplit.Length > 0 && serverResponseSplit[0].Equals("Successfully resent")) {
            print("Success. Please check email for verification");
            lm.ShowMessage("Success. Please check email for verification");
        } else {
            print("Unable to send email");
            lm.ShowMessage("Unable to send email", true);
        }
    }

    public IEnumerator ForgotPassword()
    {
        if (GlobalData.email.Equals("") && GlobalData.username.Equals("")) {
            print("Please enter your username or email");
            lm.ShowMessage("Please enter your username or email", true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("ACTION", "forgotPassword");

        form.AddField("email", GlobalData.email);
        form.AddField("username", GlobalData.username);
        lm.ShowMessage("Sending...");

		string[] serverResponseSplit;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text); //For debugging
			serverResponseSplit = SplitServerResponse(webRequest.downloadHandler.text, "--");
		}

		if (serverResponseSplit.Length > 0 && serverResponseSplit[0].Equals("Successfully resent")) {
            print("Success. Please check email for password reset");
            lm.ShowMessage("Success. Please check email for password reset");
        } else {
            print("Unable to send email");
            lm.ShowMessage("Unable to send email", true);
        }
    }

    public IEnumerator UpdatePassword()
    {
        WWWForm form = new WWWForm();
        form.AddField("ACTION", "update");

        form.AddField("username", GlobalData.username);
        form.AddField("password", GlobalData.password);
        form.AddField("newPassword", lm.PWord.text);
        lm.ShowMessage("Updating...");

		string[] serverResponseSplit;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text); //For debugging
			serverResponseSplit = SplitServerResponse(webRequest.downloadHandler.text, "--");
		}

		if (serverResponseSplit.Length > 0 && serverResponseSplit[0].Equals("Password updated")) {
            print("Successfully updated password");
            lm.ShowMessage("Successfully updated password");
            lm.LoginInfoGroup.gameObject.SetActive(false);
            lm.submitButton.gameObject.SetActive(false);
        } else {
            print("Unable to update password");
            lm.ShowMessage("Unable to update password", true);
        }
    }

    public IEnumerator Ban(string user)
    {
        WWWForm form = new WWWForm();
        form.AddField("ACTION", "ban");

        form.AddField("user", user);
        lm.ShowMessage("Banning...");

		string[] serverResponseSplit;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text); //For debugging
			serverResponseSplit = SplitServerResponse(webRequest.downloadHandler.text, "--");
		}

		if (serverResponseSplit.Length > 0 && serverResponseSplit[0].StartsWith("Successfully banned")) {
            print("Successfully banned " + user);
            lm.ShowMessage("Successfully banned " + user);
        } else {
            print("Unable to ban");
            lm.ShowMessage("Unable to ban", true);
        }
    }

    public IEnumerator RemoveAccount(string user)
    {
        WWWForm form = new WWWForm();
        form.AddField("ACTION", "deleteAccount");

        form.AddField("user", user);
        lm.ShowMessage("Deleting account...");

		string[] serverResponseSplit;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text); //For debugging
			serverResponseSplit = SplitServerResponse(webRequest.downloadHandler.text, "--");
		}

		if (serverResponseSplit.Length > 0 && serverResponseSplit[0].StartsWith("Successfully removed")) {
            print("Successfully removed " + user);
            lm.ShowMessage("Successfully removed " + user);
            if (!GlobalData.role.Equals(GlobalData.Roles.Admin)) {
                lm.Logout(); //If not an admin, logout because you just deleted your own account
            }
        } else {
            print("Unable to remove");
            lm.ShowMessage("Unable to remove", true);
        }
    }

    public IEnumerator UpdateAccountInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("ACTION", "updateAccount");

        form.AddField("username", GlobalData.username);
        form.AddField("password", lm.accountInfo.currentPassword.text);
        if (!lm.accountInfo.newPassword.text.Equals("")) {
            form.AddField("newPassword", lm.accountInfo.newPassword.text);
        }
        form.AddField("firstname", lm.accountInfo.firstName.text);
        form.AddField("lastname", lm.accountInfo.lastName.text);
        form.AddField("title", lm.accountInfo.title.captionText.text);
        form.AddField("authorName", GetAuthorName(lm.accountInfo.title.captionText.text, lm.accountInfo.firstName.text, lm.accountInfo.lastName.text));

        lm.ShowMessage("Updating information...");

		string[] serverResponseSplit;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text); //For debugging
			serverResponseSplit = SplitServerResponse(webRequest.downloadHandler.text, "--");
		}

		if (serverResponseSplit.Length > 0) {
            if (serverResponseSplit[0].StartsWith("Success")) {
                print("Successfully updated");
                lm.ShowMessage("Successfully updated");
                GlobalData.userFirstName = lm.accountInfo.firstName.text;
                GlobalData.userLastName = lm.accountInfo.lastName.text;
                GlobalData.userTitle = lm.accountInfo.title.captionText.text;
                if (serverResponseSplit.Length > 1 && serverResponseSplit[1].Equals("Completed")) {
                    print("Successfully updated authors of existing cases");
                } else {
                    print("Could not update existing menu cases");
                }
                if (!lm.accountInfo.currentPassword.gameObject.activeInHierarchy) {
                    lm.accountInfo.firstTimeNotice.SetActive(false);
                    lm.accountInfo.currentPassword.transform.parent.gameObject.SetActive(true); //Show this again after first time use from new accounts
                }
                lm.accountInfo.panel.SetActive(false);
            } else if (serverResponseSplit[0].StartsWith("Incorrect password")) {
                print("Incorrect password");
                lm.ShowMessage("Incorrect password", true);
            } else if (serverResponseSplit[0].Equals("Nothing to update")) {
                print("Nothing to update");
                lm.ShowMessage("Nothing to update", true);
            }
        } else {
            print("Server error");
        }
	}

    //Change this in UploadToServer too if changed
    private string GetAuthorName()
    {
        if (GlobalData.userTitle.Equals("--") || GlobalData.userTitle.Equals("")) {
            return GlobalData.userFirstName + " " + GlobalData.userLastName;
        }
        return GlobalData.userTitle + " " + GlobalData.userFirstName + " " + GlobalData.userLastName;
    }

    //Change this in UploadToServer too if changed
    private string GetAuthorName(string title, string fName, string lName)
    {
        if (title.Equals("--")) {
            return fName + " " + lName;
        }
        return title + " " + fName + " " + lName;
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    #region ----------------------------------Handles the page counter-----------------------------------

    /**
	 * Sets the page of the menu to a certain int
	 * 
	 * Loads the menu entries of that page (if any are available)
	 */
    public void SetPage(int i)
    {
        if (i > 0) {
            menuPage = i;
            RemoveMenuItems();
            SpawnMenuItems();
        }
    }

    /**
	 * Increments the page of the menu
	 * 
	 * Loads the menu entries of that page (if any are available)
	 */
    public void IncrementPage()
    {
        if (showingCases) {
            if (menuPage < caseItems.Count / casesPerPageLimit) {
                menuPage++;
                RemoveMenuItems();
                SpawnMenuItems();
            }
        } else {
            if (menuPage < templateItems.Count / casesPerPageLimit) {
                menuPage++;
                RemoveMenuItems();
                SpawnMenuItems();
            }
        }
    }

    /**
	 * Decrements the page of the menu
	 * 
	 * Loads the menu entries of that page
	 */
    public void DecrementPage()
    {
        if (menuPage > 0) {
            menuPage--;
            RemoveMenuItems();
            SpawnMenuItems();
        }
    }

    private void ShowCorrectPageButtons()
    {
        if (!showingCases) {
            ShowCorrectPageButtons(templateItems);
        } else {
            ShowCorrectPageButtons(caseItems);
        }
    }

    /**
	 * Enables or disables the previous/next page buttons as needed
	 */
    private void ShowCorrectPageButtons(List<MenuCase> cases)
    {
        int count;
        if (cases == null) {
            count = 0;
        } else {
            count = cases.Count;
        }

        if (count <= casesPerPageLimit) {// || (menuPage == 0 && caseExampleParent.childCount - 1 <= casesPerPageLimit)) {
            menuPageButtons.gameObject.SetActive(false);
            return;
        } else {
            menuPageButtons.gameObject.SetActive(true);
        }

        menuPageButtons.Find("Page").GetComponent<TextMeshProUGUI>().text = "Page: " + (menuPage + 1) + "/" + ((count + 1) / casesPerPageLimit + 1);

        if (menuPage == count / casesPerPageLimit) {
            menuPageButtons.Find("NextPageButton").GetComponent<Button>().interactable = false;
        } else {
            menuPageButtons.Find("NextPageButton").GetComponent<Button>().interactable = true;
        }

        if (menuPage > 0) {
            menuPageButtons.Find("PreviousPageButton").GetComponent<Button>().interactable = true;
        } else {
            menuPageButtons.Find("PreviousPageButton").GetComponent<Button>().interactable = false;
        }
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    #region -------------------------Changing Display Mode (Template, list, etc)-------------------------

    /**
	 * Determines whether or not the user is shown templates or cases
	 */
    public void ShowCases(bool b)
    {
        if (showingCases == b) {
            //return;
        }
        showingCases = b;
        RemoveMenuItems();
        SpawnMenuItems();
    }

    /**
	 * Sets the parent transform that the cases/templates will spawn under
	 * This means that we are switching tab things, so reset the page count
	 */
    public void SetItemParent(Transform t)
    {
        caseExampleParent = t;
        menuPage = 0;
    }

    public void ToggleListView()
    {
        isList = !isList;
        SpawnAppropriateContent();
    }

    public void SpawnAppropriateContent()
    {
        if (showingCourses) {
            GameObject content = Instantiate(Resources.Load("Menu/Prefabs/Courses/CourseContent") as GameObject, caseExampleParent.parent);
            Destroy(caseExampleParent.gameObject);
            caseExampleParent = content.transform;
            content.SetActive(true);
            caseExampleParent.GetComponentInParent<ScrollRect>().content = content.GetComponent<RectTransform>();

            //Swap titles at the top
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/YourCases").gameObject.SetActive(false);
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/BrowseCases").gameObject.SetActive(false);
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/Courses").gameObject.SetActive(true);

            SpawnCourses();
        } else {
            GameObject newContent;
            if (GlobalData.GDS.isMobile && isList) {
                newContent = Instantiate(Resources.Load("Menu/Prefabs/MobileTMPListContent") as GameObject, caseExampleParent.parent);
            } else if (isList) {
                newContent = Instantiate(Resources.Load("Menu/Prefabs/TMPListContent") as GameObject, caseExampleParent.parent);
            } else if (GlobalData.GDS.isMobile) {
                newContent = Instantiate(Resources.Load("Menu/Prefabs/MobileGridContent") as GameObject, caseExampleParent.parent);
            } else {
                newContent = Instantiate(Resources.Load("Menu/Prefabs/GridContent") as GameObject, caseExampleParent.parent);
            }
            Destroy(caseExampleParent.gameObject);
            caseExampleParent = newContent.transform;
            if (isWriter) //GlobalData.resourcePath.Equals("Writer"))
            {
                caseExampleParent.Find("CreateNew").GetComponent<Button>().onClick.AddListener(delegate {
                    transform.Find("PatientNamePanel").gameObject.SetActive(true);
                    transform.Find("PatientNamePanel/BackgroundPanel/Content/DescriptionText").gameObject.SetActive(false);
                });

                if (caseExampleParent.parent.parent.parent.Find("NoCasesNotification").gameObject.activeInHierarchy)
                    firstWritePanel.SetActive(true);

            } else {
                DisableNewCase();
            }
            newContent.SetActive(true);
            var scrollRect = caseExampleParent?.GetComponentInParent<ScrollRect>();
            if (scrollRect) {
                scrollRect.content = newContent?.GetComponent<RectTransform>();
            } else {
                Debug.LogError("Case Example Parent ScrollRect not found. This should be debugged further.");
            }

            //Swap titles at the top
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/YourCases").gameObject.SetActive(true);
            if (GlobalData.GDS.isMobile || GlobalData.resourcePath.Equals("Reader")) {
                transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/BrowseCases").gameObject.SetActive(true);
            } else {
                transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/TemplateCases").gameObject.SetActive(true);
            }
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/Courses").gameObject.SetActive(false);

            SpawnMenuItems();
        }
    }


    public void DisableNewCase()
    {
        caseExampleParent.Find("CreateNew").gameObject.SetActive(false);
    }

    public void SetIsWriter(bool b)
    {
        // This is called whenever you start writer or reader, so setting the menu page to the first should happen
        menuPage = 0;

        isWriter = b;
        if (isWriter) {
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/YourCases").GetComponent<Button>().onClick.Invoke();
            if (GlobalData.accountId == 0 || lm.accountInfo.firstTimeNotice.activeInHierarchy) { //Will only be active for new accounts
                                                                                                 //transform.Find("WriterQuickStart").gameObject.SetActive(true);
            }
            GlobalData.resourcePath = "Writer";
        } else {
            transform.Find("ContentPanel/SectionButtonsPanel/PanelButtons/BrowseCases").GetComponent<Button>().onClick.Invoke();
            if (GlobalData.accountId == 0 || lm.accountInfo.firstTimeNotice.activeInHierarchy) { //Will only be active for new accounts
                                                                                                 //transform.Find("ReaderQuickStart").gameObject.SetActive(true);
            }
            GlobalData.resourcePath = "Reader";
        }
    }

    public bool GetIsWriter()
    {
        return isWriter;
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    #region ----------------------Handles Menu Case GameObjects and Loads Side Panel---------------------


    public void Deselect(Image patientImage, Button caseObjButton)
    {
        if (!patientImage.transform.GetComponent<Toggle>().isOn) {
            patientImage.color = new Color(1f, 1f, 1f, 1f);
        } else {
            //patientImage.color = patientImage.transform.GetChild(0).GetComponent<Image>().color;
            caseObjButton.onClick.Invoke();
        }
    }

    public void ClearMenuItems()
    {
        caseItems.Clear();
        templateItems.Clear();
    }

    /**
	 * This is called when logging out
	 */
    public void RemoveMenuItems()
    {
        if (caseExampleParent == null) { //This should happen when we're not testing login on Cassidy's menu
            print("No menu cases to destroy");
            return;
        }
        for (int i = 0; i < caseExampleParent.childCount; i++) {
            if (caseExampleParent.GetChild(i).name.StartsWith("CaseExample")) {
                Destroy(caseExampleParent.GetChild(i).gameObject);
            }
        }
    }

    /**
	 * Changes the edit buttons to the reader-specific button across the menu
	 */
    public void ChangeEditButtons()
    {
        return;
        /*for (int i = 0; i < caseExampleParent.childCount; i++) {
			if (caseExampleParent.GetChild(i).name.Equals("CreateNew")) {
				continue;
			}
			Transform editButton = caseExampleParent.GetChild(i).Find("ImageRows/Rows/Row0/EditButton");
			editButton.Find("Edit").gameObject.SetActive(false);
			editButton.Find("View").gameObject.SetActive(true);
		}

		infoPanel.Find("ButtonRow/Edit/Edit").gameObject.SetActive(false);
		infoPanel.Find("ButtonRow/Edit/View").gameObject.SetActive(true);*/
    }

    void LoadInfoPanel(MenuCase m)
    {
        GlobalData.caseObj = m;

        //Mobile build
        if (GlobalData.GDS.isMobile) {
            infoPanel.Find("InformationPanel/AuthorInfo/TextName").GetComponent<TextMeshProUGUI>().text = m.patientName.Replace("_", " ");
            if (m.accountId == GlobalData.accountId && m.localOnly) {
                m.authorName = GetAuthorName();
            }
            infoPanel.Find("InformationPanel/AuthorInfo/TextAuthor").GetComponent<TextMeshProUGUI>().text = "by " + m.authorName;
            infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextAudience").GetComponent<TextMeshProUGUI>().text = m.audience;
            infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextDifficulty").GetComponent<TextMeshProUGUI>().text = m.difficulty;
            infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextCategory").GetComponent<TextMeshProUGUI>().text = m.GetTagsAsOneString();
            infoPanel.Find("InformationPanel/CaseInfo/LeftSide/CaseDescription/Description/Text").GetComponent<TextMeshProUGUI>().text = m.summary;
            infoPanel.Find("InformationPanel/AuthorInfo/ShortCaseDescription").GetComponent<TextMeshProUGUI>().text = m.description;
            infoPanel.Find("InformationPanel/AuthorInfo/ShortCaseDescription").GetComponent<TextMeshProUGUI>().text = m.description;
            infoPanel.Find("InformationPanel/Filename").GetComponent<TextMeshProUGUI>().text = m.filename;//.Replace(" ", "_");

            //Case last edit time
            infoPanel.Find("InformationPanel/CaseInfo/RightSide/Developer").gameObject.SetActive(true);
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //Convert from seconds from unix epoch to local DateTime
            infoPanel.Find("InformationPanel/CaseInfo/RightSide/Developer/DateModified").GetComponent<TextMeshProUGUI>().text = "Last Modified: " + unixEpoch.AddSeconds(m.dateModified).ToLocalTime();

            foreach (Transform t in infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextDifficulty").GetComponentsInChildren<Transform>()) {
                if (!t.name.Equals("InformationPanel/CaseInfo/RightSide/TextDifficulty")) {
                    t.gameObject.SetActive(true);
                }
            }
            Transform imageParent = infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextDifficulty");
            switch (m.difficulty) {
                case "Beginner":
                    imageParent.GetChild(1).gameObject.SetActive(false);
                    imageParent.GetChild(2).gameObject.SetActive(false);
                    imageParent.GetChild(3).gameObject.SetActive(true);
                    //infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextDifficulty/BeginnerImage").gameObject.SetActive(true);
                    //infoPanel.Find("TextDifficulty/BeginnerImage").GetComponent<Toggle>().isOn = true;
                    break;
                case "Intermediate":
                    imageParent.GetChild(1).gameObject.SetActive(false);
                    imageParent.GetChild(2).gameObject.SetActive(true);
                    imageParent.GetChild(3).gameObject.SetActive(false);
                    //infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextDifficulty/IntermediateImage").gameObject.SetActive(true);
                    //infoPanel.Find("TextDifficulty/IntermediateImage").GetComponent<Toggle>().isOn = true;
                    break;
                case "Advanced":
                    imageParent.GetChild(1).gameObject.SetActive(true);
                    imageParent.GetChild(2).gameObject.SetActive(false);
                    imageParent.GetChild(3).gameObject.SetActive(false);
                    //infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextDifficulty/AdvancedImage").gameObject.SetActive(true);
                    //infoPanel.Find("TextDifficulty/AdvancedImage").GetComponent<Toggle>().isOn = true;
                    break;
                default:
                    infoPanel.Find("InformationPanel/CaseInfo/RightSide/TextDifficulty/AdvancedImage").gameObject.SetActive(true);
                    break;
            }

            Transform yourRatingParent = infoPanel.Find("InformationPanel/RatingsPanel/YourRating");
            Transform averageRatingParent = infoPanel.Find("InformationPanel/RatingsPanel/AverageRating");
            foreach (Transform t in yourRatingParent) {
                t.gameObject.SetActive(false);
            }
            foreach (Transform t in averageRatingParent) {
                t.gameObject.SetActive(false);
            }

            //Your rating
            yourRatingParent.GetChild(Tracker.GetCaseData(m.recordNumber).caseRating).gameObject.SetActive(true);
            //Average rating
            averageRatingParent.GetChild(m.rating).gameObject.SetActive(true);

        }
        //Desktop build
        else {
            infoPanel.Find("TextName").GetComponent<TextMeshProUGUI>().text = m.patientName.Replace("_", " ");
            if (m.accountId == GlobalData.accountId && m.localOnly) {
                m.authorName = GetAuthorName();
            }
            infoPanel.Find("TextAuthor").GetComponent<TextMeshProUGUI>().text = "by " + m.authorName;
            infoPanel.Find("TextAudience").GetComponent<TextMeshProUGUI>().text = m.audience;
            infoPanel.Find("TextDifficulty").GetComponent<TextMeshProUGUI>().text = m.difficulty;
            infoPanel.Find("TextCategory").GetComponent<TextMeshProUGUI>().text = m.GetTagsAsOneString();
            infoPanel.Find("CaseDescription/Description/Text").GetComponent<TextMeshProUGUI>().text = m.summary;
            infoPanel.Find("ShortCaseDescription").GetComponent<TextMeshProUGUI>().text = m.description;
            infoPanel.Find("Filename").GetComponent<TextMeshProUGUI>().text = m.filename;//.Replace(" ", "_");

            //Case last edit time
            infoPanel.Find("Developer").gameObject.SetActive(true);
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //Convert from seconds from unix epoch to local DateTime
            infoPanel.Find("Developer/DateModified").GetComponent<TextMeshProUGUI>().text = "Last Modified: " + unixEpoch.AddSeconds(m.dateModified).ToLocalTime();

            foreach (Transform t in infoPanel.Find("TextDifficulty").GetComponentsInChildren<Transform>()) {
                if (!t.name.Equals("TextDifficulty")) {
                    t.gameObject.SetActive(false);
                }
            }

            switch (m.difficulty) {
                case "Beginner":
                    infoPanel.Find("TextDifficulty/BeginnerImage").gameObject.SetActive(true);
                    //infoPanel.Find("TextDifficulty/BeginnerImage").GetComponent<Toggle>().isOn = true;
                    break;
                case "Intermediate":
                    infoPanel.Find("TextDifficulty/IntermediateImage").gameObject.SetActive(true);
                    //infoPanel.Find("TextDifficulty/IntermediateImage").GetComponent<Toggle>().isOn = true;
                    break;
                case "Advanced":
                    infoPanel.Find("TextDifficulty/AdvancedImage").gameObject.SetActive(true);
                    //infoPanel.Find("TextDifficulty/AdvancedImage").GetComponent<Toggle>().isOn = true;
                    break;
                default:
                    infoPanel.Find("TextDifficulty/AdvancedImage").gameObject.SetActive(true);
                    break;
            }

            infoPanel.Find("LocalButtonRow").gameObject.SetActive(m.downloaded);
            infoPanel.Find("ServerButtonRow").gameObject.SetActive(!m.downloaded && m.accountId == GlobalData.accountId);
            infoPanel.Find("DownloadButtonRow").gameObject.SetActive(!m.downloaded && m.accountId != GlobalData.accountId);
        }

        LoadInfoPanelButtons(m);
    }

    private void LoadInfoPanelButtons(MenuCase m)
    {
        string mobileAdjustment = "";
        Button infoPanelEdit = null;
        Button infoPanelRead = null;
        //Button yourTemplateNew = null;
        //Button yourTemplateEdit = null;

        var yourTemplate = (m.caseType == MenuCase.CaseType.privateTemplate || m.caseType == MenuCase.CaseType.publicTemplate) &&
                selected.accountId == GlobalData.accountId;

        if (GlobalData.GDS.isMobile) {
            mobileAdjustment = "InformationPanel/";
            infoPanelEdit = infoPanel.Find(mobileAdjustment + "CaseInfo/RightSide/ReaderWriterButtons/OpenWriter").GetComponent<Button>();
            infoPanelRead = infoPanel.Find(mobileAdjustment + "CaseInfo/RightSide/ReaderWriterButtons/OpenReader").GetComponent<Button>();
        } else {
            if (yourTemplate) {
                infoPanelEdit = infoPanel.Find("ReaderWriterButtons/YourTemplate/EditTemplate").GetComponent<Button>();
                infoPanelRead = infoPanel.Find("ReaderWriterButtons/YourTemplate/NewCase").GetComponent<Button>();

                infoPanel.Find("ReaderWriterButtons/OpenWriter").gameObject.SetActive(false);
                infoPanel.Find("ReaderWriterButtons/OpenReader").gameObject.SetActive(false);
            } else {
                infoPanelEdit = infoPanel.Find("ReaderWriterButtons/OpenWriter").GetComponent<Button>();
                infoPanelRead = infoPanel.Find("ReaderWriterButtons/OpenReader").GetComponent<Button>();

                infoPanel.Find("ReaderWriterButtons/YourTemplate/NewCase").gameObject.SetActive(false);
                infoPanel.Find("ReaderWriterButtons/YourTemplate/EditTemplate").gameObject.SetActive(false);
            }
        }
        infoPanelEdit.onClick.RemoveAllListeners();
        infoPanelRead.onClick.RemoveAllListeners();
        //Set the buttons in the info panel to load the case

        if (!showingCases) {
            infoPanelRead.GetComponentInChildren<TextMeshProUGUI>().text = "New Case";
        } else {
            infoPanelRead.GetComponentInChildren<TextMeshProUGUI>().text = "View Case";
        }

        if (selected.accountId == GlobalData.accountId) {
            //infoPanel.Find("ButtonRow/Delete").gameObject.SetActive(true);
            if (!GlobalData.demo && !GlobalData.GDS.isMobile && isWriter && !yourTemplate) {
                //infoPanel.Find(mobileAdjustment + "ButtonRow/ExportButton").gameObject.SetActive(true);
                infoPanel.Find(mobileAdjustment + "SaveAsButton").gameObject.SetActive(true);
            } else {
                //infoPanel.Find(mobileAdjustment + "ButtonRow/ExportButton").gameObject.SetActive(false);
                infoPanel.Find(mobileAdjustment + "SaveAsButton").gameObject.SetActive(false);
            }

            if (isWriter) {
                infoPanelEdit.gameObject.SetActive(true);
                if (showingCases) {
                    infoPanelRead.gameObject.SetActive(false);
                } else {
                    infoPanelRead.gameObject.SetActive(true);
                }
                infoPanelEdit.onClick.AddListener(delegate {
                    GlobalData.resourcePath = "Writer";
                    GlobalData.createCopy = false;
                    GlobalData.firstName = "";
                    GlobalData.lastName = "";
                    ButtonLoadScene(infoPanelEdit); //, caseObj);
                });
            } else {
                infoPanelEdit.gameObject.SetActive(false);
                infoPanelRead.gameObject.SetActive(true);
            }
        } else {
            infoPanelEdit.gameObject.SetActive(false);
            infoPanelRead.gameObject.SetActive(true);
            infoPanel.Find(mobileAdjustment + "SaveAsButton").gameObject.SetActive(false);
            //infoPanel.Find(mobileAdjustment + "ButtonRow/ExportButton").gameObject.SetActive(false);
            //infoPanel.Find("ButtonRow/Delete").gameObject.SetActive(false);
        }

        infoPanelRead.onClick.AddListener(delegate {
            if (!showingCases) {
                GlobalData.createCopy = true;
                GlobalData.resourcePath = "Writer";
                transform.Find("PatientNamePanel").gameObject.SetActive(true);
                transform.Find("PatientNamePanel/BackgroundPanel/Content/DescriptionText").gameObject.SetActive(false);
                SetSelectedButton(infoPanelRead);
                return;
            } else {

                print(m.course);
                if (m.course != null) {
                    GlobalData.courseCases = new LinkedList<MenuCase>();
                    foreach (string s in m.course.GetCases()) {
                        GlobalData.courseCases.AddLast(GetCaseByRecordNumber(s));
                    }
                    GlobalData.recommendedCases = new MenuCase[1];
                    if (m.GetIndexInCourse() < m.course.GetCases().Count - 1) {
                        GlobalData.recommendedCases[0] = GetCaseByRecordNumber(m.course.GetCases()[m.GetIndexInCourse() + 1]);
                    }
                }

                GlobalData.createCopy = false;
                GlobalData.resourcePath = "Reader";
                ButtonLoadScene(infoPanelRead); //, caseObj);
            }
        });

        if (!GlobalData.GDS.isMobile) {
            Button infoPanelSaveAs = infoPanel.Find(mobileAdjustment + "SaveAsButton").GetComponent<Button>();
            infoPanelSaveAs.onClick.RemoveAllListeners();
            infoPanelSaveAs.onClick.AddListener(delegate {
                GlobalData.createCopy = true;
                GlobalData.resourcePath = "Writer";
                transform.Find("PatientNamePanel").gameObject.SetActive(true);
                transform.Find("PatientNamePanel/BackgroundPanel/Content/DescriptionText").gameObject.SetActive(true);
                SetSelectedButton(infoPanelRead);
            });
        }
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    #region ---------------------------------------Case Searching----------------------------------------

    /**
	 * Lets users search through cases
	 */
    public List<MenuCase> SearchCases()
    {
        string searchBar = sideSearchPanel.Find("TMPInputField/SearchBar").GetComponent<TMP_InputField>().text;

        //Find all active toggles. ToggleGroups only allow one toggle to be active at a time so I just removed them for now. I may implement a custom script to store active toggles as a list
        //Get category toggles
        List<Toggle> categoryToggles, difficultyToggles, audienceToggles, orderToggles;
        if (GlobalData.GDS?.isMobile == true) {
            categoryToggles = new List<Toggle>(sideSearchPanel.Find("Scroll View/Viewport/Content/FiltersPanel/CategoryPanel").GetComponentsInChildren<Toggle>());
            difficultyToggles = new List<Toggle>(sideSearchPanel.Find("Scroll View/Viewport/Content/FiltersPanel/DifficultyPanel").GetComponentsInChildren<Toggle>());//GetComponent<ToggleGroup>().ActiveToggles());
            audienceToggles = new List<Toggle>(sideSearchPanel.Find("Scroll View/Viewport/Content/FiltersPanel/AudiencePanel").GetComponentsInChildren<Toggle>());//.GetComponent<ToggleGroup>().ActiveToggles());
            orderToggles = new List<Toggle>(sideSearchPanel.Find("Scroll View/Viewport/Content/FiltersPanel/OrderPanel").GetComponentsInChildren<Toggle>());//.GetComponent<ToggleGroup>().ActiveToggles());
            orderToggles = orderToggles.FindAll(tog => tog.isOn);
        } else {
            categoryToggles = new List<Toggle>(sideSearchPanel.Find("Scroll View/Viewport/Content/FiltersPanel/CategoryPanel").GetComponentsInChildren<Toggle>());
            difficultyToggles = new List<Toggle>(sideSearchPanel.Find("Scroll View/Viewport/Content/FiltersPanel/DifficultyPanel").GetComponentsInChildren<Toggle>());//GetComponent<ToggleGroup>().ActiveToggles());
            audienceToggles = new List<Toggle>(sideSearchPanel.Find("Scroll View/Viewport/Content/FiltersPanel/AudiencePanel").GetComponentsInChildren<Toggle>());//.GetComponent<ToggleGroup>().ActiveToggles());                                                                                                                                                        //orderToggles = new List<Toggle>(sideSearchPanel.Find("FiltersPanel/OrderPanel").GetComponentsInChildren<Toggle>());//.GetComponent<ToggleGroup>().ActiveToggles());
        }

        categoryToggles = categoryToggles.FindAll(tog => tog.isOn);
        //Get difficulty toggles
        difficultyToggles = difficultyToggles.FindAll(tog => tog.isOn);
        //Get audience toggles
        audienceToggles = audienceToggles.FindAll(tog => tog.isOn);
        //Get other toggles
        //Order

        int recordNumber = -1;
        //Check to see if the user is searching for a recordNumber
        List<MenuCase> recordNumberCase = new List<MenuCase>();
        string[] s = searchBar.Split(',');
        for (int i = 0; i < s.Length; i++) {
            string searchItem = s[i].Trim(' ');
            if (int.TryParse(searchItem, out recordNumber)) {
                if (recordNumber < 0 || recordNumber >= 1000000) { //invalid record number
                    print("Invalid record number. No cases will be returned");
                    continue; //return null;
                }
                recordNumberCase.Add(caseItems.Find(m => m.recordNumber.Equals(recordNumber.ToString())));
                recordNumberCase.Add(templateItems.Find(m => m.recordNumber.Equals(recordNumber.ToString())));
                if (recordNumberCase.Count == 0 || recordNumberCase[i] == null) {
                    recordNumberCase.RemoveAt(i);
                    continue; //return recordNumberCase;
                }

                if (recordNumberCase[i].accountId != GlobalData.accountId) {
                    if (showOwnedCasesOnly) {
                        //print("You do not own the case " + recordNumberCase[0].filename + ", Will not display. Please disable Only Show Owned Cases to view");
                        recordNumberCase.RemoveAt(i);
                        continue; // return null;
                    } else if (!recordNumberCase[i].IsPublic()) {
                        print("This case is private. You do not have access to it");
                        recordNumberCase.RemoveAt(i);
                        continue; // return null;
                    }
                }
                continue;// return recordNumberCase;
            }
        }

        if (recordNumberCase.Count > 0) {
            return recordNumberCase;
        }


        if (showingCases) {
            return caseItems.FindAll(m =>
                ContainsTag(m.tags, categoryToggles) &&
                ContainsToggle(m.difficulty, difficultyToggles) &&
                ContainsToggle(m.audience, audienceToggles) &&
                //ContainsToggle(m.audience, otherToggles) &&
                CheckOtherFields(m, searchBar)
                );
        } else {
            return templateItems.FindAll(m =>
                ContainsTag(m.tags, categoryToggles) &&
                ContainsToggle(m.difficulty, difficultyToggles) &&
                ContainsToggle(m.audience, audienceToggles) &&
                //ContainsToggle(m.audience, otherToggles) &&
                CheckOtherFields(m, searchBar)
                );
        }
    }

    private bool ContainsTag(string[] tags, List<Toggle> toggles)
    {
        //If the user didn't select anything, just show all available results
        if (toggles.Count == 0) {
            return true;
        }
        List<string> tagList = new List<string>(tags);
        if (toggles.Exists(toggle => tagList.Contains(toggle.name))) {
            return true;
        }
        return false;
    }

    private bool ContainsToggle(string item, List<Toggle> toggles)
    {
        //If the user didn't select anything, just show all available results
        if (toggles.Count == 0) {
            return true;
        }
        if (toggles.Exists(toggle => toggle.name.GetHashCode() == item.GetHashCode())) {
            return true;
        }
        return false;
    }

    private bool CheckOtherFields(MenuCase m, string searchBar)
    {
        if (m.accountId != GlobalData.accountId) {
            if (showOwnedCasesOnly) {
                //print("You do not own the case " + m.filename + ", Will not display. Please disable Only Show Owned Cases to view");
                return false;
            } else if (!m.IsPublic()) {
                //print("This case is private. You do not have access to it");
                return false;
            }
        } else if (!showOwnedCasesOnly && showingCases) { //If browsing, don't show user's private cases
            if (!m.IsPublic()) {
                //The user's case is private. Do not show it under browsed cases
                return false;
            }
        }

        searchBar = searchBar.ToLower();
        foreach (string search in searchBar.Split(',')) {
            string searchElement = search.Trim(' ');
            if (m.authorName.ToLower().Contains(searchElement)) {
                continue;
            }

            if (m.patientName.ToLower().Contains(searchElement)) {
                continue;
            }

            if (m.description.ToLower().Contains(searchElement)) {
                continue;
            }

            if (m.CheckCaseTags(searchElement)) {
                continue;
            }
            return false;
        }
        return true;
    }

    public void SelectAllToggles(Transform toggleParent)
    {
        selectingAllTags = true;
        NextFrame.Function(delegate { selectingAllTags = false; SpawnMenuItems(); });
        List<Toggle> toggles = new List<Toggle>(toggleParent.GetComponentsInChildren<Toggle>());
        bool state = transform.Find("Select All").GetComponent<Toggle>().isOn;
        foreach (Toggle t in toggles) {
            t.isOn = state;
        }
    }

    public void ToggleShowOwnedCases(Toggle t)
    {
        showOwnedCasesOnly = !showOwnedCasesOnly;
    }

    public void SetShowOwnedCases(bool b)
    {
        showOwnedCasesOnly = b;
        menuPage = 0;
        GlobalData.caseObj = null;
    }

    public void SortCases(ref List<MenuCase> casesToShow)
    {
        MenuCase.SortMethod enumVal = (MenuCase.SortMethod)caseSortDropdown.value;
        if (GlobalData.GDS?.isMobile == true) {
            List<Toggle> toggles = new List<Toggle>(casesSortToggleGroup.ActiveToggles());
            if (toggles.Count == 0) {
                return;
            }
            enumVal = (MenuCase.SortMethod)toggles[0].transform.GetSiblingIndex();
        } else {
            enumVal = (MenuCase.SortMethod)caseSortDropdown.value;
        }


        switch (enumVal) {
            case MenuCase.SortMethod.PatientNameDown:
                casesToShow.Sort(casesToShow[0].ComparePatientNameDown);
                break;
            case MenuCase.SortMethod.PatientNameUp:
                casesToShow.Sort(casesToShow[0].ComparePatientNameUp);
                break;
            case MenuCase.SortMethod.AuthorDown:
                casesToShow.Sort(casesToShow[0].CompareAuthorDown);
                break;
            case MenuCase.SortMethod.AuthorUp:
                casesToShow.Sort(casesToShow[0].CompareAuthorUp);
                break;
            case MenuCase.SortMethod.DateDown:
                casesToShow.Sort(casesToShow[0].CompareDateDown);
                break;
            case MenuCase.SortMethod.DateUp:
                casesToShow.Sort(casesToShow[0].CompareDateUp);
                break;
            case MenuCase.SortMethod.None: //Shouldn't happen
                break;
        }
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    #region -----------------------------Downloading and Showing Menu Cases------------------------------

    /**
	 * Downloads all available and applicable menu files to display on the main manu.
	 * Returns them as a MenuCase item
	 */
    public IEnumerator DownloadMenuItems()
    {
        //If there's no infoPanel, you're using a different menu than Cassidy's. Return to prevent breaking.
        print("Getting menu items");
        if (infoPanel == null && !(infoPanel = transform.Find("ContentPanel/SectionButtonsPanel/CaseOverviewPanel/InformationPanel"))) {
            yield break;
        }

        StartCoroutine(DownloadCourses());
        //InitCourses();

        //Download all appropriate cases from the server (server imposed limit of 15 at a time currently
        caseItems = new List<MenuCase>();
        templateItems = new List<MenuCase>();
        listsAreLoaded = false;
        string[] items = null;
        if (GlobalData.demo) {
            //string demoMenuItems = System.IO.File.ReadAllText(Application.streamingAssetsPath + GlobalData.menuFilePath);
            //items = demoMenuItems.Split(new string[] { "::" }, StringSplitOptions.None);
            transform.Find("ContentPanel/SectionButtonsPanel/YourCases CaseSelector/Scroll View/DownloadingCasesNotification").gameObject.SetActive(false);
        } else {
            caseExampleParent.parent.parent.parent.Find("NoCasesNotification").gameObject.SetActive(false);
            if (File.Exists(menuFilePath)) {
                items = File.ReadAllText(menuFilePath).Split(new string[] { "::" }, StringSplitOptions.None);
            } else {
                print("Downloading menu items...");
                if (!GlobalData.offline) {
                    string serverURL = GlobalData.serverAddress + "Menu.php";
                    string urlParams = "?webfilename=" + "" + "&mode=downloadForOneAccount"; //&webusername=clinical&webpassword=encounters
                    List<string> tags = new List<string>(); //Load this for tags
                    string search = transform.Find("SidePanel/MainPanel/YourCasesMenuPanel/InputField1Line").GetComponent<InputField>().text;
                    urlParams = urlParams + "&page=" + menuPage + "&search=" + search + "&tags=" + string.Join("+", tags);
					using (UnityWebRequest webRequest = UnityWebRequest.Get(serverURL + urlParams + "&account_id=" + GlobalData.accountId)) {
						webRequest.SendWebRequest();
						isDownloadingAllCases = true;
						while (!webRequest.isDone) {
							if (disposeDownload) { //Use this to discard the downloaded data
								print("Discarded");
								disposeDownload = false;
								webRequest.Dispose();
								yield break;
							}
							yield return null;
						}

						isDownloadingAllCases = false;

						//Split the returned text into different MenuCases, each one separated by "::"
						items = webRequest.downloadHandler.text.Split(new string[] { "::" }, StringSplitOptions.None);

						// If there's no items, this usually means we can't connect to the server
						// Only the guest should be able to reach this
						if (items.Length == 0) {
							GlobalData.offline = true;
						}
					}
                } else {
                    items = new string[0];
                }
            }

            transform.Find("ContentPanel/SectionButtonsPanel/YourCases CaseSelector/Scroll View/DownloadingCasesNotification").gameObject.SetActive(false);
            transform.Find("ContentPanel/SectionButtonsPanel/YourCases CaseSelector/Scroll View/DownloadingCasesNotification").GetComponent<Animator>().StartPlayback();
        }
        if (items != null) {
            print(items.Length);
            if (!GlobalData.demo && items.Length == 0) {
                print("Error connecting to the server. Login again to retry connection");
                //lm.ShowMessage("Error connecting to the server. Login again to retry connection", true);
            }
        } else {
            items = new string[] { "" }; //Initialize empty array to avoid null references
        }

        MenuCase m = null; //For referencing new MenuCases
        string[] parsedItem;
        foreach (string item in items) {
            if (item.Equals("")) {
                continue;
            }
            //Split each data string of the current MenuCase, each string divided by "--"
            parsedItem = item.Split(new string[] { "--" }, StringSplitOptions.None);
            if (parsedItem.Length > 14) {
                //print(string.Join("--", parsedItem));
                //Check to see if the current user has access to the menu case
                MenuCase.CaseType type = (MenuCase.CaseType)int.Parse(parsedItem[13]);
                if (GlobalData.accountId != int.Parse(parsedItem[0]) && (type == MenuCase.CaseType.privateCase || type == MenuCase.CaseType.privateTemplate)) {
                    continue;
                }

                //Cases
                if (type == MenuCase.CaseType.privateCase || type == MenuCase.CaseType.publicCase) {
                    //Refer to MenuCase constructor to see which each item is
                    /*caseItems.Add(m = new MenuCase(parsedItem[0], parsedItem[1], parsedItem[2], parsedItem[3],
												parsedItem[4], parsedItem[5], parsedItem[6], parsedItem[7],
												parsedItem[8], parsedItem[9], parsedItem[10], parsedItem[11],
												parsedItem[12], parsedItem[13]));*/
                    caseItems.Add(m = new MenuCase(parsedItem));
                }
                //Templates
                else if (type == MenuCase.CaseType.privateTemplate || type == MenuCase.CaseType.publicTemplate) {
                    //Refer to MenuCase constructor to see which each item is
                    /*templateItems.Add(m = new MenuCase(parsedItem[0], parsedItem[1], parsedItem[2], parsedItem[3],
												parsedItem[4], parsedItem[5], parsedItem[6], parsedItem[7],
												parsedItem[8], parsedItem[9], parsedItem[10], parsedItem[11],
												parsedItem[12], parsedItem[13]));*/
                    templateItems.Add(m = new MenuCase(parsedItem));
                }

                //Handle anything related to local files
                if (File.Exists(GetLocalSavesFolderPath() + parsedItem[1].Replace("_", " ").Remove(parsedItem[1].Length - 4) + " menu.txt")) {
                    //See which save is newer and load that menu preview
                    DateTime localFileModified = System.IO.File.GetLastWriteTime(GetLocalSavesFolderPath() + parsedItem[1]);
                    DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //Convert from seconds from unix epoch to local DateTime
                    DateTime serverFileModified = unixEpoch.AddSeconds(double.Parse(parsedItem[9])); //Add the seconds as specified by the chosen MenuCase
                    if (DateTime.Compare(serverFileModified.ToLocalTime(), localFileModified.ToLocalTime()) < 0) {
                        //Debug.Log("------" + parsedItem[1] + "------");
                        //Debug.Log(serverFileModified.ToLocalTime());
                        //Debug.Log(localFileModified.ToLocalTime());
                        continue; //Continue and add the local file's menu preview, since that should be more updated than the server's anyways
                    }
                    m.downloaded = true;
                    m.local = true;
                }

                m.localOnly = false;
                m.server = true;
            }
        }

        if (Directory.Exists(GlobalData.filePath)) {
            string[] list = Directory.GetFiles(GlobalData.filePath);
            foreach (string s in list) {
                if (s.EndsWith(" menu.txt")) {
                    parsedItem = File.ReadAllText(s).Split(new string[] { "--" }, StringSplitOptions.None);
                    if (parsedItem.Length <= 1) {
                        continue;
                    }
                    //Avoid cases with duplicate record numbers (Disabling for now because of import/export. Instead check for filename to avoid duplicate entries)
                    /*if (false && (caseItems.Find(mCase => mCase.recordNumber.Equals(parsedItem[4])) != null || templateItems.Find(mCase => mCase.recordNumber.Equals(parsedItem[4])) != null)) {
						//continue;
					} else*/
                    if (caseItems.Find(mCase => mCase.filename.Equals(parsedItem[1].Replace("_", " "))) != null || templateItems.Find(mCase => mCase.filename.Equals(parsedItem[1].Replace("_", " "))) != null) {
                        continue;
                    }

                    if (true) {// || parsedItem.Length == 15) {
                               //Check to see if the current user has access to the menu case
                        MenuCase.CaseType type = (MenuCase.CaseType)int.Parse(parsedItem[13]);
                        if (GlobalData.accountId != int.Parse(parsedItem[0]) && (type == MenuCase.CaseType.privateCase || type == MenuCase.CaseType.privateTemplate)) {
                            continue;
                        }

                        //Cases
                        if (type == MenuCase.CaseType.privateCase || type == MenuCase.CaseType.publicCase) {
                            //Refer to MenuCase constructor to see which each item is
                            /*caseItems.Add(m = new MenuCase(parsedItem[0], parsedItem[1], parsedItem[2], parsedItem[3],
												parsedItem[4], parsedItem[5], parsedItem[6], parsedItem[7],
												parsedItem[8], parsedItem[9], parsedItem[10], parsedItem[11],
												parsedItem[12], parsedItem[13]));*/
                            caseItems.Add(m = new MenuCase(parsedItem));
                        }
                        //Templates
                        else if (type == MenuCase.CaseType.privateTemplate || type == MenuCase.CaseType.publicTemplate) {
                            //Refer to MenuCase constructor to see which each item is
                            /*templateItems.Add(m = new MenuCase(parsedItem[0], parsedItem[1], parsedItem[2], parsedItem[3],
												parsedItem[4], parsedItem[5], parsedItem[6], parsedItem[7],
												parsedItem[8], parsedItem[9], parsedItem[10], parsedItem[11],
												parsedItem[12], parsedItem[13]));*/
                            templateItems.Add(m = new MenuCase(parsedItem));
                        }
                        m.local = true;
                        m.downloaded = true;
                        m.localOnly = true;
                        //Change the author name for local files because they don't get updates to the author name. Need to do this to facilitate case searching
                        if (m.accountId == GlobalData.accountId && GlobalData.userFirstName.Length > 0) {
                            m.authorName = GlobalData.userTitle + " " + GlobalData.userFirstName.Remove(1) + ". " + GlobalData.userLastName;
                        }
                    }
                }
            }
        }

        print(caseItems.Count);
        print(templateItems.Count);

        disposeDownload = false;
        listsAreLoaded = true;

        ApplyTrackedData();
        SpawnMenuItems();
        CheckResumeCase();
    }

    public IEnumerator DownloadCourses()
    {
        string serverURL = GlobalData.serverAddress + "Menu.php";
        string urlParams = "?webfilename=" + "" + "&mode=downloadCourses"; //&webusername=clinical&webpassword=encounters
																		   //List<string> tags = new List<string>(); //Load this for tags
																		   //string search = transform.Find("SidePanel/MainPanel/YourCasesMenuPanel/InputField1Line").GetComponent<InputField>().text;
		string[] coursesStringList;                                                             //urlParams = urlParams + "&page=" + menuPage + "&search=" + search + "&tags=" + string.Join("+", tags);
		using (UnityWebRequest webRequest = UnityWebRequest.Get(serverURL + urlParams + "&account_id=" + GlobalData.accountId)) {
			webRequest.SendWebRequest();

			while (!webRequest.isDone && isDownloadingAllCases) {
				if (disposeDownload) { //Use this to discard the downloaded data
					print("Discarded");
					//disposeDownload = false;
					webRequest.Dispose();
					yield break;
				}
				yield return null;
			}

			print(webRequest.downloadHandler.text);

			//Split the returned text into different MenuCases, each one separated by "::"
			coursesStringList = webRequest.downloadHandler.text.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
		}

        string[] courseSplit;
        Course co;
        courses = new List<Course>();

        foreach (string s in coursesStringList) {
            courseSplit = s.Split(new string[] { "--" }, StringSplitOptions.None);
            co = new Course(courseSplit[0], courseSplit[1], courseSplit[2], new List<string>(courseSplit[3].Split('-')));
            courses.Add(co);
        }
        //For now, i'm using this to avoid compiler errors
        //InitCourses();

        List<string> wwwCourses = new List<string>();
        foreach (Course c in courses) {
            //print(c.ToString());
            wwwCourses.Add(c.ToString());
        }

        if (wwwCourses != null) {
            print(wwwCourses.Count);
            if (!GlobalData.demo && wwwCourses.Count == 0) {
                print("Error connecting to the server. Login again to retry connection");
                //lm.ShowMessage("Error connecting to the server. Login again to retry connection", true);
            }
        } else {
            wwwCourses = new List<string>(1); //Initialize empty array to avoid null references
        }

        //Wait until we have the cases downloaded. This is for validating the courses
        while (!listsAreLoaded)
            yield return null;

        courses = new List<Course>();
        foreach (string item in wwwCourses) {
            if (item.Equals("")) {
                continue;
            }
            //Split each data string of the current Course, each string divided by "--"
            string[] parsedItem = item.Split(new string[] { "--" }, StringSplitOptions.None);
            print(item);
            Course c = new Course(parsedItem[0], parsedItem[1], parsedItem[2], new List<string>(parsedItem[3].Split('-')));
            ValidateCourse(c);
            courses.Add(c);
        }

        //Enable the courses button now that they're ready
        if (transform.Find("SidePanel/MainPanel/ButtonsPanel/CoursesToggle")) {
            transform.Find("SidePanel/MainPanel/ButtonsPanel/CoursesToggle").GetComponent<Toggle>().interactable = true;
            transform.Find("SidePanel/MainPanel/ButtonsPanel/CoursesToggleOverlay")?.gameObject.SetActive(false);
        }
    }

    private void DeleteMenuFile()
    {
        if (File.Exists(menuFilePath))
            File.Delete(menuFilePath);
    }

    /**
	 * Adds a case to the list of available cases
	 * @param respawnCases whether or not to call SpawnMenuItems();
	 */
    public void AddCase(MenuCase m, bool respawnCases)
    {
        //Cases
        if (!m.IsTemplate()) {
            caseItems.Add(m);
        }
        //Templates
        else if (m.IsTemplate()) {
            templateItems.Add(m);
        }

        if (respawnCases) {
            SpawnMenuItems();
        }
    }

    /**
	 * Spawn cases in the menu. Set up to use pages and spawn only a certain number per page
	 */
    public void SpawnMenuItems()
    {

        RemoveMenuItems();

        if (caseItems == null || !lm.loggedIn) { //If the user hasn't logged in, return
            return;
        }

        if (selectingAllTags) {
            return;
        }

        MenuCase m;
        int fillBuffer = 0; //This is used to make sure the page is filled propperly in cases where private/hidden cases are returned in the search
        //string difficulty = ""; //This is for handling the difficulty and changing the image.

        int count = caseItems.Count;
        if (!showingCases || !isWriter) {
            //Viewing templates
            count = templateItems.Count;
            caseExampleParent.Find("CreateNew")?.gameObject.SetActive(false);
            //caseExampleParent.GetChild(0).gameObject.SetActive(false); //Disable the Create New Case button if we're viewing templates
        } else {
            //Viewing cases
            caseExampleParent.Find("CreateNew")?.gameObject.SetActive(true);
            //caseExampleParent.GetChild(0).gameObject.SetActive(true);
        }

        //This will create a list to pull from
        //I will decide later if this is the entire searched list (most likely) then I handle paging before the next loop
        List<MenuCase> casesToShow = SearchCases();
        if (casesToShow != null && casesToShow.Count > 0) {
            count = casesToShow.Count;
            SortCases(ref casesToShow);
            caseExampleParent.parent.parent.parent.Find("NoCasesNotification").gameObject.SetActive(false);

            //Spawn the case object to be instantiated for each object
            string casePrefabName = "CaseExample";

            // GDS is occasionally null when changing scenes or closing the game
            if (!GlobalData.GDS)
                return;
            if (GlobalData.GDS.isMobile && isList) {
                casePrefabName = "MobileTMPCaseListExample";
            } else if (GlobalData.GDS.isMobile && !isList) {
                casePrefabName = "MobileCaseExample";
            } else if (isList) {
                casePrefabName = "TMPCaseListExample";
            }
            GameObject tempCaseObj = Resources.Load("Menu" + "/Prefabs/Panels/" + casePrefabName) as GameObject;

            for (int i = menuPage * casesPerPageLimit; i < ((menuPage + 1) * casesPerPageLimit) + fillBuffer; i++) {
                //Since we don't have bounds checking in our for loop, we manually check here
                if (i == count) {
                    break;
                }

                //Handled before the for loop
                m = casesToShow[i];

                if (m == null) {
                    //Throw error or log error and continue instead?
                    return;
                }

                string fileName = m.filename;

                //Check whether or not the logged in user has access to the case
                //This should happen when menuItems is populated, but this check is just to make sure
                if (GlobalData.accountId != m.accountId && (!m.IsPublic())) {
                    fillBuffer++;
                    continue;
                }

                //Removing templates from the main cases
                if (showingCases && (m.IsTemplate())) { //If browsing cases, and this case is a template, add to the buffer
                    fillBuffer++;
                    continue;
                } else if (!showingCases && (!m.IsTemplate())) { //If browsing templates, and this case isn't a template, add to the buffer
                    fillBuffer++;
                    continue;
                }

                GameObject caseObj = Instantiate(tempCaseObj, caseExampleParent);
                caseObj.name = "CaseExample";

                //Handle the different views and set all the GameObject textboxes to what they should be
                m.PopulateCaseTransform(caseObj, isList);
                InitCaseButtons(caseObj, m);

                //Check to see if there's a local file and, if there is, alert the user if the server has a more recent version.
                if (m.localOnly && GlobalData.GDS.developer) {
                    caseObj.transform.Find("IsLocal").gameObject.SetActive(true);
                    caseObj.transform.Find("IsLocal").GetComponent<Toggle>().isOn = true;
                }
                if (CheckIfOutdatedLocalCase(m)) {
                    caseObj.transform.Find("ServerNotice").gameObject.SetActive(true);
                }
            }

            tempCaseObj = null;
        } else {
            if (!isDownloadingAllCases) {
                caseExampleParent.parent.parent.parent.Find("NoCasesNotification").gameObject.SetActive(true);
            }
            //transform.Find("ContentPanel/SectionButtonsPanel/YourCases CaseSelector/ScrollView/NoCasesNotification").gameObject.SetActive(true);
        }

        if (GlobalData.resourcePath.Equals("Reader")) {
            //ChangeEditButtons();
        }
        /*
		foreach(MenuCase mCase in caseItems) {
			print ("Case: " + mCase.ToString ());
		}
		foreach (MenuCase mCase in templateItems) {
			print("Template: " + mCase.ToString());
		}*/

        ShowCorrectPageButtons(casesToShow);
    }

    /// <summary>
    /// This applies tracked data such as case completed and rating
    /// </summary>
    private void ApplyTrackedData()
    {
        foreach (MenuCase m in caseItems) {
            /*if (GlobalData.completedCases.Contains(m.recordNumber)) {
				m.completed = true;
			}*/
            if (Tracker.HasCaseData(m.recordNumber)) {
                m.completed = Tracker.GetCaseData(m.recordNumber).caseFinished;
                //Don't want to update the rating if they haven't rated it yet
                if (Tracker.GetCaseData(m.recordNumber).caseRating > 0) {
                    //We aren't updating the rating because we wan't to know the average too
                    //m.rating = Tracker.GetCaseData(m.recordNumber).caseRating;
                }
            }
        }
    }

    private bool CheckIfOutdatedLocalCase(MenuCase m)
    {
        if (System.IO.File.Exists(GetLocalSavesFolderPath() + m.filename)) {
            DateTime localFileModified = System.IO.File.GetLastWriteTime(GetLocalSavesFolderPath() + m.filename);
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //Convert from seconds from unix epoch to local DateTime
            DateTime serverFileModified = unixEpoch.AddSeconds(m.dateModified); //Add the seconds as specified by the chosen MenuCase
                                                                                /*
                                                                                Debug.Log("------" + m.filename + "------");
                                                                                Debug.Log(serverFileModified.ToLocalTime());
                                                                                Debug.Log(localFileModified.ToLocalTime());
                                                                                print(m.dateModified + ", " + (long)localFileModified.ToUniversalTime().Subtract(unixEpoch).TotalSeconds);
                                                                                */

            if (DateTime.Compare(serverFileModified.ToLocalTime(), localFileModified.ToLocalTime()) > 0) {
                Debug.Log("Server file " + m.filename + " is newer than local file.");
                return true;
            }
        }
        return false;
    }

    private Tuple<Button, Button> InitCaseButtons(GameObject caseObj, MenuCase m)
    {
        //Setup the edit button on the case object to load the case
        //Button editButton = caseObj.transform.Find("ImageRows/Rows/Row0/EditButton").GetComponentInChildren<Button>();
        Button editButton;
        Button readButton;
        if (GlobalData.GDS.isMobile) {
            if (isList) {
                editButton = caseObj.transform.Find("Display/OpenWriter")?.GetComponentInChildren<Button>();
                readButton = caseObj.transform.Find("OpenReader")?.GetComponent<Button>();
            } else {
                editButton = caseObj.transform.Find("Row4/OpenWriter")?.GetComponentInChildren<Button>();
                readButton = caseObj.transform.Find("Row4/OpenReader")?.GetComponent<Button>();
            }
        } else if (isList) {
            editButton = caseObj.transform.Find("Display/OpenWriter").GetComponentInChildren<Button>();
            readButton = caseObj.transform.Find("Display/OpenReader").GetComponent<Button>();
        } else {
            editButton = caseObj.transform.Find("Row4/OpenWriter").GetComponentInChildren<Button>();
            readButton = caseObj.transform.Find("Row4/OpenReader").GetComponent<Button>();
        }

        if (!showingCases) {
            readButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "New Case";
        }

        if (!isWriter) {
            editButton?.gameObject.SetActive(false);
        } else {
            if (showingCases) {
                readButton?.gameObject.SetActive(false);
            } else if (GlobalData.role != GlobalData.Roles.Admin) {
                editButton?.gameObject.SetActive(false);
            }
        }

        if (m.accountId == GlobalData.accountId) {
            //editButton.gameObject.SetActive(true);
            editButton?.onClick.AddListener(delegate {
                if (m.course != null) {
                    GlobalData.recommendedCases = new MenuCase[1];
                    if (m.GetIndexInCourse() < m.course.GetCases().Count - 1) {
                        GlobalData.recommendedCases[0] = GetCaseByRecordNumber(m.course.GetCases()[m.GetIndexInCourse() + 1]);
                    }
                }
                LoadWriter(editButton);
                /*
				GlobalData.resourcePath = "Writer";
				GlobalData.firstName = "";
				GlobalData.lastName = "";
				GlobalData.createCopy = false;
				ButtonLoadScene(editButton); //, caseObj);
				*/
            });
        } else {
            editButton?.gameObject.SetActive(false);
        }

        readButton?.onClick.AddListener(delegate {
            print(m.course);
            if (m.course != null) {
                GlobalData.courseCases = new LinkedList<MenuCase>();
                foreach (string s in m.course.GetCases()) {
                    GlobalData.courseCases.AddLast(GetCaseByRecordNumber(s));
                }
                GlobalData.recommendedCases = new MenuCase[1];
                if (m.GetIndexInCourse() < m.course.GetCases().Count - 1) {
                    GlobalData.recommendedCases[0] = GetCaseByRecordNumber(m.course.GetCases()[m.GetIndexInCourse() + 1]);
                }
            }
            LoadReader(readButton);
            /*
			if (!showingCases) {
				GlobalData.createCopy = true;
				GlobalData.resourcePath = "Writer";
				transform.Find("PatientNamePanel").gameObject.SetActive(true);
				transform.Find("PatientNamePanel/BackgroundPanel/Content/DescriptionText").gameObject.SetActive(false);
				SetSelectedButton(readButton);
			} else {
				GlobalData.createCopy = false;
				GlobalData.resourcePath = "Reader";
				ButtonLoadScene(readButton); //, caseObj);
			}*/
        });

        //Edit the toggle so that it accurately updates the patient image to show what case is selected
        Toggle tog = caseObj.GetComponentInChildren<Toggle>();
        Button loadInfoButton = caseObj.GetComponent<Button>();
        if (tog != null) {
            tog.group = tog.GetComponentInParent<ToggleGroup>();
            tog.onValueChanged.AddListener(delegate {
                Deselect(tog.GetComponent<Image>(), loadInfoButton);
            });
        }

        if (!GlobalData.GDS.isMobile && GlobalData.caseObj == m) {
            loadInfoButton.interactable = false;
        }

        //Setup the case object button to load the info panel
        loadInfoButton.onClick.AddListener(delegate {
            List<MenuCase> cases;
            if (showingCases) {
                cases = caseItems;
            } else {
                cases = templateItems;
            }

            bool sameSelected = (selected != null && selected.Equals(cases.Find((MenuCase obj) => obj.filename.Equals(loadInfoButton.transform.Find("Filename").GetComponent<TextMeshProUGUI>().text))));

            selected = cases.Find((MenuCase obj) => obj.filename.Equals(loadInfoButton.transform.Find("Filename").GetComponent<TextMeshProUGUI>().text));
            infoPanel.gameObject.SetActive(true);
            GlobalData.caseObj = selected;
            LoadInfoPanel(selected);

            if (!isList && tog != null && tog.GetComponent<Image>()) {
                //caseObj.transform.Find("ImageRows/PatientImageCircle").GetComponent<Image>().color = tog.transform.parent.parent.GetChild(1).GetComponent<Image>().color;
                tog.GetComponent<Image>().color = tog.transform.parent.parent.GetChild(1).GetComponent<Image>().color;
                tog.isOn = true;
                tog.group.NotifyToggleOn(tog);
            }

            if (!sameSelected) {
                firstClick = false;
                doubleClickable = false;
            }

            if (firstClick) {
                if (isWriter && showingCases) {
                    //editButton.onClick.Invoke();
                    LoadWriter(editButton);
                } else {
                    //readButton.onClick.Invoke();
                    LoadReader(readButton);
                }
                doubleClickable = false;
            } else if (doubleClickable) {
                StopCoroutine("DoubleClickable");
                firstClick = true;
                StartCoroutine(DoubleClickable());
            } else {
                doubleClickable = true;
            }

            //Since both the server and local were removed, we remove the case example


            for (int i = 0; i < caseExampleParent.childCount; i++) {
                if (caseExampleParent.GetChild(i).name.StartsWith("CaseExample")) {
                    caseExampleParent.GetChild(i).GetComponent<Button>().interactable = true;
                }
            }
            if (!GlobalData.GDS.isMobile) {
                loadInfoButton.interactable = false;
            }

        });

        return new Tuple<Button, Button>(editButton, readButton);
    }

    public void LoadReader(Button b)
    {
        if (!showingCases) {
            GlobalData.createCopy = true;
            GlobalData.resourcePath = "Writer";
            transform.Find("PatientNamePanel").gameObject.SetActive(true);
            transform.Find("PatientNamePanel/BackgroundPanel/Content/DescriptionText").gameObject.SetActive(false);
            SetSelectedButton(b);
        } else {
            GlobalData.createCopy = false;
            GlobalData.resourcePath = "Reader";
            ButtonLoadScene(b); //, caseObj);
        }
    }

    public void LoadWriter(Button b)
    {
        GlobalData.resourcePath = "Writer";
        GlobalData.firstName = "";
        GlobalData.lastName = "";
        GlobalData.createCopy = false;
        ButtonLoadScene(b); //, caseObj);
    }

    public void DisposeCaseDownload()
    {
        if (isDownloadingAllCases) {
            disposeDownload = true;
            isDownloadingAllCases = false;
        }
    }

    public bool GetListsAreLoaded()
    {
        return listsAreLoaded;
    }

    private void SetSelectedButton(Button b)
    {
        selectedButton = b;
    }

    public void CreateNewCase()
    {
        print(GlobalData.firstName + ", " + GlobalData.lastName);
        if (transform.Find("PatientNamePanel/BackgroundPanel/Content/DescriptionText").gameObject.activeInHierarchy) {
            CreateNewCase(true);
        } else {
            CreateNewCase(false);
        }
    }

    public void CreateNewCase(bool isCaseCopy)
    {
        if (showingCases && !isCaseCopy) {
            transform.Find("ContentPanel").GetComponent<FilePickerScript>().NewFileAvoidFilePicker();
        } else {
            ButtonLoadScene(selectedButton); //, null);
        }
    }

    //Used when creating case copy
    public void UpdateCaseDescription(TMP_InputField t)
    {
        GlobalData.description = t.text;
    }

    private void ButtonLoadScene(Button button) //, GameObject caseGameObject)
    {
        string filename = "";
        Transform temp = button.transform;
        while (temp != null && !temp.name.Equals("CaseExample") && !temp.name.Equals("InformationPanel")) {
            temp = temp.parent;
        }
        filename = temp.Find("Filename").GetComponent<TextMeshProUGUI>().text;
        List<MenuCase> cases;
        if (showingCases) {
            cases = caseItems;
        } else {
            cases = templateItems;
        }
        GlobalData.caseObj = cases.Find((MenuCase obj) => obj.filename.Equals(filename));//editButton.transform.parent.Find("Filename").GetComponent<Text>().text));
        GlobalData.fileName = GlobalData.caseObj.filename;
        if (!isWriter) {
            Tracker.RecordData(Tracker.DataType.startedCase, GlobalData.caseObj.recordNumber, true);
        }

        /*if (GlobalData.recommendedCases == null) {
			FindRecommendedCases();
		}*/
        GlobalData.allDownloadedCases = caseItems;

        /*if (GlobalData.fileName != null && !GlobalData.fileName.Equals("")) {
			GlobalData.caseObj.patientName = GlobalData.firstName + "_" + GlobalData.lastName;
		}*/ //Think this is handled in filepickerscript when creating dupilcate cases
        if (GlobalData.demo) {
            GlobalData.filePath = Application.streamingAssetsPath + GlobalData.menuFilePath;
        } else {
            GlobalData.filePath = GetLocalSavesFolderPath();
            if (!System.IO.Directory.Exists(GlobalData.filePath)) {
                System.IO.Directory.CreateDirectory(GlobalData.filePath);
            }
        }
        print(File.Exists(GetLocalSavesFolderPath() + GlobalData.fileName));
        print("Filename: " + GlobalData.fileName + ", account id: " + GlobalData.caseObj.accountId);
        print("Patient name: " + GlobalData.firstName + ", " + GlobalData.lastName);

        //If the user is creating a copy, then the Description variable of GlobalData will be set
        if (!GlobalData.description.Equals("")) {
            GlobalData.caseObj.description = GlobalData.description;
            GlobalData.description = "";
        }

        string levelToLoad = "";
        switch (GlobalData.resourcePath) {
            case "Writer":
                levelToLoad = "Writer";
                if (GlobalData.createCopy) {
                    GlobalData.caseObj.caseType = MenuCase.CaseType.privateCase;
                }
                break;
            case "Reader":
                levelToLoad = readerLevelName;
                break;
        }

        string cedFilePath = GetLocalSavesFolderPath() + GlobalData.fileName;

        //If the server file is more recent, or we force the choice through dev options, offer to load it
        //if (caseGameObject != null && caseGameObject.transform.Find("ServerNotice").gameObject.activeInHierarchy ||
        if (CheckIfOutdatedLocalCase(GlobalData.caseObj) ||
           (GlobalData.GDS.developer && GlobalData.GDS.chooseFileLoadLocation && File.Exists(cedFilePath))) {
            //Open up confirmation for what to load.
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //Convert from seconds from unix epoch to local DateTime
            GameObject confirm = GameObject.Find("Popups").transform.Find("LocalOrServerConfirmation").gameObject;

            //Set the message for the popup
            string message = "";
            if (GlobalData.GDS.chooseFileLoadLocation) {
                message += "Choose which source you would like to load from:\n\n";
            } else {
                message += "Server version is newer!\nLoad locally or from the server?\n";
            }
            message += "Server save time: " + unixEpoch.AddSeconds(GlobalData.caseObj.dateModified).ToLocalTime() + "\n" +
                        "Local save time: " + File.GetLastWriteTime(GetLocalSavesFolderPath() + GlobalData.caseObj.filename).ToLocalTime();
            confirm.transform.Find("ConfirmActionPanel/Content/Row0/ActionValue").GetComponent<TextMeshProUGUI>().text = message;

            //Edit the buttons so they do what we want
            Button serverButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/ServerButton").GetComponent<Button>();
            Button localButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/LocalButton").GetComponent<Button>();
            Button bothButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/BothButton").GetComponent<Button>();

            serverButton.onClick.RemoveAllListeners();
            localButton.onClick.RemoveAllListeners();
            bothButton.onClick.RemoveAllListeners();

            bothButton.gameObject.SetActive(false);

            localButton.GetComponentInChildren<TextMeshProUGUI>().text = "Local";
            serverButton.GetComponentInChildren<TextMeshProUGUI>().text = "Server";

            serverButton.onClick.AddListener(delegate {
                confirm.SetActive(false);
                GlobalData.loadLocal = false;
                ShowLoadingScreen();
                //SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Single);
                //GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().levelName = levelToLoad;

                //GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().ActivateScene();

                StartCoroutine(GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().OpenScene(levelToLoad));
            });

            localButton.onClick.AddListener(delegate {
                confirm.SetActive(false);
                GlobalData.loadLocal = true;
                ShowLoadingScreen();
                //SceneManager.LoadScene(levelToLoad);
                //GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().levelName = levelToLoad;

                //GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().ActivateScene();

                StartCoroutine(GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().OpenScene(levelToLoad));
            });

            confirm.SetActive(true);
        }

        //If autosave file exists and it's more recent than the .ced file, offer to load it
        else if (File.Exists(cedFilePath.Remove(cedFilePath.Length - "ced".Length) + "auto") &&
            (File.GetLastWriteTime(cedFilePath).CompareTo(File.GetLastWriteTime(cedFilePath.Remove(cedFilePath.Length - "ced".Length) + "auto")) < 0)) {

            GameObject confirm = GameObject.Find("GaudyBG").transform.Find("Popups/LocalOrServerConfirmation")?.gameObject;
            // Confirm is in a different place for mobile
            if (!confirm)
                confirm = GameObject.Find("TopCanvas").transform.Find("Popups/LocalOrServerConfirmation")?.gameObject;

            //Set the message for the popup
            string message = "";
            message += "There is a newer autosave available. Would you like to open it?";
            confirm.transform.Find("ConfirmActionPanel/Content/Row0/ActionValue").GetComponent<TextMeshProUGUI>().text = message;

            //Edit the buttons so they do what we want
            Button noButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/ServerButton").GetComponent<Button>();
            Button yesButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/LocalButton").GetComponent<Button>();
            Button bothButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/BothButton").GetComponent<Button>();

            noButton.onClick.RemoveAllListeners();
            yesButton.onClick.RemoveAllListeners();
            bothButton.onClick.RemoveAllListeners();

            noButton.gameObject.SetActive(true);
            yesButton.gameObject.SetActive(true);
            bothButton.gameObject.SetActive(false);

            noButton.GetComponentInChildren<TextMeshProUGUI>().text = "No";
            yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "Yes";

            noButton.onClick.AddListener(delegate {
                confirm.SetActive(false);
                GlobalData.loadAutosave = false;
                ShowLoadingScreen();
                StartCoroutine(GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().OpenScene(levelToLoad));
            });

            yesButton.onClick.AddListener(delegate {
                confirm.SetActive(false);
                GlobalData.loadAutosave = true;
                ShowLoadingScreen();
                StartCoroutine(GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().OpenScene(levelToLoad));
            });

            confirm.SetActive(true);
        } else {
            ShowLoadingScreen();
            //SceneManager.LoadScene(levelToLoad);
            //GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().levelName = levelToLoad;

            //GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().ActivateScene();

            StartCoroutine(GameObject.Find("GaudyBG").transform.Find("ContentPanel").GetComponent<FilePickerScript>().OpenScene(levelToLoad));
        }
    }

    private IEnumerator DoubleClickable()
    {
        firstClick = true;
        yield return new WaitForSecondsRealtime(doubleClickTime);
        firstClick = false;
    }

    private void CheckResumeCase()
    {
        if (GlobalData.GDS.isMobile && !Tracker.GetAskedToResume()) {
            if (Tracker.GetInProgressKey().Length > 0) {
                print(Tracker.GetInProgressKey());
                if (!Tracker.InProgress(Tracker.GetInProgressKey())) {
                    return;
                }
                transform.Find("Popups/ResumeCase").gameObject.SetActive(true);
                GameObject caseObj = transform.Find("Popups/ResumeCase/WhitePanel/New/NextCase/CaseExample").gameObject;
                GetCaseByRecordNumber(Tracker.GetInProgressKey())
                    .PopulateCaseTransform(caseObj, false);
                InitCaseButtons(caseObj, GetCaseByRecordNumber(Tracker.GetInProgressKey()));

                //Only want to ask this at program start up/login
                //Current-In-progress is only used for this, so we can clear it
                Tracker.ClearInProgressKey();
                Tracker.Resumed();
            }
        }
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    #region-------------------------------------------Courses------------------------------------------
    private List<Course> courses;

    public void ToggleCourseView()
    {
        ToggleCourseView(!showingCourses);
    }

    public void ToggleCourseView(bool b)
    {
        showingCourses = b;
        // "No cases" notification shouldn't be showing when viewing courses
        if (showingCases)
            caseExampleParent.parent.parent.parent.Find("NoCasesNotification").gameObject.SetActive(false);

        transform.Find("SidePanel/TopPanel/SettingsPanel/SlideOpenButton").GetComponent<Button>().interactable = !b;
        transform.Find("ContentPanel/SectionButtonsPanel/ToggleView").gameObject.SetActive(!b);
        SpawnAppropriateContent();
    }

    public void SpawnCourses()
    {
        GameObject courseExample;
        bool tempList = isList;
        isList = true; //Set this to keep track of what view was being shown before. Also, populatecasetransform needs list to be true here since we use a list to show cases in courses
        foreach (Course c in courses) {
            courseExample = Instantiate(Resources.Load("Menu/Prefabs/Courses/CoursePanel") as GameObject, caseExampleParent);
            courseExample.name = "CourseExample";

            courseExample.transform.Find("Display/Title and Desc/Title").GetComponent<TextMeshProUGUI>().text = c.GetTitle();
            courseExample.transform.Find("Display/Title and Desc/Description").GetComponent<TextMeshProUGUI>().text = c.GetDescription();
            courseExample.transform.Find("Display/AuthorData").GetComponent<TextMeshProUGUI>().text = c.GetAuthor();
            Transform courseCasesParent = courseExample.transform.Find("Display/CaseViewport/CaseParent");
            foreach (string recordNumber in c.GetCases()) {
                GameObject caseFromCourse = Instantiate(Resources.Load("Menu/Prefabs/Courses/CourseTMPCaseListExample") as GameObject, courseCasesParent);
                caseFromCourse.name = "CourseCase" + caseFromCourse.transform.GetSiblingIndex();

                GetCaseByRecordNumber(recordNumber)?.PopulateCaseTransform(caseFromCourse, isList);
            }

            Button openCourseOverview = courseExample.GetComponent<Button>();
            openCourseOverview.onClick.AddListener(delegate {
                //Course c2 = courses.Find((Course c) => c.)

                Transform courseInfoPanel = transform.Find("Popups/CourseOverviewPanel");
                courseInfoPanel.gameObject.SetActive(true);

                courseInfoPanel.Find("Display/Title").GetComponent<TextMeshProUGUI>().text = c.GetTitle();
                courseInfoPanel.Find("Display/Description").GetComponent<TextMeshProUGUI>().text = c.GetDescription();
                courseInfoPanel.Find("Display/AuthorData").GetComponent<TextMeshProUGUI>().text = c.GetAuthor();

                //This copies the existing list, but the existing list isn't exactly what we want.
                //Instantiate(courseCasesParent, courseInfoPanel.Find("CourseViewport"));
                Transform parentThingy = courseInfoPanel.Find("Display/CasesViewport/CasesContent");
                for (int i = 1; i < parentThingy.childCount; i++) {
                    Destroy(parentThingy.GetChild(i).gameObject);
                }
                GameObject oneToDuplicate = null;
                GameObject listItem = null;
                MenuCase m = null;
                bool b = isList;
                isList = true;
                foreach (string s in c.GetCases()) {
                    if (oneToDuplicate == null) {
                        oneToDuplicate = Instantiate(Resources.Load("Menu/Prefabs/Panels/MobileTMPCaseListExample") as GameObject, parentThingy);
                    }
                    listItem = Instantiate(oneToDuplicate, parentThingy);
                    //listItem.transform.Find("CaseCompleted").gameObject.SetActive(false);
                    listItem.name = "CaseExample";
                    m = GetCaseByRecordNumber(s);
                    m.course = c;
                    m.PopulateCaseTransform(listItem, isList);
                    InitCaseButtons(listItem, m);
                }
                Destroy(oneToDuplicate);
                isList = b;
                listItem = null;

                //Adjust for the writer/other cases as well in a future update
                //If all cases completed, startingCase will remain at 0
                int startingCase = 0;
                for (int i = startingCase; i < c.GetCases().Count; i++) {
                    MenuCase mm = GetCaseByRecordNumber(c.GetCases()[i]);
                    if (!mm.completed) {
                        startingCase = i;
                        break;
                    }
                }
                courseInfoPanel.Find("Display/ReaderWriterButtons/OpenReader").GetComponent<Button>().onClick.AddListener(delegate {
                    print(startingCase);
                    print(c.GetCases().Count + ": " + string.Join(",", c.GetCases()));
                    GlobalData.courseCases = new LinkedList<MenuCase>();
                    foreach (string s in c.GetCases()) {
                        GlobalData.courseCases.AddLast(GetCaseByRecordNumber(s));
                    }
                    GlobalData.recommendedCases = new MenuCase[1];
                    if (startingCase < (c.GetCases().Count - 1)) {
                        GlobalData.recommendedCases[0] = GetCaseByRecordNumber(c.GetCases()[startingCase + 1]);
                    } else {
                        print("You opened the last case. No next case for you");
                        //Set recommendedCases to null to generate regular recommendations later
                        GlobalData.recommendedCases = null;
                    }
                    if (GlobalData.recommendedCases != null && GlobalData.recommendedCases[0] != null)
                        print(GlobalData.recommendedCases[0].recordNumber);
                    else
                        print("recommended case is null");
                    //We add one to starting case because the descriptions offset the count
                    LoadReader(parentThingy.GetChild(startingCase + 1).Find("OpenReader")?.GetComponent<Button>());
                });
            });


            //Move enrolled cases to the top
            /*if (user is enrolled) {
				caseExample.transform.SetSiblingIndex(0);
			}*/
        }
        isList = tempList;
    }

    /// <summary>
    /// Dev method to return a preconstructed course
    /// </summary>
    /// <returns></returns>
    public List<Course> GetCourses()
    {
        return courses;
    }

    /// <summary>
    /// Generates a test list for courses as they could be downloaded from the server
    /// </summary>
    public void InitCourses()
    {
        courses = new List<Course>();

        string courseTitle = "Random Course List";
        string courseDescription = "Here's my description";
        string courseAuthor = "Ian Mckinnon";

        List<string> caseList = new List<string>();
        caseList.Add("094118");
        caseList.Add("238113");
        caseList.Add("602901");
        caseList.Add("444445"); //Giberrish record number, as if it was deleted
        Course c = new Course(courseTitle, courseDescription, courseAuthor, caseList);
        //ValidateCourse(c);
        courses.Add(c);

        courseTitle = "Second List";
        courseDescription = "It has cases";
        courseAuthor = "Ian Mckinnon";

        caseList = new List<string>();
        caseList.Add("397651");
        caseList.Add("544971");
        caseList.Add("835801");
        c = new Course(courseTitle, courseDescription, courseAuthor, caseList);
        //ValidateCourse(c);
        courses.Add(c);

        courseTitle = "Buprenorphine Module";
        courseDescription = "A list of all cases covering Bup. practice";
        courseAuthor = "Dr. Karen Rosie";

        caseList = new List<string>();
        caseList.Add("835801");
        caseList.Add("789650");
        c = new Course(courseTitle, courseDescription, courseAuthor, caseList);
        //ValidateCourse(c);
        courses.Add(c);

        courseTitle = "Alcohol Module";
        courseDescription = "A list of all cases covering Alcohol misuse";
        courseAuthor = "Dr. Karen Rosie";

        caseList = new List<string>();
        caseList.Add("397651");
        caseList.Add("069184");
        c = new Course(courseTitle, courseDescription, courseAuthor, caseList);
        //ValidateCourse(c);
        courses.Add(c);
    }

    /// <summary>
    /// Validates all courses
    /// </summary>
    public void ValidateCourses()
    {
        foreach (Course c in courses) {
            ValidateCourse(c);
        }
    }

    /// <summary>
    /// Validates a course's contents by removing any irrelevant courses in its list
    /// </summary>
    /// <param name="c"></param>
    public void ValidateCourse(Course c)
    {
        for (int i = 0; i < c.GetCases().Count; i++) {
            if (caseItems.Find((MenuCase m) => m.recordNumber.Equals(c.GetCases()[i])) == null) {
                //If we wanted to, we could put in something designating a private or deleted case, like youtube
                c.RemoveCourse(c.GetCases()[i]);
            }
        }
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    #region -------------------------------Handles Local Encryption Folder-------------------------------

    public void OpenLocalFileFolder()
    {
        if (!lm.loggedIn) {
            lm.ShowMessage("Please log in to view your local files");
            print("Please log in to view your local files");
            return;
        }
        string formattedFilePath = Application.persistentDataPath;
        formattedFilePath = formattedFilePath.Replace("/", "\\") + "\\LocalSaves\\" + GetLocalFolder(GlobalData.accountId + "") + "\\";
        if (!Directory.Exists(formattedFilePath)) {
            Debug.Log("Could not find your local saves. They should be in a folder titled: " + GetLocalFolder(GlobalData.accountId + ""));
            Debug.Log("Opening local saves folder...");
            formattedFilePath = formattedFilePath.Replace("/", "\\") + "\\LocalSaves\\";
            if (!Directory.Exists(formattedFilePath)) {
                Directory.CreateDirectory(formattedFilePath);
            }
        }
        print(formattedFilePath);

        System.Diagnostics.Process.Start("explorer.exe", formattedFilePath);
    }

    /**
	 * Returns the full path to the local saves folder. Uses the currently logged in user's id
	 */
    public string GetLocalSavesFolderPath()
    {
        return Application.persistentDataPath + "\\LocalSaves\\" + GetLocalFolder(GlobalData.accountId + "") + "\\";
    }

    /**
	 * Returns the full path to the local saves folder. Uses the currently logged in user's id
	 */
    public string GetLocalSavesFolderPath(int accountId)
    {
        return Application.persistentDataPath + "\\LocalSaves\\" + GetLocalFolder(accountId + "") + "\\";
    }

    ///<summary>
    ///Returns a truncated md5 hash to represent unique folders for users. This returns only the folder
    ///</summary>
    ///<param name="accountId">User account id</param>
    public string GetLocalFolder(string accountId)
    {
        using (MD5 md5 = MD5.Create()) {
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(accountId));
            StringBuilder sb = new StringBuilder();
            foreach (Byte b in bytes) {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString().Substring(7, 10); //Return a random 10 digit substring of the hash to represent the folder name
        }
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    #region --------------------------Separate Downloading and Removing Cases----------------------------
    public bool IsDownloading()
    {
        return downloading;
    }

    public string DownloadingCase()
    {
        if (!downloadingCase.EndsWith(".ced")) {
            return downloadingCase + ".ced";
        }
        return downloadingCase;
    }

    public void ReDownloadMenuItems()
    {
        StartCoroutine(DownloadMenuItems());
    }

    public MenuCase GetCaseByFilename(string filename)
    {
        MenuCase selectedCase = caseItems.Find(c => c.filename.Equals(filename));
        if (selectedCase == null) {
            selectedCase = templateItems.Find(c => c.filename.Equals(filename));
        }
        return selectedCase;
    }

    public MenuCase GetCaseByRecordNumber(string recordNumber)
    {
        MenuCase selectedCase = caseItems.Find(c => c.recordNumber.Equals(recordNumber));
        if (selectedCase == null) {
            selectedCase = templateItems.Find(c => c.recordNumber.Equals(recordNumber));
        }
        return selectedCase;
    }

    public IEnumerator DownloadCase()
    {
        WWWForm form;
        string filename;
        do {
            filename = lm.GetCaseToDownload();
            if (filename == null) {
                downloading = false;
                downloadingCase = "";
                yield break;
            }

            var caseObj = GetCaseByFilename(filename);
            int caseAccId = caseObj.accountId;

            //filename = filename.Replace(" ", "_");
            if (filename.EndsWith(".ced")) {
                filename = filename.Substring(0, filename.Length - ".ced".Length);
            }
            print(filename);

            //Save the .ced file
            form = new WWWForm();
            form.AddField("mode", "downloadCase");
            form.AddField("filename", filename.Replace(" ", "_") + ".ced");
            form.AddField("account_id", caseAccId);
            form.AddField("column", "xmlData");
            lm.ShowMessage("Downloading case...");
            downloading = true;
            downloadingCase = filename;

			string outputText;
			using (UnityWebRequest webRequest = UnityWebRequest.Post(GlobalData.serverAddress + "Menu.php", form)) {
				yield return webRequest.SendWebRequest();
				outputText = webRequest.downloadHandler.text;
				print(outputText); //For debugging
			}

            if (outputText.Length > 0) {
                SaveFile(filename, outputText, ".ced");
            } else {
                print("Unable to download");
                lm.ShowMessage("Unable to download", true);
                downloading = false;
                downloadingCase = "";
                yield break;
            }

            //Save the xml
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(outputText);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(GetLocalSavesFolderPath() + filename.Replace("_", " ") + ".xml", false);
            xmlDoc.Save(sw);
            sw.Close();
            sw.Dispose();

            //Save the menu file
            sw = new System.IO.StreamWriter(GetLocalSavesFolderPath() + filename.Replace("_", " ") + " menu.txt", false);
            sw.WriteLine(GetCaseByFilename(filename + ".ced").ToString());
            sw.Close();
            sw.Dispose();

            //Save the .cei file
            form = new WWWForm();
            form.AddField("column", "imgData");
            form.AddField("mode", "downloadCase");
            form.AddField("filename", filename.Replace(" ", "_") + ".ced");
            form.AddField("account_id", caseAccId);
            form.AddField("column", "imgData");

			using (UnityWebRequest webRequest = UnityWebRequest.Post(GlobalData.serverAddress + "Menu.php", form)) {
				yield return webRequest.SendWebRequest();
				outputText = webRequest.downloadHandler.text;
				print(outputText); //For debugging
			}

            if (outputText.Length > 0) {
                SaveFile(filename, outputText, ".cei");
            } else {
                print("Unable to download images");
                lm.ShowMessage("Unable to download", true);
                downloading = false;
                downloadingCase = "";
                yield break;
            }

            print("Successfully downloaded " + filename);
            lm.ShowMessage("Successfully downloaded");
            caseObj.downloaded = true;
            if (caseObj.filename == GlobalData.caseObj.filename)
                LoadInfoPanel(caseObj);

            GameObject serverNotice;
            for (int i = 0; i < caseExampleParent.childCount; i++) {
                if (!caseExampleParent.GetChild(i).name.Equals("CaseExample")) {
                    continue;
                }
                if (caseExampleParent.GetChild(i).Find("Filename").GetComponent<TextMeshProUGUI>().text.Equals(filename + ".ced") &&
                    (serverNotice = caseExampleParent.GetChild(i).Find("ServerNotice").gameObject).activeInHierarchy) {
                    serverNotice.SetActive(false);
                    break;
                }
            }
        } while (filename != null);
        downloading = false;
        downloadingCase = "";
        print("Finished downloads. Your file(s) are available at " + Application.persistentDataPath + "/");
    }


    private void SaveFile(string filename, string fileText, string extension)
    {
        print("Saving : " + GetLocalSavesFolderPath() + filename + extension);
        if (!Directory.Exists(GetLocalSavesFolderPath())) {
            Directory.CreateDirectory(GetLocalSavesFolderPath());
        }
        StreamWriter sw = new StreamWriter(GetLocalSavesFolderPath() + filename.Replace("_", " ") + extension, false);
        sw.WriteLine(fileText);
        sw.Close();
        sw.Dispose();
    }

    /**
	 * Removes a case from the database
	 */
    public IEnumerator RemoveCase(string filename, bool removeServer, bool removeLocal)
    {
        string formattedFilename = filename.Replace("_", " ");
        filename = filename.Replace(" ", "_");
        print(formattedFilename);
        if (removeServer) {
            WWWForm form = new WWWForm();
            form.AddField("mode", "deleteCase");

            form.AddField("account_id", GlobalData.accountId);
            form.AddField("filename", filename);
            lm.ShowMessage("Deleting case " + filename + "...");

            string serverAddress = GlobalData.serverAddress + "Menu.php";

			string returnText;
			using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
				yield return webRequest.SendWebRequest();
				returnText = webRequest.downloadHandler.text;
				print(returnText); //For debugging
			}

            if (returnText.StartsWith("Could not remove menu item:")) { //The menu case is removed first
                print("Could not remove menu entry");
                lm.ShowMessage("Could not remove menu entry", true);
                yield break;
            }
            if (returnText.StartsWith("Could not remove case:")) {
                print("Could not remove case but the menu entry was removed.");
                lm.ShowMessage("Could not remove case but the menu \nentry was removed", true);
                yield break;
            }

            if (returnText.Equals("Successfully removed")) {
                bool destroyed = false;
                //Only remove the case example if there is neither a local or a server file. 
                if (removeLocal) {
                    GlobalData.caseObj.downloaded = false;
                    GlobalData.caseObj.localOnly = false;

                    if (File.Exists(GetLocalSavesFolderPath() + formattedFilename)) {
                        File.Delete(GetLocalSavesFolderPath() + formattedFilename);
                        File.Delete(GetLocalSavesFolderPath() + formattedFilename.Remove(formattedFilename.Length - 3) + "cei");
                        File.Delete(GetLocalSavesFolderPath() + formattedFilename.Remove(formattedFilename.Length - 3) + "xml");
                        File.Delete(GetLocalSavesFolderPath() + formattedFilename.Remove(formattedFilename.Length - 4) + " menu.txt");
                    } else {
                        print("No case to remove locally");
                        lm.ShowMessage("No case to remove locally");
                    }

                    //Since both the server and local were removed, we remove the case example
                    for (int i = 0; i < caseExampleParent.childCount; i++) {
                        if (caseExampleParent.GetChild(i).name.StartsWith("CaseExample") &&
                            caseExampleParent.GetChild(i).Find("Filename").GetComponent<TextMeshProUGUI>().text.Equals(formattedFilename)) {

                            Destroy(caseExampleParent.GetChild(i).gameObject);
                            destroyed = true;

                            break;
                        }
                    }
                } else if (!GlobalData.caseObj.downloaded) {
                    for (int i = 0; i < caseExampleParent.childCount; i++) {
                        if (caseExampleParent.GetChild(i).name.StartsWith("CaseExample") &&
                                caseExampleParent.GetChild(i).Find("Filename").GetComponent<TextMeshProUGUI>().text.Equals(formattedFilename)) {

                            Destroy(caseExampleParent.GetChild(i).gameObject);
                            destroyed = true;
                            GlobalData.caseObj.downloaded = false;
                            GlobalData.caseObj.localOnly = false;

                            break;
                        }
                    }
                } else {
                    GlobalData.caseObj.localOnly = true;
                }

                if (destroyed) {
                    caseItems.Remove(caseItems.Find(mc => mc.filename.Equals(formattedFilename)));
                    templateItems.Remove(templateItems.Find(mc => mc.filename.Equals(formattedFilename)));
                    infoPanel.gameObject.SetActive(false);

                    print("Successfully removed " + formattedFilename);
                    lm.ShowMessage("Successfully removed " + formattedFilename);
                } else {
                    print("Server case deleted");
                    lm.ShowMessage("Server case deleted");
                    var caseObj = caseItems.Find(mc => mc.filename.Equals(formattedFilename));
                    LoadInfoPanel(caseObj);
                }
            } else {
                print("Unknown error. Could not remove");
                lm.ShowMessage("Unknown error. Could not remove", true);
            }
        } else if (removeLocal) {
            print(formattedFilename);
            var path = GetLocalSavesFolderPath() + formattedFilename.Remove(formattedFilename.Length - 4);
            if (File.Exists($"{path}.ced") || File.Exists($"{path}.auto")) {
                if (File.Exists($"{path}.ced"))
                    File.Delete($"{path}.ced");
                if (File.Exists($"{path}.auto"))
                    File.Delete($"{path}.auto");
                if (File.Exists($"{path}.xml"))
                    File.Delete($"{path}.xml");
                File.Delete($"{path}.cei");
                File.Delete($"{path} menu.txt");
                bool destroyed = false;

                if (GlobalData.caseObj.localOnly) {
                    for (int i = 0; i < caseExampleParent.childCount; i++) {
                        if (caseExampleParent.GetChild(i).name.StartsWith("CaseExample") &&
                            caseExampleParent.GetChild(i).Find("Filename").GetComponent<TextMeshProUGUI>().text.Equals(formattedFilename)) {
                            //If the case is only local, we remove the case preview

                            Destroy(caseExampleParent.GetChild(i).gameObject);
                            destroyed = true;
                            break;
                        }
                    }
                }
                if (destroyed) {
                    caseItems.Remove(caseItems.Find(mc => mc.filename.Equals(formattedFilename)));
                    templateItems.Remove(templateItems.Find(mc => mc.filename.Equals(formattedFilename)));
                    infoPanel.gameObject.SetActive(false);

                    print("Successfully removed " + formattedFilename);
                    lm.ShowMessage("Successfully removed " + formattedFilename);
                } else {
                    if (removeServer) {
                        print("Case removed successfully, yet could not remove menu case item");
                        lm.ShowMessage("Case removed successfully, yet could not remove menu case item", true);
                    } else {
                        print("Local case deleted");
                        lm.ShowMessage("Local case deleted");
                        MenuCase caseObj;
                        if (showingCases)
                            caseObj = caseItems.Find(mc => mc.filename.Equals(formattedFilename));
                        else
                            caseObj = templateItems.Find(mc => mc.filename.Equals(formattedFilename));

                        caseObj.downloaded = false;
                        LoadInfoPanel(caseObj);
                    }
                }
            } else {
                print("No case to remove locally");
                lm.ShowMessage("No case to remove locally");
            }
        }
    }

    #endregion
    //-------------------------------------------------------------------------------------------

    public void ShowLoadingScreen()
    {
        if (!GlobalData.showLoading) {
            return;
        }
        print(LoadingScreenManager.Instance.name);
        loadingScreen = LoadingScreenManager.Instance.gameObject;
        loadingScreen.SetActive(true);
        loadingScreen.GetComponent<CanvasGroup>().alpha = 1;
        loadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        DontDestroyOnLoad(loadingScreen);

        loadingScreen.transform.Find("LoadingBar").gameObject.SetActive(true);
        loadingScreen.transform.Find("LoadingBarDescription").gameObject.SetActive(true);
        loadingScreen.transform.Find("LoadingBarDescription").GetComponent<TextMeshProUGUI>().text = "Loading Scene:";
        /*if (loadingScreen.gameObject.scene.GetRootGameObjects().Length > 1) {
			Destroy(loadingScreen.gameObject.scene.GetRootGameObjects()[0]);
		}*/
    }

    //Used during testing to force the loading screen up
    public void TestShowLoading()
    {
        ShowLoadingScreen();
    }
}
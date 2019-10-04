using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalData : MonoBehaviour
{
    static GlobalData instance;

    public static string fileName = "";
    public static string filePath = "";
    public static string resourcePath = "";
    public static string menuScene = "";
    public static CanvasGroup toolTip;
    public static CanvasGroup contentTip;
    public static Dictionary<string, string> instructionsDict;
    public static string gender;
    public static string firstName;
    public static string lastName;
    public static string description = "";
    public static string patientImageID = "patientImage";

    public static string serverAddress = "https://takecontrolgame.com/docs/games/CECreator/PHP/"; //"http://docs.clinicaltools.com/games/CECreator/PHP/"; //"http://www.clinicalencounter.com/games/IanTesting/";
    public static int accountId = 0;
    public static string username = "";
    public static string password = "";
    public static bool stayLoggedIn = false;
    public static string email = "";
    public static string role = Roles.Guest;
    public static string userTitle = "";
    public static string userFirstName = "";
    public static string userLastName = "";
    //public static UserData user = new UserData();
    public static MenuCase caseObj;
    public static MenuCase[] recommendedCases;
    public static List<MenuCase> allDownloadedCases;
    public static LinkedList<MenuCase> courseCases;

    public static bool offline = false;

    public static bool newCase = false;
    public static bool loadLocal = true;
    public static bool loadAutosave = false;
    public static bool createCopy = false;
    public static string encryptionKey = "obexOpm1wWM7NGPV";
    public static string encryptionIV = "fTfB28G5j3Pmsw1p";
    public static bool showLoading = true;
    public static int autosaveRate;
    public static bool enableAutoSave = true;
    public static bool uploadCharacterImage = false;

    [HideInInspector]
    public static float resizeVal;
    public static int resizeSteps = 3;
    public static bool showMobileCursor = true;

    public static bool demo = false;
    public static string menuFilePath = "/DemoCases/Cases/";
    public static char EMPTY_WIDTH_SPACE = System.Convert.ToChar(8203);


    private static GlobalDataScript gds;
    public static GlobalDataScript GDS {
        get {
            if (gds == null)
                gds = FindObjectOfType<GlobalDataScript>();
            return gds;
        }
        set {
            gds = value;
        }
    }

    public static GlobalData Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GlobalData>();
                if (instance == null) {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<GlobalData>();
                }
            }
            return instance;
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("WriterMainMenu")) {
            fileName = "";
            filePath = "";
        } else if (SceneManager.GetActiveScene().name.Equals("Cass1WriterMainMenu")) {
            GameObject.Find("GaudyBG").transform.Find("ContentPanel/SectionButtonsPanel/DeveloperWindow/Toggles/ToggleDemo").GetComponent<Toggle>().isOn = GDS.demoMode;
        }
    }

    public void LoadLocal(bool b)
    {
        loadLocal = b;
    }

    public void ToggleCaseCopy()
    {
        createCopy = !createCopy;
    }

    public void ToggleChooseFileLocation()
    {
        GDS.chooseFileLoadLocation = !GDS.chooseFileLoadLocation;
    }

    public void ToggleShowLoading()
    {
        showLoading = !showLoading;
    }

    public void ToggleAutoSave()
    {
        enableAutoSave = !enableAutoSave;
    }

    public void ToggleUploadCharacterImage()
    {
        uploadCharacterImage = !uploadCharacterImage;
    }

    public void ToggleDeveloper(Button tog)
    {
        GDS.developer = !GDS.developer;
        if (GDS.developer) {
            tog.GetComponentInChildren<Text>().text = "Toggle Dev: Off";
        } else {
            tog.GetComponentInChildren<Text>().text = "Toggle Dev: On";
        }
    }

    public void SetCharacter(string gend)
    {
        if (GetComponent<CharacterManagerScript>()) {
            //GetComponent<CharacterManagerScript>().setCharacter(gend);
        }
        gender = gend;
    }

    public static class Roles
    {
        public const string User = "User";
        public const string Guest = "Guest";
        public const string Admin = "Admin";
    }
}
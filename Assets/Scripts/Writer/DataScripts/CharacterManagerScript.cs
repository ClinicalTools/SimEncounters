using System.Collections;
using System.Collections.Generic;
//using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using MORPH3D;
using System.Text.RegularExpressions;


public class CharacterManagerScript : MonoBehaviour {
    /*
    public Button maleButton, femaleButton;
    public Transform characterParent, headLocation;
    public InputField heightField, weightField, ageField;
    public Slider heightSlider, weightSlider, ageSlider, faceSlider, hairSlider;
    public Text BMIVal;
    public Transform skinColorToggles, hairColorToggles;
	public Camera faceCamera; // Camera used to track face for UI component
	public Transform maleChar, femaleChar;

    private bool rightRotate, leftRotate, isMoving;
    private float bmi;
    private static int maxBMI = 54;
	private M3DCharacterManager manager;
    private SkinnedMeshRenderer skinColor, hairColor;
    private Color skinColorVal, hairColorVal;
    private float speed = 180.0f;
    private Vector3 defaultPos, movePos;
    //private string filePath = "Writer/Prefabs/Character Creation";
    //private DirectoryInfo dir;
    //private FileInfo[] info;
	private string charValsPath;
	private Image currSkinImage;
	private Image currHairImage; 
	private Vector3 defaultRot; // Rotation to reset to when exiting the chracter editor
	private bool delayLoad = false;
	private PlayerInfo pInfo;


	private struct PlayerInfo
	{
		public bool isFemale; //true = female, false = male
		public float height;
		public float weight;
		public float age;
		public float face;
		public float hair;
	};

	// Use this for initialization
	void Awake () {
		if (characterParent == null) {
			return;
		}

		if (delayLoad) {
			maleChar.gameObject.SetActive(true);
			femaleChar.gameObject.SetActive(true);
			delayLoad = false;
			NextFrame.Function(Awake);
		}

        leftRotate = false;
        rightRotate = false;
        isMoving = false;
        defaultPos = characterParent.position;
		defaultRot = femaleChar.localEulerAngles;

		currSkinImage = skinColorToggles.GetChild(0).GetComponent<Image>();
		currHairImage = hairColorToggles.GetChild(0).GetComponent<Image>();

		heightSlider.onValueChanged.AddListener(delegate { setHeight(); updateBMI(); updateHeight(); });
		weightSlider.onValueChanged.AddListener(delegate { setWeight(); updateBMI(); });
		ageSlider.onValueChanged.AddListener(delegate { setAge(); updateAge(); });
		faceSlider.onValueChanged.AddListener(delegate { updateFace(); });
		hairSlider.onValueChanged.AddListener(delegate { updateHair(); });

		pInfo = new PlayerInfo();

		foreach (Transform child in skinColorToggles) {
			child.GetComponent<Toggle>().onValueChanged.AddListener(delegate { changeSkinColor(child.GetComponent<Image>().color); currSkinImage = child.GetComponent<Image>(); });
		}
		foreach (Transform child in hairColorToggles) {
			child.GetComponent<Toggle>().onValueChanged.AddListener(delegate { changeHairColor(child.GetComponent<Image>().color); currHairImage = child.GetComponent<Image>(); });
		}

		if (femaleChar.gameObject.activeInHierarchy) {
			hairColor = GameObject.Find("MicahFemaleHair").GetComponentInChildren<SkinnedMeshRenderer>();
			manager = characterParent.GetChild(0).GetComponent<M3DCharacterManager>();
			skinColor = manager.GetComponentInChildren<SkinnedMeshRenderer>();
			setCamera(femaleChar.gameObject);
			charValsPath = "3D Female Base Values";

			//If both are active (to avoid character glitches) then disable one
			if (maleChar.gameObject.activeInHierarchy) {
				//setFemale();
				maleChar.gameObject.SetActive(false);
			}
		} else if (maleChar.gameObject.activeInHierarchy) {
			hairColor = GameObject.Find ("BritHair").GetComponentInChildren<SkinnedMeshRenderer> ();
			manager = characterParent.GetChild(1).GetComponent<M3DCharacterManager>();
			skinColor = manager.GetComponentInChildren<SkinnedMeshRenderer>();
			setCamera (maleChar.gameObject);
			charValsPath = "3D Male Base Values";
			manager.ResyncBounds();
		} else { //Set female as active if none were active
			femaleChar.gameObject.SetActive(true);
			hairColor = characterParent.Find("M3DFemale/MicahFemaleHair").GetComponentInChildren<SkinnedMeshRenderer>();
			manager = characterParent.GetChild(0).GetComponent<M3DCharacterManager>();
			skinColor = manager.GetComponentInChildren<SkinnedMeshRenderer>();
			setCamera(femaleChar.gameObject);
			charValsPath = "3D Female Base Values";
		}


		//dir = new DirectoryInfo(Application.streamingAssetsPath + "/" + charValsPath);
		//info = dir.GetFiles("*.txt");

		updateBMI();
		updateHeight();
		updateAge();
		updateFace();
	}

	public void setCharacter(string gender) {
		if (gender.Equals ("Male")) {
			setMale ();
		} else if (gender.Equals ("Female")) {
			setFemale ();
		} else {
			setFemale ();
		}
	}

	public void setCharacter(Text gender) {
		if (gender.text.Equals ("Male")) {
			setMale ();
		} else if (gender.text.Equals ("Female")) {
			setFemale ();
		} else {
			setFemale ();
		}
	}

	public void setCamera(GameObject character) {
		Transform hip = character.transform.GetChild(0).Find ("hip");
		faceCamera.transform.SetParent (hip);
	}



	public void setFemale()
	{
		if (characterParent == null) {
			return;
		}
		femaleButton.Select();       
		characterParent.GetChild(0).gameObject.SetActive(true);
		manager = characterParent.GetChild(0).GetComponent<M3DCharacterManager>();
		skinColorVal = currSkinImage.color;
		hairColorVal = currHairImage.color;
		skinColor = manager.GetComponentInChildren<SkinnedMeshRenderer>();
		hairColor = GameObject.Find("MicahFemaleHair").GetComponentInChildren<SkinnedMeshRenderer>();
		characterParent.GetChild(1).gameObject.SetActive(false);
		setCamera (femaleChar.gameObject);
		charValsPath = "3D Female Base Values";
		updateCharacter();
	}

    public void setMale()
    {
		if (characterParent == null) {
			return;
		}
		maleButton.Select();       
        characterParent.GetChild(1).gameObject.SetActive(true);
        manager = characterParent.GetChild(1).GetComponent<M3DCharacterManager>();
		skinColorVal = currSkinImage.color;
		hairColorVal = currHairImage.color;
		skinColor = manager.GetComponentInChildren<SkinnedMeshRenderer>();
        hairColor = GameObject.Find("BritHair").GetComponentInChildren<SkinnedMeshRenderer>();
        characterParent.GetChild(0).gameObject.SetActive(false);
		setCamera (maleChar.gameObject);
		charValsPath = "3D Male Base Values";
        updateCharacter();      
    }

    void updateCharacter()
    {
		//dir = new DirectoryInfo(Application.streamingAssetsPath + "/" + charValsPath);
		//info = dir.GetFiles("*.txt");
        updateBMI();
        updateHeight();
        updateAge();
		updateFace();
		NextFrame.Function(updateHeight);
		skinColor.materials[1].SetColor("_Color", skinColorVal);
        skinColor.materials[2].SetColor("_Color", skinColorVal);
        hairColor.material.SetColor("_Color", hairColorVal);
    }

    void setHeight()
    {
        heightField.text = heightSlider.value.ToString();
    }

    void setWeight()
    {
        weightField.text = weightSlider.value.ToString();
    }

    void setAge()
    {
        ageField.text = ageSlider.value.ToString();
    }

    void updateHeight()
    {
        float height, blendHeightVal;
        float.TryParse(heightField.text, out height);

        blendHeightVal = ((heightSlider.value - heightSlider.minValue) / (heightSlider.maxValue - heightSlider.minValue)) * 100.0f;
        manager.SetBlendshapeValue("FBMHeight", 100.0f - blendHeightVal);
    }

    void changeSkinColor(Color c)
    {
        skinColor.materials[1].SetColor("_Color", c);
        skinColor.materials[2].SetColor("_Color", c);
        skinColorVal = skinColor.materials[1].color;
    }

    void changeHairColor(Color c)
    {
        hairColor.material.SetColor("_Color", c);
        hairColorVal = hairColor.material.color;
    }
		
    void updateFace()
    {
		//FileInfo fInfo = (FileInfo)Resources.Load ("Writer/Prefabs/Character Creation/" + charValsPath, typeof(FileInfo));

		//TextAsset txt = (TextAsset)Resources.Load("Writer/Prefabs/Character Creation" + filename - .txt, typeof(TextAsset));
		//string fileContent = txt.text;
		//string[] lines = Regex.Split(fileContent, "\n|\r|\r\n");
        int val = System.Convert.ToInt32(faceSlider.value) - 1;
		//Debug.Log (dir.FullName);
		//WWW www = new WWW(Application.streamingAssetsPath + "/" + charValsPath);
		//string fullPath = dir.FullName + "/" + info[val].Name;
		
		
		/*
		string fullPath = Application.streamingAssetsPath + "/" + charValsPath + "/" + info[val].Name;
        StreamReader reader = new StreamReader(fullPath);

        string lineText = reader.ReadLine();
        while (!reader.EndOfStream)
        {
            lineText = reader.ReadLine();
            string[] line = lineText.Split(' ');
            manager.SetBlendshapeValue(line[0], float.Parse(line[1]));
        }
        reader.Close();
		
        
    }
    

    void updateHair()
    {

    }

    void updateAge()
    {
        int age;
        float blendAgeVal;
        int.TryParse(ageField.text, out age);

		if (age >= 65) {
			blendAgeVal = ((age - 65) / (ageSlider.maxValue - 65) * 100);
			manager.SetBlendshapeValue ("Aged_Posture", blendAgeVal);
			manager.SetBlendshapeValue ("CBMAgedBody2", blendAgeVal);
			manager.SetBlendshapeValue ("CHMAgedHead3", blendAgeVal);
		} else {
            manager.SetBlendshapeValue("Aged_Posture", 0.0f);
            manager.SetBlendshapeValue("CBMAgedBody2", 0.0f);
            manager.SetBlendshapeValue("CHMAgedHead3", 0.0f);
        }
    }
    
    void updateBMI()
    {
        float height, weight;
        float.TryParse(heightField.text, out height);
        float.TryParse(weightField.text, out weight);

        if (height > 0 && weight > 0)
        {
            bmi = (weight / 2.204f) / ((height * 0.0254f) * (height * 0.0254f));
            BMIVal.text = bmi.ToString("F1");

            if (bmi >= 25.0)
            {
                float blendBMIValHeavy = ((bmi - 25.0f) / (maxBMI - 25.0f)) * 100.0f;
                manager.SetBlendshapeValue("FBMHeavy", blendBMIValHeavy);
                manager.SetBlendshapeValue("FBMPortly", blendBMIValHeavy);
                manager.SetBlendshapeValue("FBMStocky", blendBMIValHeavy);
            }

            else if (bmi <= 19.0)
            {
				// 1kg = 2.2046 Ibs
				// 1 inch = 0.0254m

                float blendBMIValUnder = 100.0f - ((bmi / 19.0f) * 100.0f);
                manager.SetBlendshapeValue("FBMEmaciated", blendBMIValUnder);
                manager.SetBlendshapeValue("PBMForearmsSize_NEGATIVE_", blendBMIValUnder);
                manager.SetBlendshapeValue("PBMWaistWidth_NEGATIVE_", blendBMIValUnder);
                manager.SetBlendshapeValue("PHMCheeksSink", blendBMIValUnder);
                manager.SetBlendshapeValue("PBMLatsSize_NEGATIVE_", blendBMIValUnder);
            }
        }

    }

    public void generateRandomCharacter()
    {
        int randGender = Random.Range(0, 2);
        int randHairColor = Random.Range(0, hairColorToggles.childCount + 1);
        int randSkinColor = Random.Range(0, skinColorToggles.childCount + 1);
        switch (randGender)
        {
            case 0:
                setMale();
                maleButton.Select();
                break;
            case 1:
                setFemale();
                femaleButton.Select();
                break;
            default:
                break;
        }

        heightSlider.value = Random.Range(heightSlider.minValue, heightSlider.maxValue);
        weightSlider.value = Random.Range(weightSlider.minValue, weightSlider.maxValue);
        ageSlider.value = Random.Range(ageSlider.minValue, ageSlider.maxValue);
        faceSlider.value = Random.Range(faceSlider.minValue, faceSlider.maxValue);
        hairSlider.value = Random.Range(hairSlider.minValue, hairSlider.maxValue);

        changeHairColor(hairColorToggles.GetChild(randHairColor).GetComponent<Image>().color);
        changeSkinColor(skinColorToggles.GetChild(randSkinColor).GetComponent<Image>().color);
    }

    public void rotateCharacterLeft()
    {
		if (Input.GetMouseButton(1) || Input.GetMouseButtonUp(1))
			return;
		leftRotate = !leftRotate;
    }

    public void rotateCharacterRight()
    {
		if (Input.GetMouseButton(1) || Input.GetMouseButtonUp(1))
			return;
		rightRotate = !rightRotate;
    }

    public void moveHead()
    {
        isMoving = true;
        movePos = headLocation.position;
    }

	public void saveCharacterVals()
	{
		pInfo.isFemale = femaleChar.gameObject.activeInHierarchy;
		pInfo.height = heightSlider.value;
		pInfo.weight = weightSlider.value;
		pInfo.age = ageSlider.value;
		pInfo.hair = hairSlider.value;
		pInfo.face = faceSlider.value;
	}

	public void resetCharacterVals()
	{
		if (pInfo.isFemale) {
			setFemale();
		} else {
			setMale();
		}
		heightSlider.value = pInfo.height;
		weightSlider.value = pInfo.weight;
		ageSlider.value = pInfo.age;
		hairSlider.value = pInfo.hair;
		faceSlider.value = pInfo.face;
	}

    public void resetCharacterPos()
    {
        isMoving = true;
        movePos = defaultPos;
    }

	public void quickReset() {
		characterParent.position = defaultPos;
		maleChar.localEulerAngles = defaultRot;
		femaleChar.localEulerAngles = defaultRot;
	}

    void Update () {
        if (leftRotate)
        {
            manager.transform.Rotate(0.0f, speed * Time.deltaTime, 0.0f);
        }

        if (rightRotate)
        {
            manager.transform.Rotate(0.0f, -speed * Time.deltaTime, 0.0f);
        }

        if (isMoving)
        {
            float step = speed * Time.deltaTime;
            characterParent.position = Vector3.MoveTowards(characterParent.position, movePos, step);
        }
    }
    */
}

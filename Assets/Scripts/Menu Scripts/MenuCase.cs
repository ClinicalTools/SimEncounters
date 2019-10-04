using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCase : IComparer<MenuCase>
{
    public int accountId;           //ID of account creator
    public string filename;         //Name of the file
    public string authorName;       //Author of the case
    public string patientName;      //Patient's name
    public string recordNumber;     //unique record number
    public string difficulty;       //difficulty of the case
    public string description;      //Even shorter description
    public string summary;          //Short summary
    public string[] tags;           //List of tags
    public long dateModified;       //The date the case was last modified (seconds from epoch)
    public string audience;         //Current audience(explain here more)
    public string version;          //Which editor version the case was created in
    public int rating;           //A rating represented as a float. May chagne to string to represent avg. and # votes.
    public CaseType caseType;       //The type of case, be it a normal case or template, public or private
    public Sprite image;
    public Course course;

    public bool completed;          //Whether or not the active user has completed the case
    public bool localOnly;
    public bool local;
    public bool server;
    public bool downloaded;

    public int GetSizeOf()
    {
        int i = 0;
        i += sizeof(int);
        i += (sizeof(char) * filename.Length);
        i += (sizeof(char) * authorName.Length);
        i += (sizeof(char) * patientName.Length);
        i += (sizeof(char) * recordNumber.Length);
        i += (sizeof(char) * difficulty.Length);
        i += (sizeof(char) * description.Length);
        i += (sizeof(char) * summary.Length);
        i += (sizeof(char) * audience.Length);
        i += (sizeof(char) * version.Length);
        i += sizeof(int);
        i += sizeof(CaseType);
        foreach (string tag in tags) {
            i += (sizeof(char) * tag.Length);
        }

        return i;
    }

    /**
	 * Empty initializer
	 */
    public MenuCase(string filename)
    {
        accountId = 0;
        this.filename = filename;
        recordNumber = "-1"; //Stating that this is an invalid case object until saved
        patientName = "";
    }

    public MenuCase(string[] parsedItems)
    {
        accountId = int.Parse(parsedItems[0]);
        filename = parsedItems[1].Replace("_", " ");
        authorName = parsedItems[2];
        patientName = parsedItems[3];
        recordNumber = parsedItems[4];
        difficulty = parsedItems[5];
        description = parsedItems[6];
        summary = parsedItems[7];
        tags = parsedItems[8].Split(new string[] { ", " }, StringSplitOptions.None);
        dateModified = long.Parse(parsedItems[9]);
        audience = parsedItems[10];
        version = parsedItems[11];
        int.TryParse(parsedItems[12], out rating);
        caseType = (CaseType)int.Parse(parsedItems[13]);

        if (parsedItems.Length > 14 && !parsedItems[14].TrimEnd(new char[] { '\n', '\r' }).Equals("")) {
            Texture2D temp = new Texture2D(2, 2);
            try {
                temp.LoadImage(Convert.FromBase64String(parsedItems[14]));
            } catch (FormatException error) {
                Debug.Log(error.Message);
                image = null;
                return;
            }
            image = Sprite.Create(temp, new Rect(0, 0, 100, 100), new Vector2(0, 0), 1);
        } else {
            image = null;
        }
    }

    /**
	 * MenuCases deal with the general info of the case it represents.
	 */
    public MenuCase(string accountID, string filename, string authorName, string patientName,
        string recordNumber, string difficulty, string description, string summary, string tags,
        string modified, string audience, string version, string rating, string caseType)
    {
        this.accountId = int.Parse(accountID);
        this.filename = filename.Replace("_", " ");
        this.authorName = authorName;
        this.patientName = patientName;
        this.recordNumber = recordNumber;
        this.difficulty = difficulty;
        this.description = description;
        this.summary = summary;
        this.tags = tags.Split(',');
        this.dateModified = long.Parse(modified);
        this.version = version;
        this.audience = audience;
        int.TryParse(rating, out this.rating);
        this.caseType = (CaseType)int.Parse(caseType);
    }

    /**
	 * This is used if a MenuCase needs to be populated after being created
	 */
    public MenuCase Instantiate(string accountID, string filename, string authorName, string patientName,
        string recordNumber, string difficulty, string description, string summary, string tags,
        string modified, string audience, string version, string rating, string caseType)
    {
        this.accountId = int.Parse(accountID);
        this.filename = filename.Replace("_", " ");
        this.authorName = authorName;
        this.patientName = patientName;
        this.recordNumber = recordNumber;
        this.difficulty = difficulty;
        this.description = description;
        this.summary = summary;
        this.tags = tags.Split(',');
        this.dateModified = long.Parse(modified);
        this.version = version;
        this.audience = audience;
        int.TryParse(rating, out this.rating);
        this.caseType = (CaseType)int.Parse(caseType);
        return this;
    }

    /**
	 * This is used if a MenuCase needs to be populated after being created 
	 */
    public MenuCase Instantiate(MenuCase mc)
    {
        this.accountId = mc.accountId;
        this.filename = mc.filename;
        this.authorName = mc.authorName;
        this.patientName = mc.patientName;
        this.recordNumber = mc.recordNumber;
        this.difficulty = mc.difficulty;
        this.description = mc.description;
        this.summary = mc.summary;
        this.tags = mc.tags;
        this.dateModified = mc.dateModified;
        this.version = mc.version;
        this.audience = mc.difficulty;
        this.rating = mc.rating;
        this.caseType = mc.caseType;
        return this;
    }

    public int publicCompare = 0x01; //Compare the caseType to this to figure out if it's public or not
    public int templateCompare = 0x02; //Compare the caseType to this to figure out if it's a template or not

    public enum CaseType
    {
        privateCase = 0,
        publicCase = 1,
        privateTemplate = 2,
        publicTemplate = 3
    };

    public bool IsPublic()
    {
        return (caseType.GetHashCode() & publicCompare) > 0;
    }

    public bool IsTemplate()
    {
        return (caseType.GetHashCode() & templateCompare) > 0;
    }

    public string GetTagsAsOneString()
    {
        if (tags == null)
            return "";
        return string.Join(", ", tags);
    }

    public int GetIndexInCourse()
    {
        if (course == null) {
            return -1;
        }
        return course.GetCases().FindIndex((string s) => s.Equals(recordNumber));
    }

    public override string ToString()
    {
        byte[] bytes = null;
        if (image == null) {
            bytes = new byte[0];
        } else {
            bytes = image.texture.EncodeToPNG();
        }

        return
            accountId + "--" +
            filename + "--" +
            authorName + "--" +
            patientName + "--" +
            recordNumber + "--" +
            difficulty + "--" +
            description + "--" +
            summary + "--" +
            GetTagsAsOneString() + "--" +
            dateModified + "--" +
            audience + "--" +
            version + "--" +
            rating + "--" +
            caseType.GetHashCode() + "--" +
            Convert.ToBase64String(bytes); //Image text
    }

    public int ComparePatientNameUp(MenuCase x, MenuCase y)
    {
        return x.patientName.CompareTo(y.patientName);
    }

    public int ComparePatientNameDown(MenuCase x, MenuCase y)
    {
        return y.patientName.CompareTo(x.patientName);
    }

    public int CompareDateUp(MenuCase x, MenuCase y)
    {
        return x.dateModified.CompareTo(y.dateModified);
    }

    public int CompareDateDown(MenuCase x, MenuCase y)
    {
        return y.dateModified.CompareTo(x.dateModified);
    }

    public int CompareAuthorUp(MenuCase x, MenuCase y)
    {
        int returnVal = x.authorName.CompareTo(y.authorName);
        if (returnVal == 0) {
            return Compare(x, y);
        } else {
            return returnVal;
        }
    }

    public int CompareAuthorDown(MenuCase x, MenuCase y)
    {
        int returnVal = y.authorName.CompareTo(x.authorName);
        if (returnVal == 0) {
            return Compare(x, y);
        } else {
            return returnVal;
        }
    }

    public int Compare(MenuCase x, MenuCase y)
    {
        return x.filename.CompareTo(y.filename);
    }

    public enum SortMethod
    {
        None,
        PatientNameUp,
        PatientNameDown,
        DateUp,
        DateDown,
        AuthorUp,
        AuthorDown
    };

    public bool CheckCaseTags(string contains)
    {
        return new List<string>(tags).Exists(tag => tag.ToLower().Contains(contains.ToLower()));
    }

    public void PopulateCaseTransform(GameObject caseObj, bool isList)
    {
        string difficulty = "";
        string patientName;
        string author;
        string audience;
        string tags;
        string filename;
        string text;
        string difficultyTextLoc;
        string beginnerImage;
        string intermediateImage;
        string advancedImage;
        string rating;

        MenuCase m = this;

        //Handle the different views and set all the GameObject textboxes to what they should be
        if (GlobalData.GDS.isMobile) {
            if (isList) {
                patientName = "Display/Group/NameData";
                author = "Display/Group/AuthorData";
                audience = "Display/AudienceData";
                tags = "Display/TagData";
                text = "Text";
                filename = "Filename";
                difficultyTextLoc = "Display/DifficultyData";
                beginnerImage = "Display/BeginnerImage";
                intermediateImage = "Display/IntermediateImage";
                advancedImage = "Display/AdvancedImage";
                rating = "Rating";
            } else {
                patientName = "ImageRows/Rows/Row0/NameData";
                author = "ImageRows/Rows/Row1/AuthorData";
                audience = "Row3/AudienceData";
                tags = "Row2/TagData";
                text = "Text";
                filename = "Filename";
                difficultyTextLoc = "Row3/DifficultyData";
                beginnerImage = "Row3/BeginnerImage";
                intermediateImage = "Row3/IntermediateImage";
                advancedImage = "Row3/AdvancedImage";
                rating = "Row5";

                if (m.image != null) {
                    caseObj.transform.Find("ImageRows/MaskingCircle/PatientImage").GetComponent<Image>().sprite = m.image;
                }
            }
        } else {
            if (isList) {
                patientName = "Display/Group/NameData";
                author = "Display/Group/AuthorData";
                audience = "Display/AudienceData";
                tags = "Display/TagData";
                text = "Text";
                filename = "Filename";
                difficultyTextLoc = "Display/DifficultyData";
                beginnerImage = "Display/BeginnerImage";
                intermediateImage = "Display/IntermediateImage";
                advancedImage = "Display/AdvancedImage";
                rating = "Rating";
            } else {
                patientName = "ImageRows/Rows/Row0/NameData";
                author = "ImageRows/Rows/Row1/AuthorData";
                audience = "Row3/AudienceData";
                tags = "Row2/TagData";
                text = "Text";
                filename = "Filename";
                difficultyTextLoc = "Row3/DifficultyData";
                beginnerImage = "Row3/BeginnerImage";
                intermediateImage = "Row3/IntermediateImage";
                advancedImage = "Row3/AdvancedImage";
                rating = "Row5";

                if (m.image != null) {
                    caseObj.transform.Find("ImageRows/MaskingCircle/PatientImage").GetComponent<Image>().sprite = m.image;
                }
            }
        }

        caseObj.transform.Find(patientName).GetComponentInChildren<TextMeshProUGUI>().text = m.patientName.Replace("_", " "); //patientname will need to go here eventually
        TextMeshProUGUI tmpu = caseObj.transform.Find(author).GetComponentInChildren<TextMeshProUGUI>();
        tmpu.text = m.description;

        //caseObj.transform.Find(author).GetComponentInChildren<TextMeshProUGUI>().text = m.description;//"by " + menuItems [menuItems.Count - 1].authorName;
        caseObj.transform.Find(audience).GetComponentInChildren<TextMeshProUGUI>().text = m.audience; //Need audience variable?
        caseObj.transform.Find(tags).GetComponentInChildren<TextMeshProUGUI>().text = m.GetTagsAsOneString();
        caseObj.transform.Find(text).GetComponentInChildren<TextMeshProUGUI>().text = m.description + ", " + m.filename.Replace(".ced", "");
        caseObj.transform.Find(filename).GetComponentInChildren<TextMeshProUGUI>().text = m.filename;
        difficulty = m.difficulty;
        caseObj.transform.Find(difficultyTextLoc).GetComponentInChildren<TextMeshProUGUI>().text = difficulty;

        caseObj.transform.Find(beginnerImage).gameObject.SetActive(false);
        caseObj.transform.Find(intermediateImage).gameObject.SetActive(false);
        caseObj.transform.Find(advancedImage).gameObject.SetActive(false);
        switch (difficulty) {
            case "Beginner":
                caseObj.transform.Find(beginnerImage).gameObject.SetActive(true);
                break;
            case "Intermediate":
                caseObj.transform.Find(intermediateImage).gameObject.SetActive(true);
                break;
            case "Advanced":
                caseObj.transform.Find(advancedImage).gameObject.SetActive(true);
                break;
            default:
                caseObj.transform.Find(advancedImage).gameObject.SetActive(true);
                break;
        }

        caseObj.transform.Find(rating)?.GetChild(m.rating).gameObject.SetActive(true);

        if (GlobalData.resourcePath.Equals("Reader")) {
            if (m.completed) {
                //Enable completed thingy
                caseObj.transform.Find("CaseCompleted").gameObject.SetActive(true);
            } else if (Tracker.InProgress(m.recordNumber)) {
                caseObj.transform.Find("CaseInProgress")?.gameObject.SetActive(true);
            }
        }
    }
}

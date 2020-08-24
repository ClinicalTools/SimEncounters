using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class XMLManager : MonoBehaviour {

    private dialogueList result;
    private Intro introList;


    // XML Hierarchy for serialization

    [XmlRoot("Dialogue")]
    public class Intro
    {
        [XmlElement("Intro")]
        public IntroNode[] intro;
    }

    public class IntroNode
    {
        [XmlElement("Text")]
        public string introText;
    }
 


    [XmlRoot("Dialogue")]
    public class dialogueList
    {
        [XmlElement("DialogueBlock")]
        public dialogueBlock[] dialogue;
    }
    
    public class dialogueBlock{
        [XmlElement("Title")]
        public string title;

        [XmlElement("Option")]
        public Options[] options;
    }

    public class Options
    {
        [XmlAttribute]
        public string name;

        [XmlElement("Text")]
        public string[] text;

    }

    // Reads the XML file into the result class.
    void parseXML()
    {        
        Debug.Log("Reading XML");
        XmlSerializer dSerializer = new XmlSerializer(typeof(dialogueList));
        XmlSerializer iSerializer = new XmlSerializer(typeof(Intro));

        FileStream ReadFileStream = new FileStream(@"F:\VR Clinical Encounters Concept\VR Clinical Encounters Concept\Assets\Scripts\test.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
        result = (dialogueList)dSerializer.Deserialize(ReadFileStream);
        ReadFileStream.Seek(0, SeekOrigin.Begin);
        introList = (Intro)iSerializer.Deserialize(ReadFileStream);
        ReadFileStream.Close();
        Debug.Log(introList.intro[0].introText);
    }

    
    // Gets the intro text
    public string[] getIntroArray()
    {
        string[] introArr = new string[introList.intro.Length];
        for (int i = 0; i < introArr.Length; i++)
        {
            introArr[i] = introList.intro[i].introText;
        }
        return introArr;
    } 
    

    // Gets the choice text
    public string getTitle()
    {
        return result.dialogue[0].title;
    }

    // Gets the option choices
    public string[] getOptionArray()
    {
        string[] optionArr = new string[result.dialogue[0].options.Length]; 
        for (int i = 0; i < optionArr.Length; i++)
        {
            optionArr[i] = result.dialogue[0].options[i].name;
        }
        return optionArr;
    }

    // Gets all of the text associated with a specific option choice
    public string[] getTextArray(int currOption)
    {
        string[] textArr = new string[result.dialogue[0].options[currOption].text.Length];
        for (int i = 0; i < textArr.Length; i++)
        {
            textArr[i] = result.dialogue[0].options[currOption].text[i];
        }
        return textArr;
    }

    public string getNextTextElement(int currOption, int currText)
    {
        if (currText == result.dialogue[0].options[currOption].text.Length)
        {
            return null;
        }
        return result.dialogue[0].options[currOption].text[currText];
    } 
    

	// Use this for initialization
	void Start () {
        parseXML();
	}
	
}

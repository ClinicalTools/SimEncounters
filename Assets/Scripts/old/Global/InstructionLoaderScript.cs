using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InstructionLoaderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*
		string fullPath = Application.streamingAssetsPath + "/Instructions/Instructions.csv";
		StreamReader reader = new StreamReader (fullPath);

		string lineText = reader.ReadLine ();
		while (!reader.EndOfStream) {
			string[] line = lineText.Split (',');
			GlobalData.instructionsDict.Add (line [0], line [1]);
			lineText = reader.ReadLine ();
		}
		reader.Close ();
		*/
	}
}

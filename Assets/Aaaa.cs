using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Aaaa : MonoBehaviour
{
    protected static TextMeshProUGUI label;
    static string shortLog = "", longLog = "";

    // Start is called before the first frame update
    void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
        label.text = shortLog;
    }

    public static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        var line = $"{GetTypePrefix(type)}: {condition}\n";
        shortLog += line;
        if (label != null)
            label.text += line;
        longLog += $"{GetTypePrefix(type)}: {condition}\n{stackTrace}\n\n";
    }

    // Update is called once per frame
    static string GetTypePrefix(LogType type)
    {
        if (type== LogType.Exception)
            return "EX";
        else if (type == LogType.Log)
            return "L";
        else if (type == LogType.Error)
            return "ER";
        else if (type == LogType.Warning)
            return "W";
        return "";
    }

    private void OnApplicationQuit()
    {
        System.IO.File.WriteAllText(@"C:\Users\tanner\Desktop\Builds\SE\Desktop\2\blah.txt", shortLog);
        System.IO.File.WriteAllText(@"C:\Users\tanner\Desktop\Builds\SE\Desktop\2\blah2.txt", longLog);
    }
}

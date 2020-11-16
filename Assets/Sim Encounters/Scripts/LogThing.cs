using System;
using System.IO;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class LogThing : MonoBehaviour
    {
        public TextMeshProUGUI label;

        // Start is called before the first frame update
        protected virtual void Awake()
        {
            Application.logMessageReceived += HandleLog;
        }

        protected virtual void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        protected virtual string GetLogTypePrefix(LogType type)
        {
            if (type == LogType.Assert)
                return "AST";
            else if (type == LogType.Exception)
                return "EXC";
            else if (type == LogType.Log)
                return "LOG";
            else if (type == LogType.Error)
                return "ERR";
            else if (type == LogType.Warning)
                return "WRN";
            else
                return "NUL";
        }

        protected virtual void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (string.IsNullOrWhiteSpace(logString) || type != LogType.Warning)
                return;
            label.text += $"{logString}\n";
        }
    }
}
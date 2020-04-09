using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class DoubleTildeStringSplitter : IStringSplitter
    {
        private const string divider = "~~";
        public string[] Split(string str) => str.Split(new string[] { divider }, StringSplitOptions.None);
    }
    public class DoubleColonStringSplitter : IStringSplitter
    {
        private const string divider = "::";
        public string[] Split(string str) => str.Split(new string[] { divider }, StringSplitOptions.None);
    }
}
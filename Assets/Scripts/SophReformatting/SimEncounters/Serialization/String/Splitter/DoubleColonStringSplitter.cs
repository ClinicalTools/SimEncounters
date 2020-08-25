﻿using System;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class DoubleColonStringSplitter : IStringSplitter
    {
        private const string divider = "::";
        public string[] Split(string str) => str.Split(new string[] { divider }, StringSplitOptions.None);
    }
}
﻿namespace ClinicalTools.SimEncounters
{
    public class PinData
    {
        public virtual DialoguePin Dialogue { get; set; }
        public virtual QuizPin Quiz { get; set; }

        public virtual bool HasPin() => Dialogue != null || Quiz != null;
    }
}
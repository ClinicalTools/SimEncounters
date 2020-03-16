using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class Category
    {
        public List<EncounterInfo> Encounters { get; } = new List<EncounterInfo>();
    }
}
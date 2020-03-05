using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class Category
    {
        public List<EncounterDetail> Encounters { get; } = new List<EncounterDetail>();
    }
}
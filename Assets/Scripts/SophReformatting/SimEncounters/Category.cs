using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class Category
    {
        public List<EncounterInfo> Encounters { get; } = new List<EncounterInfo>();

        public bool IsCompleted()
        {
            foreach (var encounter in Encounters)
                if (encounter.UserStatus == null || !encounter.UserStatus.Completed)
                    return false;

            return true;
        }
    }
}
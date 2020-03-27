using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class Category
    {
        public string Name { get; }
        public List<EncounterInfo> Encounters { get; } = new List<EncounterInfo>();

        public Category(string name)
        {
            Name = name;
        }

        public bool IsCompleted()
        {
            foreach (var encounter in Encounters)
                if (encounter.UserStatus == null || !encounter.UserStatus.Completed)
                    return false;

            return true;
        }
    }
}
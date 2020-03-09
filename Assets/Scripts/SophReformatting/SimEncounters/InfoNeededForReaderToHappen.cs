using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class InfoNeededForMainMenuTohappen
    {
        public User User { get; }

        public Dictionary<string, Category> Categories { get; } = new Dictionary<string, Category>();

        public InfoNeededForMainMenuTohappen(User user)
        {
            User = user;
        }

        public void AddEncounterDetail(EncounterDetail encounterDetail)
        {
            var categories = encounterDetail.InfoGroup.GetLatestInfo().Categories;
            foreach (var category in categories)
            {
                if (Categories.ContainsKey(category))
                    Categories[category].Encounters.Add(encounterDetail);
                else
                    Categories.Add(category, new Category());
            }
        }
    }

    public class InfoNeededForReaderToHappen
    {
        public User User { get; }

        public EncounterDetail EncounterDetail { get; }

        public Encounter Encounter { get; private set; }

        public event Action<Encounter> EncounterLoaded;


        public List<EncounterDetail> SuggestedEncounters { get; } = new List<EncounterDetail>();

        public InfoNeededForReaderToHappen(User user, EncounterDetail encounterDetail, Encounter encounter)
        {
            User = user;
            EncounterDetail = encounterDetail;
            Encounter = encounter;
        }

        public InfoNeededForReaderToHappen(User user, EncounterDetail encounterDetail, IEncounterGetter encounterGetter)
        {
            User = user;
            EncounterDetail = encounterDetail;
            if (encounterGetter.IsDone)
                Encounter = encounterGetter.Encounter;
            else
                encounterGetter.Completed += EncounterGetter_Completed;
        }

        private void EncounterGetter_Completed(Encounter encounter)
        {
            Encounter = encounter;
            EncounterLoaded?.Invoke(Encounter);
        }
    }
}
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class InfoNeededForMainMenuToHappen
    {
        public User User { get; }

        public Dictionary<string, Category> Categories { get; } = new Dictionary<string, Category>();

        public event Action<Dictionary<string, Category>> CategoriesLoaded;
        public bool IsDone { get; protected set; }

        public InfoNeededForMainMenuToHappen(User user, IEncountersInfoReader encountersInfoReader)
        {
            User = user;
            encountersInfoReader.Completed += EncountersInfoReader_Completed;
            encountersInfoReader.GetEncounterInfos(user);
        }

        private void EncountersInfoReader_Completed(List<EncounterDetail> encounterDetails)
        {
            foreach (var encounterDetail in encounterDetails)
                AddEncounterDetail(encounterDetail);
            IsDone = true;
            CategoriesLoaded.Invoke(Categories);
        }

        public void AddEncounterDetail(EncounterDetail encounterDetail)
        {
            var latestInfo = encounterDetail.InfoGroup.GetLatestInfo();
            if (latestInfo.IsTemplate)
                return;

            var categories = latestInfo.Categories;
            foreach (var category in categories) {
                if (string.IsNullOrWhiteSpace(category))
                    continue;

                if (!Categories.ContainsKey(category))
                    Categories.Add(category, new Category());

                Categories[category].Encounters.Add(encounterDetail);
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
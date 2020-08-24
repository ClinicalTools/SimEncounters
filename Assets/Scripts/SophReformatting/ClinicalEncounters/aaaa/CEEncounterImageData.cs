using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.ClinicalEncounters.Data
{
    public class CEEncounterImageData : EncounterImageContent
    {
        public virtual Dictionary<string, LegacyIcon> LegacyIconsInfo { get; } = new Dictionary<string, LegacyIcon>();
    }
}
using ClinicalTools.SimEncounters;
using System.Collections.Generic;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEEncounterImageData : EncounterImageContent
    {
        public virtual Dictionary<string, LegacyIcon> LegacyIconsInfo { get; } = new Dictionary<string, LegacyIcon>();
    }
}
﻿using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IImageDataReader
    {
        WaitableResult<EncounterImageContent> GetImageData(User user, EncounterMetadata metadata);
    }
}
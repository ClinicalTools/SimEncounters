﻿namespace ClinicalTools.SimEncounters
{
    public class AutosaveFileExtensionManager : IFileExtensionManager
    {
        public string GetExtension(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Data:
                    return "aced";
                case FileType.Image:
                    return "acei";
                case FileType.Metadata:
                    return "acem";
                default:
                    return null;
            }
        }
    }
}
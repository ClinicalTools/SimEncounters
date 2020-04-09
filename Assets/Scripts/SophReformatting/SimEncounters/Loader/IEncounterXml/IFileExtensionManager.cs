namespace ClinicalTools.SimEncounters
{
    public enum FileType
    {
        Data, Image, BasicStatus, DetailedStatus, Metadata
    }
    public interface IFileExtensionManager
    {
        string GetExtension(FileType fileType);
    }
    public class FileExtensionManager : IFileExtensionManager
    {
        public string GetExtension(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Data:
                    return "ced";
                case FileType.Image:
                    return "cei";
                case FileType.BasicStatus:
                    return "ces";
                case FileType.DetailedStatus:
                    return "cesd";
                case FileType.Metadata:
                    return "cem";
                default:
                    return null;
            }
        }
    }
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
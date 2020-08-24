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
}
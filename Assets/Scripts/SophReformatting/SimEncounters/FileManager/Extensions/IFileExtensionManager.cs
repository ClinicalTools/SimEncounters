namespace ClinicalTools.SimEncounters
{
    public interface IFileExtensionManager
    {
        string GetExtension(FileType fileType);
    }
}
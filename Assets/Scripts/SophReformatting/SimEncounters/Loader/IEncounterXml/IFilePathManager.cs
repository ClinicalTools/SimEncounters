namespace ClinicalTools.SimEncounters
{
    public interface IFilePathManager
    {
        string AutoSaveDataFilePath(string filePath);
        string AutoSaveImageFilePath(string filePath);
        string DataFilePath(string filePath);
        string GetLocalSavesFolder(User user);
        string ImageFilePath(string filePath);
        string StatusFilePath(string filePath);
        string DetailedStatusFilePath(string filePath);
        string EncounterFilePath(User user, EncounterInfo encounterInfo);
    }
}
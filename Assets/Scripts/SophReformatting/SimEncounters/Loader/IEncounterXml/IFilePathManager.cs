namespace ClinicalTools.SimEncounters.Loading
{
    public interface IFilePathManager
    {
        string AutoSaveDataFilePath(string filePath);
        string AutoSaveImageFilePath(string filePath);
        string DataFilePath(string filePath);
        string GetLocalFolder(User user);
        string ImageFilePath(string filePath);
    }
}
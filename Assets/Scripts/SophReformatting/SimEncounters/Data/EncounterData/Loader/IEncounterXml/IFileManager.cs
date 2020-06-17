using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IFileManager
    {
        //string GetFile(User user, FileType fileType, EncounterMetadata metadata);
        //WaitableResult<string[]> GetFile(User user, FileType fileType);
        void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents);

        WaitableResult<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata);
        WaitableResult<string[]> GetFilesText(User user, FileType fileType);
    }
}
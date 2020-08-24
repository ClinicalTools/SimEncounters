using System.IO;

namespace ClinicalTools.ClinicalEncounters.MainMenu
{
    public class FilenameUpdater
    {
        public void UpdateFileNames(string path, string oldFilePrefix, string newFilePrefix)
        {
            var files = Directory.GetFiles(path, $"{oldFilePrefix}*");
            foreach (var file in files)
                File.Move(file, file.Replace(oldFilePrefix, newFilePrefix));
        }
    }
}
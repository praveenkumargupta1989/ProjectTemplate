using System.IO;

namespace ACIPL.Template.Core.Utilities
{
    public interface IFileManager
    {
        bool Zip(string sourceDirectory, string destinationZipFilePath);
        bool Delete(string fileName);
        bool CreateDirectory(string directoryPath, bool reCreate = false);
        string UnZip(string directoryName, string fileName, string filename);
        bool TextWritter(string path, string data);
        bool Create(string filepath);
        void Move(string sourcePath, string destinationPath);
    }

    public class FileManager : IFileManager
    {
        public bool Zip(string sourceDirectory, string destinationZipFilePath)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                zip.AddDirectory(sourceDirectory, "");
                zip.Save(destinationZipFilePath);
            }

            return true;
        }

        public bool Delete(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            return true;
        }

        public bool CreateDirectory(string directoryPath, bool reCreate = false)
        {
            if (reCreate && Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return true;
        }

        public string UnZip(string directoryName, string fileName, string filename)
        {
            string unzipFileName = "";
            if (File.Exists(fileName))
            {
                //using (ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(fileName))
                //{
                //    foreach (ZipArchiveEntry entry in archive.Entries)
                //    {
                //        if (entry.FullName.EndsWith(".txt", System.StringComparison.OrdinalIgnoreCase) || entry.FullName.EndsWith(".json", System.StringComparison.OrdinalIgnoreCase))
                //        {
                //            unzipFileName = directoryName + filename + "\\" + entry.FullName;
                //            System.IO.Compression.ZipFile.ExtractToDirectory(fileName, directoryName + filename);
                //        }
                //    }
                //}
                return unzipFileName;
            }
            return unzipFileName;
        }

        public bool TextWritter(string path, string data)
        {
            if (data.Length > 0)
            {
                using (TextWriter tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(data);
                    tw.Close();
                }
            }
            return true;
        }

        public bool Create(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            File.Create(filepath).Close();
            return true;
        }

        public void Move(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, destinationPath);
            }
        }
    }
}

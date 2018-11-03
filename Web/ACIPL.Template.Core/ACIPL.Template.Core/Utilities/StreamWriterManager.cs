using System;
using System.IO;

namespace ACIPL.Template.Core.Utilities
{
    public interface IStreamWriterManager
    {
        bool StreamWriteFromBase64(string filePath, string data);
    }
    public class StreamWriterManager : IStreamWriterManager
    {
        public bool StreamWriteFromBase64(string filePath, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    foreach (byte value in Convert.FromBase64String(data))
                    {
                        fs.WriteByte(value);
                    }
                    fs.Close();
                }
            }
            return true;
        }
    }
}

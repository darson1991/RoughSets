using System.IO;

namespace BusinessLogic.Helpers
{
    public static class FileOperations
    {
        public static string GetFileContent(string url)
        {
            using (var streamReader = new StreamReader(url))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}

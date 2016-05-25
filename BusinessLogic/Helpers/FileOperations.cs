using System;
using System.IO;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Helpers
{
    public static class FileOperations
    {
        public static string GetFileContent(string url)
        {
            try
            {
                using (var streamReader = new StreamReader(url))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                throw new FileOperationsException(exception);
            }
        }
    }
}

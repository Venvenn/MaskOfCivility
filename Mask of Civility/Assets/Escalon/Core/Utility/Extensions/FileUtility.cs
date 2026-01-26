using System.IO;

namespace Escalon
{
    public static class FileUtility
    {
        public static void TryCreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
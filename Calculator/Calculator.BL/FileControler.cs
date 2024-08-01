namespace Calculator.BL
{
    public static class FileControler
    {
        public static (bool isCorrect, string errorMessage) FileISCorrect(string filePath)
        {
            if (filePath == null)
            {
                return (false, "File path is null");
            }

            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return (false, "File doesn't exist");
            }
            else if (fileInfo.Extension != ".txt")
            {
                return (false, "File doesn't txt");
            }
            else if (fileInfo.Length == 0)
            {
                return (false, "File is empty");
            }
            return (true, "");
        }

        public static bool DirectoryISCorrect(string directoryPath)
        {
            if (directoryPath == null)
            {
                return false;
            }

            var directoryInfo = new DirectoryInfo(directoryPath);
            if (!directoryInfo.Exists)
            {
                return false;
            }

            return true;
        }
    }
}

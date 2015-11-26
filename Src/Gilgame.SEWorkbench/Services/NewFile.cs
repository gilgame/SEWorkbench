using System;
using System.IO;
using System.Text;

namespace Gilgame.SEWorkbench.Services
{
    public static class NewFile
    {
        private const string FILE = "NewFile.csx";

        public static string Contents
        {
            get
            {
                return GetContents();
            }
        }

        private static string GetContents()
        {
            string filename = GetFilename();
            if (!File.Exists(filename))
            {
                return CreateNewFile();
            }
            else
            {
                return File.ReadAllText(filename);
            }
        }

        private static string CreateNewFile()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("void Main(string argument)");
            result.AppendLine("{");
            result.AppendLine("    ");
            result.AppendLine("}");

            return result.ToString();
        }

        private static string GetFilename()
        {
            return String.Format("{0}{1}{2}", Environment.CurrentDirectory, Path.DirectorySeparatorChar, FILE);
        }
    }
}

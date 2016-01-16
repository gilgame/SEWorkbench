using System;
using System.Text;

using Gilgame.SEWorkbench.Services.IO;

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
                return File.Read(filename);
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
            return Path.Combine(Directory.GetCurrentDirectory(), FILE);
        }
    }
}

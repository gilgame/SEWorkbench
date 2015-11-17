using System;
using System.IO;
using System.Text;

namespace Gilgame.SEWorkbench.Services
{
    public static class NewFile
    {
        private const string FILE = "NewFiles.csx";

        public static string Contents
        {
            get
            {
                return GetContents();
            }
        }

        private static string GetContents()
        {
            if (!File.Exists(FILE))
            {
                return CreateNewFile();
            }
            else
            {
                return File.ReadAllText(FILE);
            }
        }

        private static string CreateNewFile()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("var main");
            result.AppendLine("{");
            result.AppendLine("    ");
            result.AppendLine("}");
            result.AppendLine();

            return result.ToString();
        }
    }
}

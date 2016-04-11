using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Gilgame.SEWorkbench.Services
{
    public static class Hasher
    {
        public static string File(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return String.Empty;
            }

            StringBuilder result = new StringBuilder();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                byte[] hash = MD5.Create().ComputeHash(stream);
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("X2").ToLower());
                }
            }
            return result.ToString();
        }

        public static string Filename(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return String.Empty;
            }

            StringBuilder result = new StringBuilder();

            byte[] bytes = new UTF8Encoding().GetBytes(path);

            byte[] hash = MD5.Create().ComputeHash(bytes);
            foreach (byte b in hash)
            {
                result.Append(b.ToString("X2").ToLower());
            }
            return result.ToString();
        }
    }
}

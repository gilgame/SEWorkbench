using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Gilgame.SEWorkbench.Updater
{
    public static class Hasher
    {
        public static string File(string path)
        {
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
    }
}

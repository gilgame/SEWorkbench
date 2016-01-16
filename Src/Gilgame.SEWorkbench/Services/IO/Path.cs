using System;

namespace Gilgame.SEWorkbench.Services.IO
{
    public static class Path
    {
        public static string Combine(params string[] paths)
        {
            return System.IO.Path.Combine(paths);
        }

        public static string GetDirectoryName(string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public static string GetFileName(string path)
        {
            return System.IO.Path.GetFileName(path);
        }

        public static string GetRandomFileName()
        {
            return System.IO.Path.GetRandomFileName();
        }
    }
}

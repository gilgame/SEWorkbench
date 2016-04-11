using System;

namespace Gilgame.SEWorkbench.Services.IO
{
    public static class File
    {
        public static string Read(string path)
        {
            try
            {
                return System.IO.File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to read file ({0})", path), ex);
                return String.Empty;
            }
        }

        public static bool Write(string path, string contents)
        {
            try
            {
                System.IO.File.WriteAllText(path, contents);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to write file ({0})", path), ex);
                return false;
            }
        }

        public static bool Copy(string source, string destination)
        {
            try
            {
                System.IO.File.Copy(source, destination);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to copy file ({0})", source), ex);
                return false;
            }
        }

        public static bool Move(string source, string destination)
        {
            try
            {
                System.IO.File.Move(source, destination);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to move file ({0})", source), ex);
                return false;
            }
        }

        public static bool Delete(string path)
        {
            try
            {
                System.IO.File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to delete file ({0})", path), ex);
                return false;
            }
        }

        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static DateTime? LastWriteTime(string path)
        {
            if (String.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }
            try
            {
                System.IO.FileInfo info = new System.IO.FileInfo(path);
                return info.LastWriteTime;
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to access file ({0})", path), ex);
                return null;
            }
        }
    }
}

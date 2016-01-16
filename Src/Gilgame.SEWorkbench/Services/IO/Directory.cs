using System;

namespace Gilgame.SEWorkbench.Services.IO
{
    public static class Directory
    {
        public static string GetCurrentDirectory()
        {
            try
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
            catch (Exception ex)
            {
                Services.MessageBox.ShowError("Unable to retrieve the current directory", ex);
                return String.Empty;
            }
        }

        public static System.IO.DirectoryInfo CreateDirectory(string path)
        {
            try
            {
                if (!Exists(path))
                {
                    return System.IO.Directory.CreateDirectory(path);
                }
                else
                {
                    return new System.IO.DirectoryInfo(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to create directory ({0})", path), ex);
                return new System.IO.DirectoryInfo(".");
            }
        }

        public static bool Move(string source, string destination)
        {
            try
            {
                System.IO.Directory.Move(source, destination);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to move directory ({0})", source), ex);
                return false;
            }
        }

        public static bool Delete(string path, bool recursive = false)
        {
            try
            {
                System.IO.Directory.Delete(path, recursive);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to delete directory ({0})", path), ex);
                return false;
            }
        }

        public static bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static System.IO.DirectoryInfo GetParent(string path)
        {
            try
            {
                return System.IO.Directory.GetParent(path);
            }
            catch (Exception ex)
            {
                MessageBox.ShowError(String.Format("Failed to access directory ({0})", path), ex);
                return new System.IO.DirectoryInfo(".");
            }
        }
    }
}

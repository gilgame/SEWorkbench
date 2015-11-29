using System;
using System.Windows;

namespace Gilgame.SEWorkbench.Configuration
{
    public static class Convert
    {
        public static bool ToBoolean(object o)
        {
            try
            {
                return System.Convert.ToBoolean(o);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }
        }

        public static int ToInteger(object o)
        {
            try
            {
                return System.Convert.ToInt32(o);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return -1;
            }
        }

        public static string ToString(object o)
        {
            try
            {
                return o.ToString();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return String.Empty;
            }
        }

        private static void LogError(Exception ex)
        {
            // TODO log errors
        }
    }
}

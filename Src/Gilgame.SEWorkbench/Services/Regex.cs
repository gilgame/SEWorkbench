using System;

namespace Gilgame.SEWorkbench.Services
{
    public static class Regex
    {
        public static readonly string BlockName = @"^[^_\- ]+[a-zA-Z0-9 _\-]*$";

        public static readonly string BlockNameChar = @"^[a-zA-Z0-9 _\-]+$";
    }
}

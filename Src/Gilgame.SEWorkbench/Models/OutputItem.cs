using System;

namespace Gilgame.SEWorkbench.Models
{
    public class OutputItem
    {
        public int Line { get; set; }

        public int Column { get; set; }

        public string Error { get; set; }

        public string Message { get; set; }

        public string Filename { get; set; }
    }
}

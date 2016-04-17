using System;

namespace Gilgame.SEWorkbench.Models
{
    public class BackupItem
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Original { get; set; }

        public DateTime? Modified { get; set; }

        public string Contents { get; set; }
    }
}

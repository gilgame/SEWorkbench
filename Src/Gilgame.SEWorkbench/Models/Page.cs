﻿using System;

namespace Gilgame.SEWorkbench.Models
{
    public class Page
    {
        public string Name { get; set; }

        public Guid Identifier { get; set; }

        public PageType Type { get; set; }
    }
}

﻿using System.Collections.Generic;

namespace Hyproc
{
    public class TableComparator<T>
    {
        public List<T> MasterTable { get; set; }
        public List<T> SlaveTable { get; set; }
        public void LoadMasterAndSlave()
        { }
    }
}

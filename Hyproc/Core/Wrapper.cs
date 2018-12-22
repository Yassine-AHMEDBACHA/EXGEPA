﻿using CORESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyproc.Core
{
    public class Wrapper<TData>
    {
        public IDataProvider<TData> provider { get; set; }
        public TData Data { get; set; }
        public DbOperation DbOperation { get; set; }

        public Wrapper(TData data, DbOperation dbOperation)
        {
            this.DbOperation = dbOperation;
            this.Data = data;
        }

        public Wrapper()
        {

        }
    }

    public enum DbOperation
    {
        Update = 1,
        Delete,
        Insert
    }
}

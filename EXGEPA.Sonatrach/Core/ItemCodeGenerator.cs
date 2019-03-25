﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Core.Interfaces;
using EXGEPA.Core.Tools;

namespace EXGEPA.Sonatrach.Core
{
    public class ItemCodeGenerator //: IItemCodeGenerator
    {
        public int Priority { get { return 100; } }

        public string Generate(object data = null, int length = 6)
        {
            string query = "SELECT ISNULL(MAX(CONVERT(INTEGER,[Key])),1) FROM ITEMS";
            var dbFacade = ServiceLocator.Resolve<IDbFacade>();
            int result = dbFacade.ExecuteScalaire<int>(query);
            result++;
            var code = KeyLengthNormalizer.Normalize(result.ToString(), length);
            return code;
        }
    }
}
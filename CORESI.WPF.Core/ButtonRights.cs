using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Security;

namespace CORESI.WPF.Core
{
    public class ButtonRights : IButtonRights
    {
        public int Priority => 0;

        public bool CanDoAction(string actionName)
        {
            return true;
        }
    }
}

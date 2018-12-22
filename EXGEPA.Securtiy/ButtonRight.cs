using CORESI.WPF;
using System.Collections.Generic;
using System;
using System.Collections.Concurrent;

namespace EXGEPA.Securtiy
{
    public class ButtonRight : IButtonRights
    {
        public Dictionary<string, bool> Rights { get; set; }

        public ButtonRight()
        {
            this.Rights = new Dictionary<string, bool>();
        }

        public int Priority
        {
            get
            {
                return 10;
            }
        }

        public bool CanDoAction(string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
                return true;
            bool hasRight;
            if (!Rights.TryGetValue(actionName, out hasRight))
                return true;
            return hasRight;
        }
    }
}

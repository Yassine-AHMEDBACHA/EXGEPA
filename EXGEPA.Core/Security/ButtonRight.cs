using System.Collections.Generic;
using System.Linq;
using CORESI.IoC;
using CORESI.Security;
using CORESI.WPF;

namespace EXGEPA.Core.Security
{
    public class ButtonRight : IButtonRights
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly RightManager rightManager;

        public ButtonRight()
        {
            ServiceLocator.Resolve(out this.rightManager);
        }

        public int Priority => 10;

        public bool CanDoAction(string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
                return true;
            return this.rightManager.HasAccess(actionName);
        }

        
    }
}

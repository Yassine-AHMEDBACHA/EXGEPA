using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Security;
using EXGEPA.Model;

namespace EXGEPA.Sonatrach.Core
{
    public class ItemValidator : IValidator<Item>
    {
        public List<int> ValidationLevels
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }

    public enum ValidationLevel
    {
        Creation,
    }
}

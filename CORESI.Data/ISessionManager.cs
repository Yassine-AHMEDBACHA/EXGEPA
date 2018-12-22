using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Data
{
    public interface ISessionManager
    {
        Session OpenSession(string applicationLogin = "Unknown", string applicationName = null);

        void SetApplicationName(string name);

        Session CurrentSession { get; }
        void CloseSession();
    }
}

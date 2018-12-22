using CORESI.Data;
using System;

namespace Hyproc.Core
{
    public interface IAnlayzer
    {
        void UpdateDatabase(IDbFacade targetDbFacade);
    }
}
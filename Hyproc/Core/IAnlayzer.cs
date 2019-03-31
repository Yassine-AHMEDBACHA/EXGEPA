using CORESI.Data;

namespace Hyproc.Core
{
    public interface IAnlayzer
    {
        void UpdateDatabase(IDbFacade targetDbFacade);
    }
}
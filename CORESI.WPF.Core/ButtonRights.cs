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

namespace CORESI.Security
{
    public interface ILoginManager<T> where T : IOperator
    {
        T OpenSession(string login, string password);
        bool UpdatePassword(string login, string password);
        string ResetPassword(string login);
    }
}

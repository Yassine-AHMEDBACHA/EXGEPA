namespace CORESI.DataAccess.Core
{
    public class DataBase
    {
        public DataBase(string connexionString)
        {
            this.ConnexionString = connexionString;
        }
        public string ConnexionString { get; private set; }
    }
}

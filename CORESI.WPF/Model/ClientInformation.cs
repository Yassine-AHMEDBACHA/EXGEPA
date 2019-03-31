using System;
using CORESI.Security;

namespace CORESI.WPF.Model
{
    public class ClientInformation : ICloneable
    {
        public int Id { get; set; }
        public string   Login { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
       // public virtual AccountState SessionState { get; set; }


        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

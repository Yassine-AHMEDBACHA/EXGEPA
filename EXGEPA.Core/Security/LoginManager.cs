﻿using CORESI.Data;
using CORESI.IoC;
using CORESI.Security;
using EXGEPA.Model;
using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace EXGEPA.Security
{
    [Export(typeof(ILoginManager<IOperator>))]
    public class LoginManager : ILoginManager<IOperator>
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDataProvider<Operator> ApplicationOperatorService { get; set; }
        IStringCryptor StringCryptor { get; set; }

        public LoginManager()
        {

            this.ApplicationOperatorService = ServiceLocator.Resolve<IDataProvider<Operator>>();
            this.StringCryptor = new StringCryptor();
            if (!ApplicationOperatorService.HasRows())
            {
                ApplicationOperatorService.Add(new Operator()
                {
                    Key = "$",
                    Password = StringCryptor.Crypte("$"),
                    Name = "Administrator"
                });
            }
        }
        private string GenerateNewPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string newPassword = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return newPassword.ToLower();
        }

        public string ResetPassword(string login)
        {
            string password = GenerateNewPassword(8);
            UpdatePassword(login, password, true);
            return password;
        }

        public IOperator OpenSession(string login, string password)
        {
            IOperator user = null;
            try
            {
                string cryptedPassword = StringCryptor.Crypte(password);
                System.Collections.Generic.IList<Operator> users = ApplicationOperatorService.SelectAll();
                user = users.FirstOrDefault(opr => opr?.Key == login && opr?.Password == cryptedPassword);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return user;
        }


        public bool UpdatePassword(string login, string password, bool toBeReset = false)
        {
            Operator user = GetOperatorByLogin(login); user.Password = StringCryptor.Crypte(password);
            user.ExpiredPassword = toBeReset;
            return this.ApplicationOperatorService.Update(user) > 0;
        }

        private Operator GetOperatorByLogin(string login)
        {
            System.Collections.Generic.IList<Operator> users = ApplicationOperatorService.SelectAll();
            Operator user = users.First(opr => opr.Key == login);

            return user;
        }


        public bool UpdatePassword(string login, string password)
        {
            return this.UpdatePassword(login, password, false);
        }
    }
}

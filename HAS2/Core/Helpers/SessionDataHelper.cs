using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAS2.Core.Helpers
{
    public class SessionDataHelper
    {
        private static Byrom.Framework.Objects.systemBaseUser _currentByromuser = null;
        public static Byrom.Framework.Objects.systemBaseUser CurrentByromUser
        {
            get {
                Byrom.Framework.Objects.systemBaseUser _user = new Byrom.Framework.Objects.systemBaseUser();            
                _user.GlobalID = 1191;
                _user.EventID = 191;
                _user.Login = "joan.seijas";
                _user.Password = Byrom.Framework.Security.CommonSecurity.GenerateHashMD5("123456789");
                return _user;
            }
            set
            {
                _currentByromuser = value;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using GpuStore.WebUI.Infrastructure.Abstract;

namespace GpuStore.WebUI.Infrastructure.Concrete
{
    public class FormAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);
            if (result)
                FormsAuthentication.SetAuthCookie(username, false);
            return result;
        }
    }
}
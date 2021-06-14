using sievis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sievis.Service
{
    public class AppServices
    {

        public static AppUsers AppUser
        {
            get
            {
                return HttpContext.Current.Session["APP_USER"] as AppUsers;
            }
        }
    }
}
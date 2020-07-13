using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
namespace TuiXachOnline.Models
{
    public class SSO
    {
        QLTuiXachEntities db = new QLTuiXachEntities();
        public string emailaddress { get; set; }
        
        public string givenname { get; set; }
        public string surname { get; set; }
        public string nameidentifier { get; set; }

        internal static SSO GetLoginInfo(ClaimsIdentity identity)
        {
            if (identity.Claims.Count() == 0 || identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email) == null)
            {
                return null;
            }

            return new SSO
            {
                emailaddress = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value,
                givenname = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value,
                nameidentifier = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
            };
        }
    }
}
using TuiXachOnline.Models;
using Microsoft.Owin.Security.Cookies;
using System.Security.Claims;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;


namespace TuiXachOnline.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public void SignIn(string ReturnUrl = "/", string type = "")
        {
            if (!Request.IsAuthenticated)
            {
                if (type == "Google")
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "Account/GoogleLoginCallback" }, "Google");
                }
            }
        }
        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Redirect("~/");
        }

        [AllowAnonymous]
        public ActionResult GoogleLoginCallback()
        {
            var claimsPrincipal = HttpContext.User.Identity as ClaimsIdentity;
          
            var loginInfo = SSO.GetLoginInfo(claimsPrincipal);
            if (loginInfo == null)
            {
                return RedirectToAction("Index");
            }


            QLTuiXachEntities db = new QLTuiXachEntities(); //DbContext
            var user = db.KHACH_HANG.FirstOrDefault(x => x.Email == loginInfo.emailaddress);
            
            if (user == null)
            {
                user = new KHACH_HANG
                {
                    
                    Email = loginInfo.emailaddress,
                    TenKH = loginInfo.givenname,
                    TenDN = loginInfo.nameidentifier,

                };
                db.KHACH_HANG.Add(user);
                db.SaveChanges();
            }

            Session["Tenkh"] = loginInfo.givenname;
            Session["Makh"] = user.MaKH;
            
            var ident = new ClaimsIdentity(
                    new[] { 
									// adding following 2 claim just for supporting default antiforgery provider
									new Claim(ClaimTypes.NameIdentifier, user.Email),
                                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                                    new Claim(ClaimTypes.Name, user.TenKH),
                                    new Claim(ClaimTypes.Email, user.Email),
									// optionally you could add roles if any
									new Claim(ClaimTypes.Role, "User"),
                                 
                                    
                    },
                    CookieAuthenticationDefaults.AuthenticationType);


            HttpContext.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
            return Redirect("~/");

        }
        
        

    }
}
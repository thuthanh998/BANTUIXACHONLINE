using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace TuiXachOnline
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Index"),
                SlidingExpiration = true
            });
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()//hàm cung cấp id xác thực ứng dụng trên google
            {
                ClientId = "134741838966-2o42qskvceb8jmsd23lmhcb8j3cbf67i.apps.googleusercontent.com",
                ClientSecret = "qEkMjToTeFvJFestT1aO29RF",
                CallbackPath = new PathString("/GoogleLoginCallback")
            });
        }
    }
}

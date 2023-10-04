using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using AvaChemAdminPanelMobiAPI.App_Start;
using AvaChemAdminPanelMobiAPI.Common_File;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(AvaChemAdminPanelMobiAPI.Startup))]
namespace AvaChemAdminPanelMobiAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);
            app.UseOAuthAuthorizationServer(new AvaChemOAuthOptions());
            app.UseJwtBearerAuthentication(new AvaChemJwtOptions());
            app.UseWebApi(config);
        }

        public class AvaChemJwtOptions : JwtBearerAuthenticationOptions
        {
            public AvaChemJwtOptions()
            {
                var issuer = ConfigurationManager.AppSettings["issuer"];
                var audience = ConfigurationManager.AppSettings["audience"];

                var key = Convert.FromBase64String(ConfigurationManager.AppSettings["JwtKey"]);
                AllowedAudiences = new[] { audience };
                IssuerSecurityKeyProviders = new[]
                {
                new SymmetricKeyIssuerSecurityKeyProvider(issuer, key)
            };
            }
        }

        public class AvaChemOAuthOptions : OAuthAuthorizationServerOptions
        {
            public AvaChemOAuthOptions()
            {
                AllowInsecureHttp = true;
                TokenEndpointPath = new PathString("/token");
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(43200);
                AccessTokenFormat = new AvaChemJwtFormat();
                Provider = new AvaChemOAuth();
            }
        }

        public class AvaChemOAuth : OAuthAuthorizationServerProvider
        {
            public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var identity = new ClaimsIdentity("otc");
                var username = context.OwinContext.Get<string>("otc:username");
                var sid = context.OwinContext.Get<string>("otc:id");
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim(ClaimTypes.UserData, sid));

                context.Validated(identity);
                return Task.FromResult(0);

            }
            public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                //to check database here
                try
                {
                    var username = context.Parameters["username"];
                    var password = context.Parameters["password"];
                    var pw = Utils.Hash(password);
                    User user = new UserDataConnector().GetUserByUsernameAndPassword(username, pw);

                    if (user != null)
                    {
                        context.OwinContext.Set("otc:username", user.Username);
                        context.OwinContext.Set("otc:id", user.ID.ToString());
                        context.Validated();
                    }
                    else
                    {
                        context.Rejected();
                    }
                }
                catch (Exception ex)
                {
                    context.SetError(ex.Message);
                    context.Rejected();
                }
                return Task.FromResult(0);
                //return base.ValidateClientAuthentication(context);

            }

        }

        public class AvaChemJwtFormat : ISecureDataFormat<AuthenticationTicket>
        {
            public string SignatureAlgorithm
            {
                get { return SecurityAlgorithms.HmacSha256Signature; }
            }
            public string DigestAlgorithm
            {
                get { return SecurityAlgorithms.HmacSha256; }
            }
            public string Protect(AuthenticationTicket data)
            {
                if (data == null) throw new ArgumentNullException("data");

                var issuer = ConfigurationManager.AppSettings["issuer"];
                var audience = ConfigurationManager.AppSettings["audience"];
                var key = Convert.FromBase64String(ConfigurationManager.AppSettings["JwtKey"]);
                var now = DateTime.Now;
                var expires = now.AddMinutes(43200);
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SignatureAlgorithm,
                    DigestAlgorithm
                    );

                var token = new JwtSecurityToken(issuer, audience, data.Identity.Claims, now, expires, signingCredentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            public AuthenticationTicket Unprotect(string protectedText)
            {
                throw new NotImplementedException();
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using FindMyHome.Domain;

//Rename to AuthConfig.cs before use.
//Insert your Facebook app id and secret.
namespace FindMyHome
{
    public static class AuthConfig_Template
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "Your app id here",
                appSecret: "Your app secret here");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}

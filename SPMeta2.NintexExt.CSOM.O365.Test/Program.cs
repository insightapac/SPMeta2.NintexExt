using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Services;
using SPMeta2.NintexExt.CSOM.O365.Services;
using SPMeta2.NintexExt.CSOM.O365.Test.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.CSOM.O365.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var targetSiteUrl = ConfigurationManager.AppSettings.Get("targetSiteUrl");
            using (ClientContext context = new ClientContext(targetSiteUrl))
            {
                //context.RequestTimeout = Timeout.Infinite;
                var username = ConfigurationManager.AppSettings.Get("sharePointUserName");
                var password = ConfigurationManager.AppSettings.Get("sharePointPassword");
                var securedPassword = new SecureString();
                foreach (var c in password.ToCharArray())
                {
                    securedPassword.AppendChar(c);
                }
                context.Credentials = new SharePointOnlineCredentials(username, securedPassword);

                NintexFormApiKeys.ApiKey = ConfigurationManager.AppSettings.Get("nintexApiKey");
                NintexFormApiKeys.WebServiceUrl = ConfigurationManager.AppSettings.Get("nintexServiceUrl");

                var service = new CSOMProvisionService();
                service.RegisterModelHandlers(typeof(NintexFormApiKeys).Assembly);
                WebModel.Provision(context, service);


            }

        }
    }
}

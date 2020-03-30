using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Services;
using SPMeta2.NintexExt.CSOM.O365.Handlers;
using SPMeta2.NintexExt.CSOM.O365.Services;
using SPMeta2.NintexExt.CSOM.O365.Test.Model;
using SPMeta2.NintexExt.Utils;
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
            CommandLineUtils.ApplyValuesFromcommandLine(args);
            var targetSiteUrl = ConfigurationManager.AppSettings.Get("targetSiteUrl");
            using (ClientContext context = new ClientContext(targetSiteUrl))
            {
                //context.RequestTimeout = Timeout.Infinite;
                var username = ConfigurationManager.AppSettings.Get("sharePointUserName");
                var password = ConfigurationManager.AppSettings.Get("sharePointPassword");
                var securedPassword = new SecureString();
                if (string.IsNullOrEmpty(password))
                {
                    securedPassword = GetConsoleSecurePassword();
                }
                else
                {
                    foreach (var c in password.ToCharArray())
                    {
                        securedPassword.AppendChar(c);
                    }
                }
                context.Credentials = new SharePointOnlineCredentials(username, securedPassword);

                NintexApiSettings.ApiKey = ConfigurationManager.AppSettings.Get("nintexApiKey");
                NintexApiSettings.WebServiceUrl = ConfigurationManager.AppSettings.Get("nintexServiceUrl");
                NintexApiSettings.HttpRequestTimeout = TimeSpan.Parse(ConfigurationManager.AppSettings["nintexApiTimeout"]);


                context.Load(context.Web, x=>x.Title, x=>x.Url);
                context.ExecuteQuery();
                Console.WriteLine("Provisioning to the site with");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\ttitle:url {0}:{1}", context.Web.Title, context.Web.Url);
                Console.ForegroundColor = ConsoleColor.White;

                var service = new CSOMProvisionService();
                service.RegisterModelHandlers(typeof(NintexFormO365Handler).Assembly);
                WebModel.PreProvision(context, service);
                Console.WriteLine("please ensure that the subweb NintexTest has the nintex apps installed and then press any key");
                WebModel.Provision(context, service);

            }

        }

        private static SecureString GetConsoleSecurePassword()
        {
            Console.WriteLine("Please enter the password");
            SecureString pwd = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    pwd.RemoveAt(pwd.Length - 1);
                    Console.Write("\b \b");
                }
                else
                {
                    pwd.AppendChar(i.KeyChar);
                    Console.Write("*");
                }
            }
            return pwd;
        }
    }
}

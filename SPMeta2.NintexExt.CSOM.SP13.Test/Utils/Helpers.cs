using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using MSDN.Samples.ClaimsAuth;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Support
{
    
    public class OutputMessage : IDisposable
    {
        ConsoleColor _parentColour = ConsoleColor.Yellow;
        ConsoleColor _childColour = ConsoleColor.Gray;
        ConsoleColor _originalColour;
        ConsoleColor _successColour = ConsoleColor.Green;
        int _level;

        public OutputMessage()
        {
            _level = 1;
        }

        public OutputMessage(string message, int level = 1)
        {
            _level = level;

            var colour = _level > 1 ? _childColour : _parentColour;

            WriteMessage(message, colour);
        }

        public OutputMessage(string message, ConsoleColor colour, int level = 1)
        {
            _level = level;

            WriteMessage(message, colour);
        }

        public static void Write(string message, ConsoleColor colour)
        {
            var originalColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.Write(message);
            Console.ForegroundColor = originalColour;
        }

        private void WriteMessage(string message, ConsoleColor colour)
        {
            _originalColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;

            int cursorPosition = 0;
            try
            {
                cursorPosition = Console.CursorLeft;
            }
            catch (Exception) { }

            if (_level > 1)
            {
                if (cursorPosition > 0)
                    Console.Write("\n"); //clear existing line

                for (int i = 1; i < _level; i++)
                {
                    Console.Write("  "); //indent
                }
                Console.Write("-"); //hyphonate
            }

            Console.Write(message + "...");

            Console.ForegroundColor = _originalColour;
        }

        public void Dispose()
        {
            int cursorPosition = 0;
            try
            {
                cursorPosition = Console.CursorLeft;
            }
            catch (Exception) { }

            if (_level == 1 && cursorPosition == 0)
            {
                Console.ForegroundColor = _parentColour;
                Console.Write("...");
                Console.ForegroundColor = _originalColour;
            }
            else if (_level == 2 && cursorPosition == 0)
            {
                Console.ForegroundColor = _childColour;
                Console.Write("  ...");
                Console.ForegroundColor = _originalColour;
            }

            Console.ForegroundColor = _successColour;
            Console.WriteLine("Done!");
            Console.ForegroundColor = _originalColour;
        }
    }
    
    public static class Helpers
    {

        public static ClientContext GetClientContext()
        {
            var useAdProxy = ConfigurationManager.AppSettings.Get("useADProxy");
            var targetSiteUrl = ConfigurationManager.AppSettings.Get("targetSiteUrl");
            ClientContext context = new ClientContext(targetSiteUrl);

            context.RequestTimeout = Timeout.Infinite;

            if (useAdProxy.ToLowerInvariant() == "true")
            {
                var claimse = new ClaimsWebAuth(targetSiteUrl, 800, 600);
                var cookieCollection = claimse.Show();

                Uri uriBase = new Uri(targetSiteUrl);
                Uri uri = new Uri(uriBase, "/");
                var cookies = cookieCollection.GetCookieHeader(uriBase);

                context.ExecutingWebRequest += (object sender, WebRequestEventArgs e) =>
                {
                    e.WebRequestExecutor.WebRequest.CookieContainer = cookieCollection;
                };

                return context;

            }
            else
            {
                var username = ConfigurationManager.AppSettings.Get("sharePointUserName");
                var password = ConfigurationManager.AppSettings.Get("sharePointPassword");
                var securedPassword = new SecureString();

                foreach (var c in password.ToCharArray())
                {
                    securedPassword.AppendChar(c);
                }

                //context.Credentials = new SharePointOnlineCredentials(username, securedPassword);
                context.Credentials = new NetworkCredential(username, password);

                return context;
            }

        }


        /// <summary>
        /// Checks an AppSettings flag to determine if the specified action should be run.
        /// </summary>
        /// <param name="configKey">The AppSettings configuration key</param>
        /// <param name="consoleMessage">The message to output to console</param>
        /// <param name="action">The delegate to run</param>
        /// <param name="level">Integer indicating the call hierarchy 'level' in the provisioning</param>
        public static void Execute(string configKey, string consoleMessage, Action action, int level = 1)
        {
            bool provisionFlag;

            try
            {
                provisionFlag = Boolean.Parse(ConfigurationManager.AppSettings[configKey]);
            }
            catch
            {
                provisionFlag = true;
            }

            if (provisionFlag)
            {
                using (new OutputMessage(consoleMessage, level))
                {
                    action();
                }
            }
        }

        public static string GetConfigValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch
            {
                return null;
            }
        }
        public static bool GetConfigValue(string key, bool defaultValue)
        {
            try
            {
                return Boolean.Parse(ConfigurationManager.AppSettings[key]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string CombineUrl(params string[] args)
        {
            return SPMeta2.Utils.UrlUtility.CombineUrl(args);
        }

        private static SecureString GetConsoleSecurePassword()
        {
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.Utils
{
    public class CommandLineUtils
    {
        private static ConsoleColor DefaultConsoleForegroundColor = Console.ForegroundColor;
        public static string GetConsoleSecurePassword()
        {
            StringBuilder pwd = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace && pwd.Length > 0)
                {
                    pwd.Remove(pwd.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else
                {
                    pwd.Append(i.KeyChar);
                    Console.Write("*");
                }
            }
            return pwd.ToString();
        }

        public static void ApplyValuesFromcommandLine(string[] args)
        {
            // process app config
            //read all values from the command line if possible and apply to the configuration dictionary
            Dictionary<string, string> configuration = new Dictionary<string, string>();
            foreach (var x in ConfigurationManager.AppSettings.AllKeys)
            {
                configuration[x] = ConfigurationManager.AppSettings[x];
            }

            foreach (var arg in args)
            {
                if (arg.Contains("="))
                {
                    var splittedArg = arg.Split('=');
                    if (splittedArg.Length >= 2)
                    {
                        configuration[splittedArg[0]] = splittedArg[1];
                        ConfigurationManager.AppSettings[splittedArg[0]] = splittedArg[1];
                    }
                }
            }
            //write back into appsettings for "compatibility" with some code that references the app settings
            foreach (var key in configuration.Keys.ToArray())
            {
                var value = configuration[key];
                if (value.ToLower() == "!read!")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Please enter value for {0}: ", key);
                    Console.ForegroundColor = DefaultConsoleForegroundColor;
                    value = Console.ReadLine();
                    configuration[key] = value;
                    //Console.WriteLine();
                }
                if (value.ToLower() == "!readpassword!")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Please enter value for {0}: ", key);
                    Console.ForegroundColor = DefaultConsoleForegroundColor;
                    value = GetConsoleSecurePassword();
                    configuration[key] = value;
                    Console.WriteLine();
                }
                ConfigurationManager.AppSettings[key] = value;
            }

            Console.WriteLine();
        }


    }
}

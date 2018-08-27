using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.IO;

namespace O365_RESTAPI_NF_Publish
{
    class Program
    {
        // The API key and root URL for the REST API.
        // TODO: Replace with your API key and root URL.
        static private string apiKey = "";
        static private string apiRootUrl = "";

        // The SharePoint site and credentials to use with the REST API.
        // TODO: Replace with your site URL, user name, and password.
        static private string spSiteUrl = "";
        static private string spUsername = "";
        static private string spPassword = "";

        // The list form to export, and the name of the destination list for which
        // the new form is to be imported.
        // TODO: Replace with your form identifier and list title.
        static private string listId = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Copy a list form from a source list to a destination list.
            PublishForm();
            Console.Write("Press any key to close window.");
            Console.ReadKey();
        }

        static private string GetSPOCookie()
        {
            // If successful, this variable contains an authentication cookie; 
            // otherwise, an empty string.
            string result = String.Empty;
            try
            {
                // Construct a secure string from the provided password.
                // NOTE: For sample purposes only.
                var securePassword = new SecureString();
                foreach (char c in spPassword) { securePassword.AppendChar(c); }

                // Instantiate a new SharePointOnlineCredentials object, using the 
                // specified username and password.
                var spoCredential = new SharePointOnlineCredentials(spUsername, securePassword);
                // If successful, try to authenticate the credentials for the
                // specified site.
                if (spoCredential == null)
                {
                    // Credentials could not be created.
                    result = String.Empty;
                }
                else
                {
                    // Credentials exist, so attempt to get the authentication cookie
                    // from the specified site.
                    result = spoCredential.GetAuthenticationCookie(new Uri(spSiteUrl));
                }
            }
            catch (Exception ex)
            {
                // An exception occurred while either creating the credentials or
                // getting an authentication cookie from the specified site.
                Console.WriteLine(ex.ToString());
                result = String.Empty;
            }

            // Return the result.
            return result;
        }

        static private void PublishForm()
        {
            // Create a new HTTP client and configure its base address.
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(spSiteUrl);

            // Add common request headers for the REST API to the HTTP client.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Api-Key", apiKey);

            // Get the SharePoint authorization cookie to be used by the HTTP client
            // for the request, and use it for the Authorization request header.
            string spoCookie = GetSPOCookie();
            if (spoCookie != String.Empty)
            {
                var authHeader = new AuthenticationHeaderValue(
                    "cookie",
                    String.Format("{0} {1}", spSiteUrl, spoCookie)
                );
                // Add the defined authentication header to the HTTP client's
                // default request headers.
                client.DefaultRequestHeaders.Authorization = authHeader;
            }
            else
            {
                throw new InvalidOperationException("Cannot define Authorization header for request.");
            }
            // Console test block
            var testURI = String.Format("{0}/api/v1/forms/{1}/publish",
                apiRootUrl.TrimEnd('/'),
                Uri.EscapeUriString(listId));
            Console.Write(testURI,"\n");

            // If we're at this point, we're ready to make our request.
            // Note that we're making this call synchronously - you can call the REST API
            // asynchronously, as needed.
            var publishFormUri = String.Format("{0}/api/v1/forms/{1}/publish",
                apiRootUrl.TrimEnd('/'),
                Uri.EscapeUriString(listId));
            HttpResponseMessage publishResponse = client.PostAsync(publishFormUri, new StringContent("")).Result;

            if (publishResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully published form.");
            }
            else
            {
                Console.WriteLine("Failed to publish form.");
            }
        }


    }
}

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

namespace NW2013O365_RESTAPI_ImportPost
{
    static class Program
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

        // The list workflow to export, and the name of the destination list for which
        // the new workflow is to be imported.
        // TODO: Replace with your workflow identifier and list title.
        static private string sourceWorkflowId = "";
        static private string destinationListTitle = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Copy a list workflow from a source list to a destination list.
            CopyWorkflowToList();
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

        static async private void CopyWorkflowToList()
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
                throw new InvalidOperationException("Cannot define Authentication header for request.");
            }

            // If we're at this point, we're ready to make our request.
            // Note that we're making this call synchronously - you can call the REST API
            // asynchronously, as needed.
            // First, we'll export the workflow from the source list.
            var exportWorkflowUri = String.Format("{0}/api/v1/workflows/packages/{1}",
                apiRootUrl.TrimEnd('/'),
                Uri.EscapeUriString(sourceWorkflowId));
            HttpResponseMessage exportResponse = client.GetAsync(exportWorkflowUri).Result;

            // If we're successful, import the exported workflow to the destination list, as a new workflow.
            if (exportResponse.IsSuccessStatusCode)
            {
                // The response body contains a Base64-encoded binary string, which we'll
                // asynchronously retrieve as a byte array.
                byte[] exportFileContent = await exportResponse.Content.ReadAsByteArrayAsync();

                // Next, import the exported workflow to the destination list.
                var importWorkflowUri = String.Format("{0}/api/v1/workflows/packages/?migrate=true&listTitle={1}",
                    apiRootUrl.TrimEnd('/'),
                    Uri.EscapeUriString(destinationListTitle));

                // Create a ByteArrayContent object to contain the byte array for the exported workflow.
                var importContent = new ByteArrayContent(exportFileContent);

                // Send a POST request to the REST resource.
                HttpResponseMessage importResponse = client.PostAsync(importWorkflowUri, importContent).Result;

                // Indicate to the console window the success or failure of the operation.
                if (importResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Successfully imported workflow.");
                }
                else
                {
                    Console.WriteLine("Failed to import workflow.");
                }
            }
        }
    }
}

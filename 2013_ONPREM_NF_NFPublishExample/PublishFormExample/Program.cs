/* Program.cs

   Copyright (c) 2014 - Nintex. All Rights Reserved.  
   This code released under the terms of the  
   Microsoft Reciprocal License (MS-RL,  http://opensource.org/licenses/MS-RL.html.)
   
*/

using System;
using System.IO;
using System.Net;
using System.Text;
using NintexFormsClient;

namespace ConsoleApplication1
{
    // TODO: Ensure that all variables have been set prior to running this sample. To quickly
    // locate variables that must be set, search for comments that start with TODO within this
    // file, and follow the instructions provided by each comment.

    /// <summary>
    /// This class demonstrates how to use the NintexFormsClient assembly to invoke the REST service operations 
    /// provided by Nintex Forms.
    /// </summary>
    class Program
    {
        // TODO: Set the NTLM credential information.
        // Ensure that the specified credentials have access to the SharePoint web URLs for both
        // the source and destination environments specified in the methods for this 
        private readonly static NetworkCredential ClientCredentials = new NetworkCredential("sp_admin", "***", "domain");

        // TODO: Set the SharePoint version.
        private const NintexFormsClient.Version ClientVersion = NintexFormsClient.Version.SharePoint2013;

        static void Main()
        {
            // Retrieve the form definition XML from the source environment, and then save
            // the XML to a file for later use.
            //GetFormFromSourceAndSaveToFile();

            // Retrieve the form definition XML from the source environment, and then publish 
            // the form to the destination environment.
            //GetFormFromSourceAndPublishToDestinationList();

            // Delete the form from the source environment.
            //DeleteFormFromSource();

            // Retrieve the form definition XML from a file, and then publish the form to 
            // the source environment.
            GetFormFromFileAndPublishToSourceList();

            // Pause until the user presses any key.
            Console.ReadLine();
        }

        private static void GetFormFromSourceAndSaveToFile()
        {
            // Configure the source environment.
            // TODO: Set the SharePoint web URL for the source environment.
            const string sourceUrl = "http://sp131:82/";

            // TODO: Set the content type ID of the content type for the source environment.
            // Use an empty string ("") to denote the default content type for the SharePoint list.
            const string sourceContentTypeId = "";

            // TODO: Set the list ID of the SharePoint list for the source environment.
            // Remember to enclose the GUID in curly brackets ({}).
            const string sourceListId = "{b0e621d9-3d9f-4f69-a8d3-e180adc083d3}";

            // Configure the file.
            // TODO: Set the full path and file name to the form definition XML file.
            const string filePath = @"d:\temp\form.xml";

            // Create a client context for the source environment.
            var sourceCtx = new NfClientContext(sourceUrl, ClientCredentials, ClientVersion);

            // Get the form from the source environment, and save it to the specified file.
            GetFormFromListAndSaveToFile(sourceCtx, sourceContentTypeId, sourceListId, filePath);
        }

        private static void GetFormFromSourceAndPublishToDestinationList()
        {
            // Configure the source environment.
            // TODO: Set the SharePoint web URL for the source environment.
            const string sourceUrl = "http://sp131:82/";

            // TODO: Set the content type ID of the content type for the source environment.
            // Use an empty string ("") to denote the default content type for the SharePoint list.
            const string sourceContentTypeId = "";

            // TODO: Set the list ID of the SharePoint list for the source environment.
            // Remember to enclose the GUID in curly brackets ({}).
            const string sourceListId = "{b0e621d9-3d9f-4f69-a8d3-e180adc083d3}";

            // Configure the destination environment.
            // TODO: Set the SharePoint web URL for the destination environment.
            const string destinationUrl = "http://sp131:82/";

            // TODO: Set the content type ID of the content type for the destination environment.
            // Use an empty string ("") to denote the default content type for the SharePoint list.
            const string destinationContentTypeId = "";

            // TODO: Set the list ID of the SharePoint list for the destination environment.
            // Remember to enclose the GUID in curly brackets ({}).
            const string destinationListId = "{b0e621d9-3d9f-4f69-a8d3-e180adc083d3}";

            // Create a client context for the source environment.
            var sourceCtx = new NfClientContext(sourceUrl, ClientCredentials, ClientVersion);

            // Create a client context for the destination environment.
            var destinationCtx = new NfClientContext(destinationUrl, ClientCredentials, ClientVersion);

            // Get the form from the source environment, and publish it to the destination environment.
            GetFormFromListAndPublishToList(sourceCtx, sourceContentTypeId, sourceListId, 
                destinationCtx, destinationContentTypeId, destinationListId);
        }

        private static void DeleteFormFromSource()
        {
            // Configure the source environment.
            // TODO: Set the SharePoint web URL for the source environment.
            const string sourceUrl = "http://source.com";

            // TODO: Set the content type ID of the content type for the source environment.
            // Use an empty string ("") to denote the default content type for the SharePoint list.
            const string sourceContentTypeId = "";

            // TODO: Set the list ID of the SharePoint list for the source environment.
            // Remember to enclose the GUID in curly brackets ({}).
            const string sourceListId = "{00000000-0000-0000-0000-000000000000}";

            // Create a client context for the source environment.
            var sourceCtx = new NfClientContext(sourceUrl, ClientCredentials, ClientVersion);

            // Delete the form from the source environment.
            DeleteFormFromList(sourceCtx, sourceContentTypeId, sourceListId);
        }


        private static void GetFormFromFileAndPublishToSourceList()
        {
            // Configure the file.
            // TODO: Set the full path and file name to the form definition XML file.
            const string filePath = @"d:\temp\form.xml";

            // Configure the source environment.
            // TODO: Set the SharePoint web URL for the source environment.
            const string sourceUrl = "http://sp131:82/";

            // TODO: Set the content type ID of the content type for the source environment.
            // Use an empty string ("") to denote the default content type for the SharePoint list.
            const string sourceContentTypeId = "";

            // TODO: Set the list ID of the SharePoint list for the source environment.
            // Remember to enclose the GUID in curly brackets ({}).
            const string sourceListId = "{b0e621d9-3d9f-4f69-a8d3-e180adc083d3}";

            // Create a client context for the source environment.
            var sourceCtx = new NfClientContext(sourceUrl, ClientCredentials, ClientVersion);

            // Get the form from the file, and publish it to the source environment.
            GetFormFromFileAndPublishToList(filePath, sourceCtx, sourceListId, sourceContentTypeId);
        }

        private static void GetFormFromListAndSaveToFile(NfClientContext sourceContext, string sourceContentTypeId, string sourceListId, string fullFilePath)
        {
            // Read the form definition XML by invoking the GetFormXml method
            // from the client context for the source environment.
            Console.WriteLine("Getting form...");
            string formXml = sourceContext.GetFormXml(sourceListId, sourceContentTypeId);

            Console.WriteLine("Saving form...");
            File.WriteAllText(fullFilePath, formXml, Encoding.Unicode);

            // Display the file name of the saved form to the console.
            Console.WriteLine("Successfully saved form to: {0}", fullFilePath);
        }

        private static void GetFormFromListAndPublishToList(NfClientContext sourceContext, string sourceContentTypeId, string sourceListId, NfClientContext destinationContext, string destinationContentTypeId, string destinationListId)
        {
            // Read the form definition XML by invoking the GetFormXml method
            // from the client context for the source environment.
            Console.WriteLine("Getting form...");
            string formXml = sourceContext.GetFormXml(sourceListId, sourceContentTypeId);

            // Publish the form definition XML by invoking the PublishForm method
            // from the client context for the destination environment.
            Console.WriteLine("Publishing form...");
            var result = destinationContext.PublishForm(destinationListId, destinationContentTypeId, formXml);
            
            // Display the version number of the published form to the console.
            Console.WriteLine("Successfully published version: {0}", result.Version);
        }

        private static void DeleteFormFromList(NfClientContext sourceContext, string sourceContentTypeId, string sourceListId)
        {
            // Delete the form by invoking the GetFormXml method
            // from the client context for the source environment.
            Console.WriteLine("Deleting form...");
            var result = sourceContext.DeleteForm(sourceListId, sourceContentTypeId);

            // Display the version number of the published form to the console.
            Console.WriteLine("Successfully deleted form: {0}", result);
        }

        private static void GetFormFromFileAndPublishToList(string fullFilePath, NfClientContext destinationCtx, string destinationListId, string destinationContentTypeId)
        {
            // Read the form definition XML by reading the contents of the
            // specified file.
            Console.WriteLine("Getting form...");
            string formXml = File.ReadAllText(fullFilePath, Encoding.Unicode);

            // Publish the form definition XML by invoking the PublishForm method
            // from the client context for the destination environment. In turn, the 
            // client context invokes the PublishFormXml REST operation.
            Console.WriteLine("Publishing form...");
            var result = destinationCtx.PublishForm(destinationListId, destinationContentTypeId, formXml);

            // Display the version number of the published form to the console.
            Console.WriteLine("Successfully published version: {0}", result.Version);
        }
    }
}

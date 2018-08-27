/* FormDigest2013.cs

   Copyright (c) 2014 - Nintex. All Rights Reserved.  
   This code released under the terms of the  
   Microsoft Reciprocal License (MS-RL,  http://opensource.org/licenses/MS-RL.html.)
   
*/

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace NintexFormsClient
{
    /// <summary>
    /// Represents the form digest for the Nintex Forms service endpoint on SharePoint 2013.
    /// </summary>
    class FormDigest2013 : FormDigest
    {
        /// <summary>
        /// Creates a new instance for the specified URL and credentials.
        /// </summary>
        /// <param name="webUrl">The URL of the SharePoint site.</param>
        /// <param name="credentials">The credentials to use.</param>
        /// <remarks>The class invokes the ContextInfo REST operation, 
        /// provided by the REST service from SharePoint 2013, 
        /// to retreive the appropriate form digest value.</remarks>
        public FormDigest2013(string webUrl, ICredentials credentials)
        {
            // Validate input.
            if (String.IsNullOrEmpty(webUrl)) return;

            // Create the request URL for the specified site URL.
            var restRequest = (HttpWebRequest)WebRequest.Create(new Uri(webUrl + "/_api/contextinfo"));

            // Configure the request as a REST request.
            restRequest.UseDefaultCredentials = false;
            restRequest.PreAuthenticate = true;
            restRequest.Credentials = credentials;
            restRequest.Method = "POST";
            restRequest.ContentLength = 0;

            // Send the request and parse the value of the form digest from the response.
            using (var restResponse = (HttpWebResponse)restRequest.GetResponse())
            {
                using (var respStream = restResponse.GetResponseStream())
                {
                    if (respStream == null) return;

                    var doc = XDocument.Parse(new StreamReader(respStream).ReadToEnd());
                    XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";

                    FormDigestValue = doc.Descendants(d + "FormDigestValue").First().Value;
                }
            }
        }
    }
}

/* IFormsWcfService.cs

   Copyright (c) 2014 - Nintex. All Rights Reserved.  
   This code released under the terms of the  
   Microsoft Reciprocal License (MS-RL,  http://opensource.org/licenses/MS-RL.html.)
   
*/

using System.ServiceModel;
using System.ServiceModel.Web;

namespace NintexFormsClient
{
    /// <summary>
    /// Describes the Nintex Forms service operations.
    /// </summary>
    [ServiceContract]
    public interface IFormsWcfService
    {
        /// <summary>
        /// Retrieves the XML form definition for the specified content type and SharePoint list.
        /// </summary>
        /// <param name="listId">The unique identifier for the SharePoint list.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        /// <returns>If successful, a string containing the XML form definition for the 
        /// specified content type and SharePoint list; otherwise, an empty string ("").</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, UriTemplate = "/_vti_bin/NintexFormsServices/NfRestService.svc/GetFormXml", ResponseFormat = WebMessageFormat.Xml)]
        string GetFormXml(string listId, string contentTypeId);

        /// <summary>
        /// Deletes the form for the specified content type and SharePoint list.
        /// </summary>
        /// <param name="listId">The unique identifier for the SharePoint list.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        /// <returns>If successful, a string containing the default view URL of the 
        /// specified SharePoint list; otherwise, an empty string ("").</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, UriTemplate = "/_vti_bin/NintexFormsServices/NfRestService.svc/DeleteForm", ResponseFormat = WebMessageFormat.Json)]
        string DeleteForm(string listId, string contentTypeId);
        
        /// <summary>
        /// Publishes the XML form definition for the specified content type and SharePoint list.
        /// </summary>
        /// <param name="listId">The unique identifier for the SharePoint list.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        /// <param name="formXml">The XML form definition to be published.</param>
        /// <returns>If successful, a <see cref="NintexFormsClient.FormSaveInfo" /> object 
        /// containing the URL and version for the published form; otherwise, a null reference.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, UriTemplate = "/_vti_bin/NintexFormsServices/NfRestService.svc/PublishFormXml", ResponseFormat = WebMessageFormat.Json)]
        FormSaveInfo PublishFormXml(string listId, string contentTypeId, string formXml);
    }
}

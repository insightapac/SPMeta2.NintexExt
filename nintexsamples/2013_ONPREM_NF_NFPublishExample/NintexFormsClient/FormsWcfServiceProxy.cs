/* FormsWcfServiceProxy.cs

   Copyright (c) 2014 - Nintex. All Rights Reserved.  
   This code released under the terms of the  
   Microsoft Reciprocal License (MS-RL,  http://opensource.org/licenses/MS-RL.html.)
   
*/

using System.ServiceModel;
using System.ServiceModel.Channels;

namespace NintexFormsClient
{
    /// <summary>
    /// The WCF service proxy for the Nintex Forms service endpoints.
    /// </summary>
    class FormsWcfServiceProxy : ClientBase<IFormsWcfService>, IFormsWcfService
    {
        public FormsWcfServiceProxy(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        /// <summary>
        /// Retrieves the XML form definition for the specified content type and SharePoint list.
        /// </summary>
        /// <param name="listId">The unique identifier for the SharePoint list.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        /// <returns>If successful, a string containing the XML form definition for the specified 
        /// content type and SharePoint list; otherwise, an empty string ("").</returns>
        public string GetFormXml(string listId, string contentTypeId)
        {
            return Channel.GetFormXml(listId, contentTypeId);
        }

        /// <summary>
        /// Deletes the form for the specified content type and SharePoint list.
        /// </summary>
        /// <param name="listId">The unique identifier for the SharePoint list.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        /// <returns>If successful, a string containing the default view URL of the specified 
        /// SharePoint list; otherwise, an empty string ("").</returns>
        public string DeleteForm(string listId, string contentTypeId)
        {
            return Channel.DeleteForm(listId, contentTypeId);
        }

        /// <summary>
        /// Publishes the XML form definition for the specified content type and SharePoint list.
        /// </summary>
        /// <param name="listId">The unique identifier for the SharePoint list.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        /// <param name="formXml">The XML form definition to be published.</param>
        /// <returns>If successful, a <see cref="NintexFormsClient.FormSaveInfo" /> object containing 
        /// the URL and version for the published form; otherwise, a null reference.</returns>
        public FormSaveInfo PublishFormXml(string listId, string contentTypeId, string formXml)
        {
            return Channel.PublishFormXml(listId, contentTypeId, formXml);
        }
    }
}

using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.NintexExt.Core.Definitions;
using SPMeta2.NintexExt.CSOM.O365.Services;
using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.CSOM.O365.Handlers
{
    public class NintexFormO365Handler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(NintexFormO365Definition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            NintexO365HandlerOnProvisionedEvent result = new NintexO365HandlerOnProvisionedEvent();
            // we need to have list id and the sharepoint authentication cookie
            NintexFormO365Definition formModel = (NintexFormO365Definition)model;
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(Object),
                ObjectDefinition = formModel,
                ModelHost = modelHost
            });
            base.DeployModel(modelHost, model);
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var web = listModelHost.HostWeb;
            var list = listModelHost.HostList;
            var clientContext = listModelHost.HostClientContext;
            string formDigestValue = clientContext.GetFormDigestDirect().DigestValue;

            var clientCredentials = clientContext.Credentials.WithAssertAndCast<SharePointOnlineCredentials>("sharepoint online credentials", value => value.RequireNotNull());
            var spSiteUrl = clientContext.Url;

            /// find the content type id or get the default one if not specified
            var listContentTypes = list.ContentTypes;
            clientContext.Load(listContentTypes);
            clientContext.ExecuteQueryWithTrace();
            var listContentTypesArray = listContentTypes.ToArray();
            var listContentType = listContentTypesArray[0];
            if (!string.IsNullOrEmpty(formModel.ListContentTypeNameOrId))
            {
                foreach (var x in listContentTypesArray)
                {
                    if (
                        (x.Name == formModel.ListContentTypeNameOrId) 
                            || 
                        (x.Id.StringValue.StartsWith(formModel.ListContentTypeNameOrId))
                    ){
                        listContentType = x;
                    }
                }
            }

            // Create a new HTTP client and configure its base address.
            HttpClient client = new HttpClient();
            HttpClientWrapper wrapper = new HttpClientWrapper(client);
            client.Timeout = NintexApiSettings.HttpRequestTimeout;
            client.BaseAddress = new Uri(spSiteUrl);
            // Add common request headers for the REST API to the HTTP client.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Api-Key", NintexApiSettings.ApiKey);

            string spoCookie = clientCredentials.GetAuthenticationCookie(new Uri(spSiteUrl));
            spoCookie.WithAssert("spoCookie", value => value.RequireStringNotOrEmpty());
            //var authHeader = new AuthenticationHeaderValue(
            //    "cookie",
            //    String.Format("{0} {1}", spSiteUrl, spoCookie)
            //);
            var authHeader = new AuthenticationHeaderValue(
                "cookie",
                $"{spSiteUrl} {spoCookie}"
            );
            // Add the defined Authorization header to the HTTP client's
            // default request headers.
            client.DefaultRequestHeaders.Authorization = authHeader;

            var importFormUri = String.Format("{0}/api/v1/forms/{1}",
                NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                Uri.EscapeUriString(list.Id.ToString()));

            if (!string.IsNullOrEmpty(formModel.ListContentTypeNameOrId))
            {
                importFormUri = String.Format("{0}/api/v1/forms/{1},{2}",
                    NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                    Uri.EscapeUriString(list.Id.ToString()),
                    listContentType.StringId);
            }

            HttpContent saveContent = new ByteArrayContent(formModel.FormData);
            result.saveResponse = wrapper.Put(importFormUri, saveContent);

            if (formModel.Publish || formModel.AssignedUseForProduction.HasValue)
            {
                //var publishFormUri = String.Format("{0}/api/v1/forms/{1}/publish",
                //    NintexFormApiKeys.WebServiceUrl.TrimEnd('/'),
                //    Uri.EscapeUriString(list.Id.ToString()));
                var publishFormUri = String.Format("{0}/api/v1/forms/{1},{2}/publish",
                    NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                    Uri.EscapeUriString(list.Id.ToString()),
                    listContentType.StringId);
                var content = "";
                //if (!string.IsNullOrEmpty(formModel.ListContentTypeNameOrId))
                //{
                //    content = string.Format(@"{{""contentTypeId"":""{0}"",""listId"":""{1}""}}",

                //        listContentType.StringId,
                //        list.Id.ToString("B").ToUpper());
                //}
                result.puiblishResponse = wrapper.Post(publishFormUri, new StringContent(content));
            }
            if (formModel.AssignedUseForProduction.HasValue)
            {
                var publishFormUri = String.Format("{0}/api/v1/forms/{1},{2}/assigneduse",
                    NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                    Uri.EscapeUriString(list.Id.ToString()),
                    listContentType.Id.ToString());
                var content = "";
                content = string.Format(@"{{""value"":""{0}""}}",
                    formModel.AssignedUseForProduction.Value ? "production" : "development");
                // interesting, this can return 405 and in details ()puiblishResponse.Content.ReadAsStringAsync()
                // in my case i had  a message saying "your license does not allow this" or something like this
                result.assignedUseForProductionValue = wrapper.Put(publishFormUri,
                    new StringContent(content, null, "application/json"));
            }



            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = result,
                ObjectType = typeof(NintexO365HandlerOnProvisionedEvent),
                ObjectDefinition = formModel,
                ModelHost = modelHost
            });

        }

    }
}

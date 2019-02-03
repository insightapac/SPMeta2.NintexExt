using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using SPMeta2.Common;
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
    public abstract class NintexWorkflow365HandlerBase : CSOMModelHandlerBase
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            NintexFormO365HandlerOnProvisionedEvent result = new NintexFormO365HandlerOnProvisionedEvent();
            NintexWorkflowO365DefinitionBase workflowModel = (NintexWorkflowO365DefinitionBase)model;
            //TODO: add some specifics?
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(Object),
                ObjectDefinition = workflowModel,
                ModelHost = modelHost
            });
            base.DeployModel(modelHost, model);
            var listModelHost = modelHost as ListModelHost;
            var baseModelHost = modelHost as CSOMModelHostBase;

            var web = baseModelHost.HostWeb;
            var list = listModelHost != null ? listModelHost.HostList : null;
            var clientContext = baseModelHost.HostClientContext;
            string formDigestValue = clientContext.GetFormDigestDirect().DigestValue;

            var clientCredentials = clientContext.Credentials.WithAssertAndCast<SharePointOnlineCredentials>("sharepoint online credentials", value => value.RequireNotNull());
            var spSiteUrl = clientContext.Url;

            // Create a new HTTP client and configure its base address.
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(spSiteUrl);
            // Add common request headers for the REST API to the HTTP client.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Api-Key", NintexFormApiKeys.ApiKey);

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



            var getFormUri1 = String.Format("{0}/api/v1/workflows",
                NintexFormApiKeys.WebServiceUrl.TrimEnd('/'));
            var getResult1 = client.GetAsync(getFormUri1).Result;
            var getResult1String = getResult1.Content.ReadAsStringAsync().Result;
            var parsedData = JObject.Parse(getResult1String);

            var workflowId = "";

            // trying to find id by name
            if (list!=null)
            {
                workflowId = (from d in (parsedData["data"] as JArray)
                              where (d["workflowType"].Value<string>() == "List"
                                   && d["name"].Value<string>() == workflowModel.Name
                                   && d["listId"].Value<string>() == list.Id.ToString()
                                   )
                              select d["id"].Value<string>()).FirstOrDefault();
            }
            else
            {
                workflowId = (from d in (parsedData["data"] as JArray)
                                       where (d["workflowType"].Value<string>() == "Site" 
                                            && d["name"].Value<string>()==workflowModel.Name)
                                       select d["id"].Value<string>()).FirstOrDefault();
            }


            if (string.IsNullOrEmpty(workflowId))
            {
                var importFormUri = String.Format("{0}/api/v1/workflows/packages/?migrate=true",
                    NintexFormApiKeys.WebServiceUrl.TrimEnd('/'));

                if (list != null)
                {
                    importFormUri = String.Format("{0}/api/v1/workflows/packages/?migrate=true&listTitle={1}",
                        NintexFormApiKeys.WebServiceUrl.TrimEnd('/'),
                        Uri.EscapeUriString(list.Title.ToString())
                        );
                }
                HttpContent saveContent = new ByteArrayContent(workflowModel.WorkflowData);
                result.saveResponse = client.PostAsync(importFormUri, saveContent).Result;
            }
            else
            {

                var importFormUri = String.Format("{0}/api/v1/workflows/packages/{1}/?migrate=true",
                    NintexFormApiKeys.WebServiceUrl.TrimEnd('/'),
                    Uri.EscapeUriString(workflowId));
                if (list != null)
                {
                    importFormUri = String.Format("{0}/api/v1/workflows/packages/{1}/?migrate=true&listTitle={2}",
                        NintexFormApiKeys.WebServiceUrl.TrimEnd('/'),
                        Uri.EscapeUriString(workflowId),
                        Uri.EscapeUriString(list.Title.ToString())
                        );
                }
                HttpContent saveContent = new ByteArrayContent(workflowModel.WorkflowData);
                result.saveResponse = client.PutAsync(importFormUri, saveContent).Result;
            }

            //TODO: assigned use for production
        }
    }
}

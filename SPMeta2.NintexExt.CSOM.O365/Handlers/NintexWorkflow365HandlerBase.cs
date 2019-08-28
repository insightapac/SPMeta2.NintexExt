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
using System.Net;
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
            NintexO365HandlerOnProvisionedEvent result = new NintexO365HandlerOnProvisionedEvent();
            NintexWorkflowO365DefinitionBase workflowModel = (NintexWorkflowO365DefinitionBase)model;
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



            var getFormUri1 = String.Format("{0}/api/v1/workflows",
                NintexApiSettings.WebServiceUrl.TrimEnd('/'));
            var getResult1 = wrapper.Get(getFormUri1);
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
                    NintexApiSettings.WebServiceUrl.TrimEnd('/'));

                if (list != null)
                {
                    importFormUri = String.Format("{0}/api/v1/workflows/packages/?migrate=true&listTitle={1}",
                        NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                        Uri.EscapeUriString(list.Title.ToString())
                        );
                }
                HttpContent saveContent = new ByteArrayContent(workflowModel.WorkflowData);
                result.saveResponse = wrapper.Post(importFormUri, saveContent);

                var saveresult = result.saveResponse.Content.ReadAsStringAsync().Result;
                var parsedSavedData = JObject.Parse(saveresult);
                workflowId = parsedSavedData["id"].Value<string>();
            }
            else
            {

                var importFormUri = String.Format("{0}/api/v1/workflows/packages/{1}/?migrate=true",
                    NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                    Uri.EscapeUriString(workflowId));
                if (list != null)
                {
                    importFormUri = String.Format("{0}/api/v1/workflows/packages/{1}/?migrate=true&listTitle={2}",
                        NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                        Uri.EscapeUriString(workflowId),
                        Uri.EscapeUriString(list.Title.ToString())
                        );
                }
                HttpContent saveContent = new ByteArrayContent(workflowModel.WorkflowData);
                result.saveResponse = wrapper.Put(importFormUri, saveContent);
            }


            if (workflowModel.AssignedUseForProduction.HasValue)
            {
                var publishFormUri = String.Format("{0}/api/v1/workflows/{1}/assigneduse",
                    NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                    Uri.EscapeUriString(workflowId));
                var content = "";
                var asssignedUseStringValue = workflowModel.AssignedUseForProduction.Value ? "production" : "development";
                content = string.Format(@"{{""value"":""{0}""}}",asssignedUseStringValue);
                // interesting, this can return 405 and in details ()puiblishResponse.Content.ReadAsStringAsync()
                // in my case i had  a message saying "your license does not allow this" or something like this
                result.assignedUseForProductionValue = wrapper.Put(publishFormUri,
                    new StringContent(content, null, "application/json"), 
                    (obj,args)=> {
                        if (NintexApiSettings.SemiSuccessFullPublishHttpErrorCodes.Contains(args.Message.StatusCode))
                        {

                            getFormUri1 = String.Format("{0}/api/v1/workflows",
                                NintexApiSettings.WebServiceUrl.TrimEnd('/'));
                            getResult1 = wrapper.Get(getFormUri1);
                            getResult1String = getResult1.Content.ReadAsStringAsync().Result;
                            parsedData = JObject.Parse(getResult1String);

                            var assignedUseResult  = (from d in (parsedData["data"] as JArray)
                                          where (d["id"].Value<string>() == workflowId)
                                          select d["assignedUse"].Value<string>()).FirstOrDefault();
                            if (asssignedUseStringValue.ToLower() == (assignedUseResult??"").ToLower())
                            {
                                args.Message.StatusCode = (HttpStatusCode)299;
                                args.StopProcessing = true;
                            }

                        }

                    });
            }
            if (workflowModel.Publish)
            {
                var publishFormUri = String.Format("{0}/api/v1/workflows/{1}/published",
                    NintexApiSettings.WebServiceUrl.TrimEnd('/'),
                    Uri.EscapeUriString(workflowId));
                var content = "";
                result.puiblishResponse = wrapper.Post(publishFormUri, new StringContent(content),
                    (obj, args)=> {
                    if (NintexApiSettings.SemiSuccessFullPublishHttpErrorCodes.Contains(args.Message.StatusCode))
                    {

                        getFormUri1 = String.Format("{0}/api/v1/workflows",
                            NintexApiSettings.WebServiceUrl.TrimEnd('/'));
                        getResult1 = wrapper.Get(getFormUri1);
                        getResult1String = getResult1.Content.ReadAsStringAsync().Result;
                        parsedData = JObject.Parse(getResult1String);

                        var published = (from d in (parsedData["data"] as JArray)
                                                 where (d["id"].Value<string>() == workflowId)
                                                 select d["isPublished"].Value<string>()).FirstOrDefault();
                        if ((published??"").ToLower() == "true")
                        {
                            args.Message.StatusCode = (HttpStatusCode)299;
                            args.StopProcessing = true;
                        }

                    }

                });
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = result,
                ObjectType = typeof(NintexO365HandlerOnProvisionedEvent),
                ObjectDefinition = workflowModel,
                ModelHost = modelHost
            });


        }
    }
}

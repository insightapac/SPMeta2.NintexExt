using Microsoft.SharePoint.Client;
using SPMeta2.NintexExt.Core.Definitions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.CSOM.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization.Json;
using SPMeta2.Common;

namespace SPMeta2.NintexExt.CSOM.SP13.Handlers
{
    public class NintexFormHandler : CSOMModelHandlerBase
    {
        [DataContract]
        public class NintexFormSerialize

        {
            [DataMember]
            public string listId { get; set; }
            [DataMember]
            public string contentTypeId { get; set; }
            [DataMember]
            public string formXml { get; set; }

            public static NintexFormSerialize FromDefinition(NintexFormDefinition def, ClientContext ctx, Web web, List list)
            {
                var result = new NintexFormSerialize();
                result.listId = list.Id.ToString("B");
                result.contentTypeId = def.ListContentTypeNameOrId;
                result.formXml = def.FormXml;

                return result;
            }
        }

        public override Type TargetType
        {
            get { return typeof(NintexFormDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            NintexFormDefinition formModel = (NintexFormDefinition)model;
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

            //https://help.nintex.com/en-US/sdks/sdk2013/FormSDK/Topics/SDK_NF_API_OPS__2013_WWSvcOps.htm?tocpath=Nintex%20Software%20Development%20Kit%7CNintex%20Forms%202013%20Software%20Development%20Kit%7CNintex%20Forms%20SDK%20Samples%7CMigrate%20Forms%20with%20the%20Nintex%20Forms%20API%7CInvoking%20methods%20from%20the%20REST%20API%7C_____2
            //https://help.nintex.com/en-US/sdks/sdk2013/#FormSDK/Topics/SDK_NF_API_REF_PublishFormXml.htm%3FTocPath%3DNintex%2520Software%2520Development%2520Kit%7CNintex%2520Forms%25202013%2520Software%2520Development%2520Kit%7CNintex%2520Forms%25202013%2520SDK%2520Reference%7CWeb%2520Services%2520Reference%7C_____4

            base.DeployModel(modelHost, model);

            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var web = listModelHost.HostWeb;
            var list = listModelHost.HostList;
            var clientContext = listModelHost.HostClientContext;
            string formDigestValue = clientContext.GetFormDigestDirect().DigestValue;

            var publishUrl = UrlUtility.CombineUrl(clientContext.Url, "/_vti_bin/NintexFormsServices/NfRestService.svc/PublishFormXml");

            var executor = clientContext.WebRequestExecutorFactory.CreateWebRequestExecutor(clientContext, publishUrl);
            executor.RequestContentType = "application/json";
            executor.RequestContentType = "application/json; charset=utf-8";

            executor.RequestHeaders.Add("X-RequestDigest", formDigestValue);
            //executor.RequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            //executor.RequestHeaders.Add(HttpRequestHeader.ContentEncoding, "utf-8");
            executor.RequestMethod = "POST";

            var serializedObject = NintexFormSerialize.FromDefinition(formModel, clientContext, web, list);

            var requestStream = executor.GetRequestStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NintexFormSerialize));
            ser.WriteObject(requestStream, serializedObject);
            requestStream.Close();
            //TODO:
            // instead if using requestor.execute, run the following
            // ClientRuntimeContext.SetupRequestCredential(m_context, m_webRequest);
            //  this has to be done via reflection  context.FireExecutingWebRequestEvent(new WebRequestEventArgs(webrequestexecutor));
            executor.Execute();
            string result = "";

            using (StreamReader sr = new StreamReader(executor.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = result,
                ObjectType = typeof(string),
                ObjectDefinition = formModel,
                ModelHost = modelHost
            });

        }
    }
}

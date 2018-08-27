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
using SPMeta.NintexExt.CSOM.SP13.NintexWorkflowWS;

namespace SPMeta2.NintexExt.CSOM.SP13.Handlers
{
    public class NintexListWorkflowHandler : CSOMModelHandlerBase
    {
        [DataContract]
        public class NintexListWorkflowSerialize

        {
            [DataMember]
            public string listId { get; set; }

            [DataMember]
            public string WorkflowXml { get; set; }

            public static NintexListWorkflowSerialize FromDefinition(NintexListWorkflowDefinition def, ClientContext ctx, Web web, List list)
            {
                var result = new NintexListWorkflowSerialize();
                result.listId = list.Id.ToString("B");
                result.WorkflowXml = def.WorkflowXml;

                return result;
            }
        }

        public override Type TargetType
        {
            get { return typeof(NintexListWorkflowDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            NintexListWorkflowDefinition WorkflowModel = (NintexListWorkflowDefinition)model;
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(Object),
                ObjectDefinition = WorkflowModel,
                ModelHost = modelHost
            });

            // https://help.nintex.com/en-US/sdks/sdk2013/#Walkthroughs/WK_Prc_NWF2013_exportandpublishaworkflow.htm%3FTocPath%3DNintex%2520Software%2520Development%2520Kit%7CModifying%2520and%2520Extending%2520Nintex%2520Workflow%2520and%2520Forms%7C_____5


            base.DeployModel(modelHost, model);

            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var web = listModelHost.HostWeb;
            var list = listModelHost.HostList;
            var clientContext = listModelHost.HostClientContext;
            string FormDigestValue = clientContext.GetFormDigestDirect().DigestValue;

            var publishUrl = UrlUtility.CombineUrl(clientContext.Url, "/_vti_bin/NintexWorkflow/Workflow.asmx");

            NintexWorkflowWSSoapClient soapClient = new NintexWorkflowWSSoapClient("NintexWorkflowWSSoap"); //need to name the endpoint being used.

            soapClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(publishUrl));
            soapClient.Endpoint.ListenUri = new Uri(publishUrl);

            soapClient.ClientCredentials.Windows.ClientCredential = clientContext.Credentials.GetCredential(new Uri(publishUrl), "");
            soapClient.ClientCredentials.Windows.AllowNtlm = true; // optional - NOTE - this was copied from Nintex SDK, it's not clear if this is required or helpful
            soapClient.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation; //optional - NOTE - this was copied from Nintex SDK, it's not clear if this is required or helpful

            byte[] workflowAsByteArray = System.Text.UTF8Encoding.UTF8.GetBytes(WorkflowModel.WorkflowXml);
            // System.Convert.ToBase64String(workflowAsByteArray);

            var result = soapClient.PublishFromNWF(workflowAsByteArray, list.Title, WorkflowModel.Name, true);




            //var executor = clientContext.WebRequestExecutorFactory.CreateWebRequestExecutor(clientContext, publishUrl);
            //executor.RequestContentType = "application/json";
            //executor.RequestContentType = "application/json; charset=utf-8";

            //executor.RequestHeaders.Add("X-RequestDigest", FormDigestValue);
            ////executor.RequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            ////executor.RequestHeaders.Add(HttpRequestHeader.ContentEncoding, "utf-8");
            //executor.RequestMethod = "POST";

            //var serializedObject = NintexListWorkflowSerialize.FromDefinition(WorkflowModel, clientContext, web, list);

            //var requestStream = executor.GetRequestStream();
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NintexListWorkflowSerialize));
            //ser.WriteObject(requestStream, serializedObject);
            //requestStream.Close();
            //executor.Execute();
            //string result = "";

            //using (StreamReader sr = new StreamReader(executor.GetResponseStream()))
            //{
            //    result = sr.ReadToEnd();
            //}

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = result,
                ObjectType = typeof(string),
                ObjectDefinition = WorkflowModel,
                ModelHost = modelHost
            });

        }
    }
}

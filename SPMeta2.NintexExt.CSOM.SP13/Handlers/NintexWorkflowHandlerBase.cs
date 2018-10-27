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
using System.ServiceModel;

namespace SPMeta2.NintexExt.CSOM.SP13.Handlers
{
    public abstract class NintexWorkflowHandlerBase : CSOMModelHandlerBase
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

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            NintexWorkflowDefinition WorkflowModel = (NintexWorkflowDefinition)model;
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

            var listModelHost = modelHost as ListModelHost;
            var baseModelHost = modelHost as CSOMModelHostBase;

            var web = baseModelHost.HostWeb;
            var list = listModelHost!=null? listModelHost.HostList : null;
            var clientContext = baseModelHost.HostClientContext;
            string FormDigestValue = clientContext.GetFormDigestDirect().DigestValue;

            var publishUrl = UrlUtility.CombineUrl(web.Url, "/_vti_bin/NintexWorkflow/Workflow.asmx");

            // Create the binding as per the settings specified in the Nintex SDK

            BasicHttpBinding binding = new BasicHttpBinding
            {               

                Name = "NintexWorkflowWSSoap",
                CloseTimeout = TimeSpan.FromMinutes(1),
                OpenTimeout = TimeSpan.FromMinutes(1),
                ReceiveTimeout = TimeSpan.FromMinutes(10),
                SendTimeout = TimeSpan.FromMinutes(1),
                AllowCookies = false,
                BypassProxyOnLocal = false,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                MaxBufferSize = 100000000,
                MaxBufferPoolSize = 100000000,
                MaxReceivedMessageSize = 100000000,
                MessageEncoding = WSMessageEncoding.Text,
                TextEncoding = System.Text.Encoding.UTF8,
                TransferMode = TransferMode.Buffered,
                UseDefaultWebProxy = true,       
            };

            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 999999999;
            binding.ReaderQuotas.MaxArrayLength = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;

            if (web.Url.ToLowerInvariant().StartsWith("https://"))
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
            }
            else
            {
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            }

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm; // TODO - is this unnecessarily constraining?
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = "";

            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;

            var endpoint = new EndpointAddress(publishUrl);

            NintexWorkflowWSSoapClient soapClient = new NintexWorkflowWSSoapClient(binding, endpoint);

            soapClient.ClientCredentials.Windows.ClientCredential = clientContext.Credentials.GetCredential(new Uri(publishUrl), "");
            soapClient.ClientCredentials.Windows.AllowNtlm = true; // optional - NOTE - this was copied from Nintex SDK, it's not clear if this is required or helpful
            soapClient.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation; //optional - NOTE - this was copied from Nintex SDK, it's not clear if this is required or helpful

            //byte[] workflowAsByteArray = System.Text.UTF8Encoding.UTF8.GetBytes(WorkflowModel.WorkflowXml);
            
            // System.Convert.ToBase64String(workflowAsByteArray); // This line was in the Nintex SDK sample but appears to break the process

            var result = soapClient.PublishFromNWFXml(WorkflowModel.WorkflowXml, list!=null? list.Title : null, WorkflowModel.Name, true);

            // TODO - improve the method of constructing the web service call, similar to the below

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

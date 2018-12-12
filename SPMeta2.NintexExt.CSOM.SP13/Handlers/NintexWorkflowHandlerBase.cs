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
using System.Security;
using SPMeta2.Common;
using SPMeta.NintexExt.CSOM.SP13.NintexWorkflowWS;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace SPMeta2.NintexExt.CSOM.SP13.Handlers
{
    public abstract class NintexWorkflowHandlerBase : CSOMModelHandlerBase
    {
        private const string SoapPublishFromNWFXmlRequstTemplate = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                            <PublishFromNWFXml xmlns=""http://nintex.com"">
                                <workflowFile>{0}</workflowFile>
                                <listName>{1}</listName>
                                <workflowName>{2}</workflowName>
                                <saveIfCannotPublish>{3}</saveIfCannotPublish>
                            </PublishFromNWFXml>
                        </s:Body>
                    </s:Envelope>";

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
            var list = listModelHost != null ? listModelHost.HostList : null;
            var clientContext = baseModelHost.HostClientContext;

            var formDigestValue = clientContext.GetFormDigestDirect().DigestValue;
            var publishUrl = UrlUtility.CombineUrl(web.Url, "/_vti_bin/NintexWorkflow/Workflow.asmx");

            var executor = clientContext.WebRequestExecutorFactory.CreateWebRequestExecutor(clientContext, publishUrl);
            executor.RequestMethod = "POST";
            executor.RequestContentType = "text/xml; charset=utf-8";
            executor.RequestHeaders.Add("X-RequestDigest", formDigestValue);
            executor.RequestHeaders.Add("SOAPAction", "\"http://nintex.com/PublishFromNWFXml\"");
            executor.RequestHeaders.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            executor.WebRequest.AutomaticDecompression = DecompressionMethods.GZip;

            var requestStream = executor.GetRequestStream();
            using (var streamWriter = new StreamWriter(requestStream, Encoding.UTF8))
            {
                var formatedString = BuildSoapRequest(list, WorkflowModel);
                streamWriter.Write(formatedString);
            }
            requestStream.Close();

            executor.Execute();

            var result = ReadSoapResponse(executor);

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

        private string BuildSoapRequest(List list, NintexWorkflowDefinition workflowModel)
        {
            //var encodedXml = workflowModel.WorkflowXml.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
            var encodedXml = System.Net.WebUtility.HtmlEncode(workflowModel.WorkflowXml);
            var listTitle = list != null ? list.Title : String.Empty;
            var workflowName = workflowModel.Name;
            var saveIfCannotPublish = Boolean.TrueString.ToLower();
            var request = String.Format(SoapPublishFromNWFXmlRequstTemplate, encodedXml, listTitle, workflowName, saveIfCannotPublish);
            return request;
        }

        private string ReadSoapResponse(WebRequestExecutor requestExecutor)
        {
            string soapResponse = "";
            using (StreamReader sr = new StreamReader(requestExecutor.GetResponseStream()))
            {
                soapResponse = sr.ReadToEnd();
            }
            XmlDocument document = new XmlDocument();
            document.LoadXml(soapResponse);
            XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
            manager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            manager.AddNamespace("ns", "http://nintex.com");
            var response = document.SelectSingleNode("*/soap:Body/*/ns:PublishFromNWFXmlResult", manager);
            return response != null ? response.InnerText : soapResponse;
        }
    }
}


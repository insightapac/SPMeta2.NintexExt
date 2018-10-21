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
    public class NintexSiteWorkflowHandlerBase : NintexWorkflowHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(NintexWebWorkflowDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);
        }
    }
}

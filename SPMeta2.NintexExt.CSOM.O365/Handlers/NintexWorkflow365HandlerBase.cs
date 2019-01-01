using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.NintexExt.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
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


        }
    }
}

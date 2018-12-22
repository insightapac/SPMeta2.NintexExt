using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.CSOM.O365.Handlers
{
    [Obsolete("not implemented yet",true)]
    public abstract class NintexWorkflow365HandlerBase : CSOMModelHandlerBase
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);
        }
    }
}

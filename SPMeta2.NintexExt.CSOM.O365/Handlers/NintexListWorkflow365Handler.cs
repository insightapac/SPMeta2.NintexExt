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
    [Obsolete("not implemented yet",true)]
    public class NintexListWorkflow365Handler : NintexWorkflow365HandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(NintexListWorkflowO365Definition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);
        }
    }
}

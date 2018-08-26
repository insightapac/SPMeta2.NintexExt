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
    public class NintexFormO365Handler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(NintexFormO365Definition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
        }

    }
}

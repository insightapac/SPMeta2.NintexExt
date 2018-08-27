using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.Core.Syntax.Default
{
    public static class NintexListWorkflowSyntax
    {
        public static TModelNode AddNintexWorkflow<TModelNode>(this TModelNode model, Definitions.NintexListWorkflowDefinition definition, 
            Action<ModelNode> action = null) where TModelNode : ModelNode, IContentTypeLinkHostModelNode, new()
        {
            model.AddDefinitionNode(definition, action);
            return model;
        }
    }
}

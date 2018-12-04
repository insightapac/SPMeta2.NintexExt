using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.Core.Syntax.Default
{
    public static class NintexWorkflowSyntaxO365
    {
        public static TModelNode AddNintexWorkflowO365<TModelNode>(this TModelNode model, Definitions.NintexWebWorkflowO365Definition definition,
            Action<ModelNode> action = null) where TModelNode : WebModelNode, IContentTypeLinkHostModelNode, new()
        {
            model.AddDefinitionNode(definition, action);
            return model;
        }
        public static TModelNode AddNintexWorkflowO365<TModelNode>(this TModelNode model, Definitions.NintexListWorkflowO365Definition definition,
            Action<ModelNode> action = null) where TModelNode : ListModelNode, IContentTypeLinkHostModelNode, new()
        {
            model.AddDefinitionNode(definition, action);
            return model;
        }
    }
}

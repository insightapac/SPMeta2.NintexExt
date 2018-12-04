using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.Core.Syntax.Default
{
    public static class NintexFormSyntax
    {
        public static TModelNode AddNintexForm<TModelNode>(this TModelNode model, Definitions.NintexFormDefinition definition, 
            Action<ModelNode> action = null) where TModelNode : ListModelNode, IContentTypeLinkHostModelNode, new()
        {
            model.AddDefinitionNode(definition, action);
            return model;
        }
    }
}

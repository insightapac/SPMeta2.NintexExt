using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.NintexExt.Core.Definitions
{
    public class NintexListWorkflowDefinition : DefinitionBase
    {

        /// <summary>
        /// The display name of the workflow.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        [ExpectRequired]
        public string Name { get; set; }


        /// <summary>
        /// The binary content of the workflow.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        [ExpectRequired]
        public string WorkflowXml { get; set; }

    }
}

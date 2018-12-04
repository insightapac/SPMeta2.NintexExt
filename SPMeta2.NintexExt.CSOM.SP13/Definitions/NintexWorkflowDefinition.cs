using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;

namespace SPMeta2.NintexExt.Core.Definitions
{
    public abstract class NintexWorkflowDefinition : DefinitionBase
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
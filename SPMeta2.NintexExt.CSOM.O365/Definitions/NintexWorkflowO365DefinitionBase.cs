using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.NintexExt.Core.Definitions
{
    public abstract class NintexWorkflowO365DefinitionBase : DefinitionBase
    {

        /// <summary>
        /// The display name of the workflow.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        [ExpectRequired]
        public string Name { get; set; }

        ///// <summary>
        ///// The if of the workflow that can be read from the export file.
        ///// </summary>
        //[System.Runtime.Serialization.DataMemberAttribute]
        //[ExpectUpdate]
        //[ExpectValidation]
        //[ExpectRequired]
        //public string WorkflowId { get; set; }
        
        /// <summary>
        /// The binary content of the workflow.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        [ExpectRequired]
        public byte[] WorkflowData { get; set; }

        /// <summary>
        /// If the workflow should be published
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#Reference/REST/NWO_REF_REST_PublishWorkflow.htm%3FTocPath%3DNintex%2520Office%2520365%2520API%7CNintex%2520Workflow%2520for%2520Office%2520365%2520REST%2520API%7CAPI%2520Reference%7CREST%2520Resources%7CWorkflows%7C_____3"/>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        public bool Publish { get; set; }

        /// <summary>
        /// True will set it to production, false to development, null will do nothing
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#Reference/REST/NWO_REF_REST_AssignedUse.htm%3FTocPath%3DNintex%2520Office%2520365%2520API%7CNintex%2520Workflow%2520for%2520Office%2520365%2520REST%2520API%7CAPI%2520Reference%7CREST%2520Resources%7CWorkflows%7C_____1"/>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        public bool? AssignedUseForProduction { get; set; }
    }
}

using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.NintexExt.Core.Definitions
{
    public class NintexFormDefinition : DefinitionBase
    {
        /// <summary>
        /// List content type name
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectRequired]
        [ExpectUpdate]
        [ExpectValidation]
        public virtual string ListContentTypeNameOrId { get; set; }

        /// <summary>
        /// The string content of the form, this format is used for onprem forms.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        [ExpectRequired]
        public string FormXml { get; set; }

    }
}

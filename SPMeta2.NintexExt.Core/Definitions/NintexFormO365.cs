using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.NintexExt.Core.Definitions
{
    public class NintexFormO365Definition : DefinitionBase
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
        /// The binary content of the form.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        [ExpectRequired]
        public byte[] FormData { get; set; }

    }
}

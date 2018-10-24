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

        /// <summary>
        /// If the form should be published
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/NF_SC002_REF_API_R_PublishForm.htm%3FTocPath%3DNintex%2520Office%2520365%2520API%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%2520API%7CAPI%2520Reference%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%25C2%25A0API%25C2%25A0Resources%7C_____5"/>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        public bool Publish { get; set; }

        /// <summary>
        /// True will set it to production, false to development, null will do nothing
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/NF_SC002_REF_API_R_AssignedUse.htm%3FTocPath%3DNintex%2520Office%2520365%2520API%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%2520API%7CAPI%2520Reference%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%25C2%25A0API%25C2%25A0Resources%7C_____1"/>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        public bool? AssignedUseForProduction { get; set; }
    }
}

using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.Core.Definitions
{
    [Obsolete]
    class NintexO365ApiKeys : DefinitionBase
    {
        /// <summary>
        /// The api key for the nintex operations
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/SDK_NFO_PRC__REST_QSG.htm#Get_your_API_key"/>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectRequired]
        [ExpectUpdate]
        [ExpectValidation]
        public virtual string ApiKey { get; set; }

        /// <summary>
        /// The api key for the nintex operations
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/SDK_NFO_PRC__REST_QSG.htm#Get_your_API_key"/>
        [System.Runtime.Serialization.DataMemberAttribute]
        [ExpectUpdate]
        [ExpectValidation]
        public virtual string WebServiceUrl { get; set; }
        //TODO: calculate the url if not specified
    }
}

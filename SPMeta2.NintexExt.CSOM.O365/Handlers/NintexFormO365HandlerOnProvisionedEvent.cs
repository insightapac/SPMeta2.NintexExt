using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.CSOM.O365.Handlers
{
    public class NintexFormO365HandlerOnProvisionedEvent
    {
        /// <summary>
        /// Saves the response of the call to save api
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/NF_SC002_REF_API_R_SaveForm.htm%3FTocPath%3DNintex%2520Office%2520365%2520API%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%2520API%7CAPI%2520Reference%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%25C2%25A0API%25C2%25A0Resources%7C_____4"/>
        /// </summary>
        public HttpResponseMessage saveResponse = null;
        /// <summary>
        /// Saves the response of the call to publish api, if publish is requested
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/NF_SC002_REF_API_R_PublishForm.htm%3FTocPath%3DNintex%2520Office%2520365%2520API%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%2520API%7CAPI%2520Reference%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%25C2%25A0API%25C2%25A0Resources%7C_____5"/>
        /// </summary>
        public HttpResponseMessage puiblishResponse = null;
        /// <summary>
        /// Saves the response of the call to assigned use, note it might not work for some licensing schemes
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/NF_SC002_REF_API_R_AssignedUse.htm%3FTocPath%3DNintex%2520Office%2520365%2520API%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%2520API%7CAPI%2520Reference%7CNintex%2520Forms%2520for%2520Office%2520365%2520REST%25C2%25A0API%25C2%25A0Resources%7C_____1"/>
        /// </summary>
        public HttpResponseMessage assignedUseForProductionValue = null;
    }
}

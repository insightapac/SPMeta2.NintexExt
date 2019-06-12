using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.CSOM.O365.Services
{

    [Obsolete("Use NintexFormSettings")]
    public class NintexFormApiKeys
    {
        public static string ApiKey
        {
            get { return NintexFormSettings.ApiKey; }
            set { NintexFormSettings.ApiKey = value; }
        }
        public static string WebServiceUrl
        {
            get { return NintexFormSettings.WebServiceUrl; }
            set { NintexFormSettings.WebServiceUrl = value; }
        }
    }

    public class NintexFormSettings
    {
        /// <summary>
        /// The api key for the nintex operations
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/SDK_NFO_PRC__REST_QSG.htm#Get_your_API_key"/>
        [ThreadStatic]
        public static string ApiKey;

        /// <summary>
        /// The api key for the nintex operations
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/SDK_NFO_PRC__REST_QSG.htm#Get_your_API_key"/>
        [ThreadStatic]
        public static string WebServiceUrl;
        //TODO: calculate the url if not specified

        /// <summary>
        /// The api key for the nintex operations
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/SDK_NFO_PRC__REST_QSG.htm#Get_your_API_key"/>
        [ThreadStatic]
        public static TimeSpan HttpRequestTimeout = TimeSpan.FromMinutes(3);
    }
}

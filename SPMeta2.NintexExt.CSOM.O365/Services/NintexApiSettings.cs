using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.CSOM.O365.Services
{

    [Obsolete("Use NintexApiSettings")]
    public class NintexFormApiKeys
    {
        public static string ApiKey
        {
            get { return NintexApiSettings.ApiKey; }
            set { NintexApiSettings.ApiKey = value; }
        }
        public static string WebServiceUrl
        {
            get { return NintexApiSettings.WebServiceUrl; }
            set { NintexApiSettings.WebServiceUrl = value; }
        }
    }

    public class NintexApiSettings
    {
        /// <summary>
        /// The api key for the nintex operations
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/SDK_NFO_PRC__REST_QSG.htm#Get_your_API_key"/>
        public static string ApiKey;

        /// <summary>
        /// The web service url for the nintex operations
        /// </summary>
        /// <see cref="https://help.nintex.com/en-US/sdks/sdko365/#FormSDK/Topics/SDK_NFO_PRC__REST_QSG.htm#Get_your_API_key"/>
        public static string WebServiceUrl;
        //TODO: calculate the url if not specified

        /// <summary>
        /// The timeout for HttpClient.Timeout that is used in the implementation
        /// </summary>
        public static TimeSpan HttpRequestTimeout = TimeSpan.FromMinutes(3);

        /// <summary>
        /// Maximum amount of retries in a case of error coming from httpclient
        /// </summary>
        public static int  MaxRetries = 1;
    }
}

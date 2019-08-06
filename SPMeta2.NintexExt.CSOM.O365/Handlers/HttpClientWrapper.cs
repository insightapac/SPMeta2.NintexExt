using SPMeta2.NintexExt.CSOM.O365.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.CSOM.O365.Handlers
{
    public class HttpClientWrapper
    {
        private static HttpContent Clone(HttpContent content)
        {
            //TODO: not working 
            return content;
            HttpContent result = null;
            if (content is StringContent)
            {
                result = new StringContent(content.ReadAsStringAsync().Result);
            }
            else
            {
                result = new ByteArrayContent(content.ReadAsByteArrayAsync().Result);
            }

            return result;
        }

        HttpClient innerClient;
        public HttpClientWrapper(HttpClient innerClient)
        {
            this.innerClient = innerClient;
        }

        public HttpResponseMessage Put(string requestUri, HttpContent content)
        {
            HttpResponseMessage result = null;
            int iIdx = 0;
            do
            {
                result = innerClient.PutAsync(requestUri, Clone(content)).Result;
                if (!result.IsSuccessStatusCode)
                {//debug
                    var q = 1;
                }
            }
            while ((iIdx++ < NintexApiSettings.MaxRetries) && (result == null || !result.IsSuccessStatusCode));
            return result;
        }

        public HttpResponseMessage Post(string requestUri, HttpContent content)
        {
            HttpResponseMessage result = null;
            int iIdx = 0;
            do
            {
                //TODO: find the way to clone the content so it does not get disposed
                result = innerClient.PostAsync(requestUri, Clone(content)).Result;
                if (!result.IsSuccessStatusCode)
                {//debug
                    var q = 1;
                }
            }
            while ((iIdx++ < NintexApiSettings.MaxRetries) && (result == null || !result.IsSuccessStatusCode));
            return result;
        }

        public HttpResponseMessage Get(string requestUri)
        {
            HttpResponseMessage result = null;
            int iIdx = 0;
            do
            {
                result = innerClient.GetAsync(requestUri).Result;
                if (!result.IsSuccessStatusCode)
                {//debug
                    var q = 1;
                }
            }
            while ((iIdx++ < NintexApiSettings.MaxRetries) && (result == null || !result.IsSuccessStatusCode));
            return result;
        }
    }
}

using SPMeta2.NintexExt.CSOM.O365.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPMeta2.NintexExt.CSOM.O365.Handlers
{
    public class HttpClientWrapper
    {
        public class HttpErrorEventArgs : EventArgs
        {
            public HttpErrorEventArgs (HttpResponseMessage  message)
            {
                this.Message = message;
                this.StopProcessing = false;
            }
            public HttpResponseMessage Message { get; private set; }
            public bool StopProcessing { get;set; }
        }

        private static HttpContent Clone(HttpContent content)
        {
            if (content == null)
            {
                return null;
            }
            HttpContent result = null;
            if (content is StringContent)
            {
                result = new StringContent(content.ReadAsStringAsync().Result);
            }
            else
            {
                result = new ByteArrayContent(content.ReadAsByteArrayAsync().Result);
            }

            result.Headers.Clear();
            foreach ( var header in content.Headers)
            {
                result.Headers.Add(header.Key, header.Value);
            }

            return result;
        }

        HttpClient innerClient;
        public HttpClientWrapper(HttpClient innerClient)
        {
            this.innerClient = innerClient;
        }

        private static HttpResponseMessage ExecuteWrapper(string requestUri, HttpContent content, 
            Func<string,HttpContent, Task<HttpResponseMessage>> innerAction,
            EventHandler<HttpErrorEventArgs> errorHandler = null)
        {
            HttpResponseMessage result = null;
            int iIdx = 0;
            int timeout = NintexApiSettings.FirstRetryTimeoutMs;
            do
            {
                result = innerAction(requestUri, Clone(content)).Result;
                if (!result.IsSuccessStatusCode)
                {//debug
                    if (errorHandler != null)
                    {
                        var args = new HttpErrorEventArgs(result);
                        errorHandler(null, args);
                        if (args.StopProcessing)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(timeout);
                    timeout += NintexApiSettings.TimeoutIncreaseMs;
                }
            }
            while ((iIdx++ < NintexApiSettings.MaxRetries) && (result == null || !result.IsSuccessStatusCode));
            return result;
        }

        public HttpResponseMessage Put(string requestUri, HttpContent content, 
            EventHandler<HttpErrorEventArgs> errorHandler = null)
        {
            return ExecuteWrapper(requestUri, content, innerClient.PutAsync, errorHandler);
        }

        public HttpResponseMessage Post(string requestUri, HttpContent content, 
            EventHandler<HttpErrorEventArgs> errorHandler = null)
        {
            return ExecuteWrapper(requestUri, content, innerClient.PostAsync, errorHandler);
        }

        public HttpResponseMessage Get(string requestUri)
        {
            return ExecuteWrapper(requestUri, null, 
                (innerRequestUri,content) =>{ return innerClient.GetAsync(innerRequestUri); }
            );

        }
    }
}

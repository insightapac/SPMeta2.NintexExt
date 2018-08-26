/* FormsHeaderInspector.cs

   Copyright (c) 2014 - Nintex. All Rights Reserved.  
   This code released under the terms of the  
   Microsoft Reciprocal License (MS-RL,  http://opensource.org/licenses/MS-RL.html.)
   
*/

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace NintexFormsClient
{
    class FormsHeaderInspector : IClientMessageInspector
    {
        private string _formDigest;

        /// <summary>
        /// Creates an instance, specifying the form digest to use for request headers.
        /// </summary>
        /// <param name="formDigest">The form digest to use.</param>
        public FormsHeaderInspector(string formDigest)
        {
            _formDigest = formDigest;
        }
        
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Does nothing
        }

        /// <summary>
        /// Sets the value of the X-RequestDigest header property to the form digest prior to sending the request.
        /// </summary>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        /// <returns>A null reference.</returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;

            // If an appropriate HttpRequestMessageProperty is available, set the value of the X-RequestDigest 
            // header property to the form digest; otherwise, create a new HttpRequestMessageProperty and add 
            // the X-RequestDigest header property to it.
            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessage = (HttpRequestMessageProperty)httpRequestMessageObject;
                if (string.IsNullOrEmpty(httpRequestMessage.Headers["X-RequestDigest"]))
                {
                    httpRequestMessage.Headers["X-RequestDigest"] = _formDigest;
                }
            }
            else
            {
                httpRequestMessage = new HttpRequestMessageProperty();
                httpRequestMessage.Headers.Add("X-RequestDigest",  _formDigest);

                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
            }

            return null;
        }
    }
}

/* FormsHeaderBehavior.cs

   Copyright (c) 2014 - Nintex. All Rights Reserved.  
   This code released under the terms of the  
   Microsoft Reciprocal License (MS-RL,  http://opensource.org/licenses/MS-RL.html.)
   
*/

using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace NintexFormsClient
{
    class FormsHeaderBehavior : WebHttpBehavior
    {
        // The form digest to use when validating client requests.
        private string _formDigest;

        /// <summary>
        /// Creates an instance, using the specified form digest.
        /// </summary>
        /// <param name="formDigest">The form digest to use.</param>
        public FormsHeaderBehavior(string formDigest)
        {
            _formDigest = formDigest;
        }

        /// <summary>
        /// Adds a new message inspector to the service endpoint.
        /// </summary>
        /// <param name="endpoint">The Nintex Forms service endpoint.</param>
        /// <param name="runtime">The client runtime.</param>
        /// <remarks>The custom client message inspector adds the X-RequestDigest 
        /// header property to requests and sets the value of the header property 
        /// to the form digest.</remarks>
        public override void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime runtime)
        {
            runtime.MessageInspectors.Add(new FormsHeaderInspector(_formDigest));
            base.ApplyClientBehavior(endpoint, runtime);
        }

        //public override void Validate(ServiceEndpoint endpoint)
        //{
        //    base.Validate(endpoint);
        //}

        //public override void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        //{
        //    base.AddBindingParameters(endpoint, bindingParameters);
        //}

        //public override void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        //{
        //    base.ApplyDispatchBehavior(endpoint, endpointDispatcher);
        //}
    }
}

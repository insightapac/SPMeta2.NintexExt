/* FormDigestFactory.cs

   Copyright (c) 2014 - Nintex. All Rights Reserved.  
   This code released under the terms of the  
   Microsoft Reciprocal License (MS-RL,  http://opensource.org/licenses/MS-RL.html.)
   
*/

using System.Net;

namespace NintexFormsClient
{
    /// <summary>
    /// Object factory for FormDigest instances.
    /// </summary>
    class FormDigestFactory
    {
        #region Private Members
        private static FormDigestFactory _formDigestFactory;
        #endregion

        #region Constructor
        protected FormDigestFactory()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current instance of the factory.
        /// </summary>
        public static FormDigestFactory Current
        {
            get { return _formDigestFactory ?? (_formDigestFactory = new FormDigestFactory()); }
        }

        /// <summary>
        /// Gets a new <see cref="NintexFormsClient.FormDigest" /> for the specified version, endpoint URL, and credentials.
        /// </summary>
        /// <param name="version">The SharePoint version for the service endpoint.</param>
        /// <param name="webUrl">The URL of the SharePoint site.</param>
        /// <param name="credentials">The credentials to use.</param>
        /// <returns>A <see cref="NintexFormsClient.FormDigest" /> implementation for the specified version, 
        /// endpoint URL, and credentials; otherwise, a null reference.</returns>
        public FormDigest Get(Version version, string webUrl, ICredentials credentials) 
        {
            switch (version)
            {
                case Version.SharePoint2013:
                    return new FormDigest2013(webUrl, credentials);
                case Version.SharePoint2010:
                    return new FormDigest2010(webUrl, credentials);
                default:
                    return null;
            }
        }

        #endregion
    }
}

/* FormSaveInfo.cs

   Copyright (c) 2014 - Nintex. All Rights Reserved.  
   This code released under the terms of the  
   Microsoft Reciprocal License (MS-RL,  http://opensource.org/licenses/MS-RL.html.)
   
*/

using System.Runtime.Serialization;

namespace NintexFormsClient
{
    /// <summary>
    /// Represents metadata associated with saving or publishing a form.
    /// </summary>
    [DataContract]
    public class FormSaveInfo
    {
        /// <summary>
        /// The URL for the form.
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// The current version of the form.
        /// </summary>
        [DataMember]
        public string Version { get; set; }
    }
}

//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.SqlTools.Hosting.Protocol.Contracts;
using Microsoft.SqlTools.ServiceLayer.QueryExecution.Contracts;

namespace Microsoft.SqlTools.Serialization.Contracts
{
    /// <summary>
    /// Information returned from a <see cref="CreateSessionRequest"/>.
    /// Contains a <see cref="SessionId"/>
    /// </summary>
    public class CreateSerializationSessionResponse
    {
        /// <summary>
        /// Unique ID to use when sending any requests
        /// </summary>
        public string SessionId { get; set; }
        public SaveResultsRequestParams Params { get; set; }

    }

    /// <summary>
    /// Information returned from a <see cref="CreateSessionRequest"/>.
    /// Contains success information, e.g. <see cref="SessionId"/>
    /// </summary>
    public class SessionCreatedParameters
    {
        /// <summary>
        /// Boolean indicating if the connection was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Unique ID to use when sending any requests for objects in the
        /// tree under the node
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Error message returned for a serialization session failure reason, if any.
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Parameters sent in as part of a 'create session' request
    /// </summary>
    public class CreateSessionRequestParameters
    {
        public SaveResultsRequestParams ResultsRequestParams { get; set; }        
        public DbColumnWrapper[] Columns { get; set; }
    }

    /// <summary>
    /// Establishes a serialization session. 
    /// </summary>
    public class CreateSessionRequest
    {
        public static readonly
            RequestType<CreateSessionRequestParameters, CreateSerializationSessionResponse> Type =
            RequestType<CreateSessionRequestParameters, CreateSerializationSessionResponse>.Create("query/createSerializationSession");
    }

}

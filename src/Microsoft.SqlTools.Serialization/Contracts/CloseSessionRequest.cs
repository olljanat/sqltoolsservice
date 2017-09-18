//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.SqlTools.Hosting.Protocol.Contracts;

namespace Microsoft.SqlTools.Serialization.Contracts
{
    /// <summary>
    /// Information returned from a <see cref="CloseSessionRequest"/>.
    /// Contains success information, a <see cref="SessionId"/> to be used when
    /// requesting closing an existing session.
    /// </summary>
    public class CloseSessionResponse
    {
        /// <summary>
        /// Boolean indicating if the session was closed successfully 
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Unique session id
        /// </summary>
        public string SessionId { get; set; }
    }

    /// <summary>
    /// Parameters to the <see cref="CloseSessionRequest"/>.
    /// </summary>
    public class CloseSessionParams
    {
        /// <summary>
        /// The Id returned from a <see cref="CreateSessionRequest"/>
        /// </summary>
        public string SessionId { get; set; }
    }

    public class CloseSessionRequest
    {
        public static readonly
            RequestType<CloseSessionParams, CloseSessionResponse> Type =
            RequestType<CloseSessionParams, CloseSessionResponse>.Create("query/serializationClosedSession");
    }
    
}

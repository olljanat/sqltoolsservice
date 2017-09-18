//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.SqlTools.Hosting.Protocol.Contracts;

namespace Microsoft.SqlTools.ServiceLayer.QueryExecution.Contracts
{
    /// <summary>
    /// Class used for storing results and how the results are to be serialized
    /// </summary>
    public class SerializationWriteRequestParameters
    {
        public string OwnerUri { get; set; }

        /// <summary>
        /// Results that are to be serialized into 'SaveFormat' format
        /// </summary>
        public DbCellValue[][] Rows { get; set; }

        /// <summary>
        /// Whether the current set of Rows passed in is the last for this file
        // </summary>
        public bool IsLast { get; set; }
    }

    public class SaveAsRequest
    {
        public static readonly
            RequestType<SerializationWriteRequestParameters, SaveResultRequestResult> Type =
            RequestType<SerializationWriteRequestParameters, SaveResultRequestResult>.Create("query/saveAs");
    }
}

//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;

namespace Microsoft.SqlTools.Hosting.Contracts
{
    /// <summary>
    /// Defines a class that describes the capabilities of a language
    /// client.  At this time no specific capabilities are listed for
    /// clients.
    /// </summary>
    public class ClientCapabilities
    {
        public Dictionary<string, object> workspace;
        public Dictionary<string, object> textDocument;
        public Dictionary<string, object> experimental;
    }
}


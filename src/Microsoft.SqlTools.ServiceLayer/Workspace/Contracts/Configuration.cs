//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.Collections.Generic;
using Microsoft.SqlTools.Hosting.Protocol.Contracts;

namespace Microsoft.SqlTools.ServiceLayer.Workspace.Contracts
{
    public class DidChangeConfigurationNotification<TConfig> 
    {
        public static readonly
            EventType<DidChangeConfigurationParams<TConfig>> Type =
            EventType<DidChangeConfigurationParams<TConfig>>.Create("workspace/didChangeConfiguration");
    }

    public class DidChangeConfigurationParams<TConfig>
    {
        public TConfig Settings { get; set; }
    }

    public class GetWorkspaceConfigurationRequest<TConfig>
    {
        public static readonly RequestType<ConfigurationParams, TConfig> Type =
            RequestType<ConfigurationParams, TConfig>.Create("workspace/configuration");
    }

    public class ConfigurationParams
    {
        public ConfigurationItem[] items;
    }

    public class ConfigurationItem
    {
        public string ScopeUri;
        public string Section;
    }
}

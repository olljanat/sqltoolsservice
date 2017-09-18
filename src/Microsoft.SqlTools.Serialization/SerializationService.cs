//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.SqlTools.Extensibility;
using Microsoft.SqlTools.Hosting;
using Microsoft.SqlTools.Hosting.Protocol;
using Microsoft.SqlTools.ServiceLayer.QueryExecution.Contracts;
using Microsoft.SqlTools.Utility;
using System.Threading;
using Microsoft.SqlTools.Serialization.Contracts;

namespace Microsoft.SqlTools.Serialization
{
    /// <summary>
    /// Service responsible for serializing and saving data into another format. This provides
    /// a generic API.
    /// </summary>
    
    [Export(typeof(IHostedService))]
    public class SerializationService : HostedService<SerializationService>, IComposableService
    {
        // Map of owner uri's to sessions
        internal ConcurrentDictionary<string, CreateSessionRequestParameters> SessionMap { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SerializationService()
        {
            SessionMap = new ConcurrentDictionary<string, CreateSessionRequestParameters>();
        }
        public override void InitializeService(IProtocolEndpoint serviceHost)
        {
            Logger.Write(LogLevel.Verbose, "Serialization initialized");
            // Register request and event handlers with the Service Host
            serviceHost.SetRequestHandler(CreateSessionRequest.Type, HandleCreateSerializationSessionRequest);
            serviceHost.SetRequestHandler(CloseSessionRequest.Type, HandleCloseSerializationSessionRequest);
        }

        internal async Task HandleCreateSerializationSessionRequest(CreateSessionRequestParameters parameters, RequestContext<CreateSerializationSessionResponse> context)
        {
            try
            {
                Logger.Write(LogLevel.Verbose, "HandleCreateSerializationSessionRequest");
                Func<Task<CreateSerializationSessionResponse>> doCreateSession = async () =>
                {
                    
                    return await Task.Factory.StartNew(() =>
                    {
                        string ownerUri = parameters.ResultsRequestParams.OwnerUri;
                        string sessionId = GenerateUniqueId(ownerUri, context.requestMessage.Id);
                        Validate.IsNotNull(nameof(context), context);
                        SessionMap.AddOrUpdate(sessionId, parameters, (key, oldSession) => parameters);
                        var response = new CreateSerializationSessionResponse() {SessionId = sessionId, Params = parameters.ResultsRequestParams};
                        return response;
                    });
            
                };

                await HandleRequestAsync(doCreateSession, context, "HandleCreateSerializationSessionRequest");
            }

            catch (Exception ex)
            {
                await context.SendError(ex.ToString());
            }

        }

        // Create a unique URI composed of the ownerUri (partially composed of a file name) of a createSession request and the message id of a request context.
        // Since there can be multiple concurrent serialization session per owner uri, this guarantees that we are able to create a unique session for each one.
        internal static string GenerateUniqueId(string ownerUri, string messageId)
        {
            return ownerUri + ":" + messageId;
        }

        internal async Task HandleCloseSerializationSessionRequest(CloseSessionParams closeSessionParams, RequestContext<CloseSessionResponse> context)
        {

            Logger.Write(LogLevel.Verbose, "HandleCloseSerializationSessionRequest");
            Func<Task<CloseSessionResponse>> closeSession = () =>
            {
                Validate.IsNotNull(nameof(closeSessionParams), closeSessionParams);
                Validate.IsNotNull(nameof(context), context);
                return Task.Factory.StartNew(() =>
                {
                    string sessionId = closeSessionParams.SessionId;
                    CreateSessionRequestParameters session = null;
                    bool success = false;
                    if (!SessionMap.TryGetValue(sessionId, out session))
                    {
                        Logger.Write(LogLevel.Verbose, $"Cannot close serialization session. Couldn't find session for id. {sessionId} ");
                    }

                    if (session != null)
                    {

                        SessionMap.TryRemove(sessionId, out session);
                        success = true;
                    }

                    var response = new CloseSessionResponse() { Success = success, SessionId = sessionId };
                    return response;
                });
            };

            await HandleRequestAsync(closeSession, context, "HandleCloseSerializationSessionRequest");
        }

        /// <summary>
        /// For tests only
        /// </summary>
        internal Task CreateSessionTask
        {
            get;
            private set;
        }

        private async Task HandleRequest<T>(Func<Task<T>> handler, RequestContext<T> requestContext, string requestType)
        {
            Logger.Write(LogLevel.Verbose, requestType);

            try
            {
                T result = await handler();
                await requestContext.SendResult(result);
            }
            catch (Exception ex)
            {
                await requestContext.SendError(ex.ToString());
            }
        }

    }
}

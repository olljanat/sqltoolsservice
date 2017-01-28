//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using System.Data.SqlClient;

namespace Microsoft.SqlTools.ServiceLayer.Connection.Contracts
{
    /// <summary>
    /// Message format for the initial connection request
    /// </summary>
    /// <remarks>
    /// If this contract is ever changed, be sure to update ConnectionDetailsExtensions methods.
    /// </remarks>
    public class ConnectionDetails : ConnectionSummary
    {
        #region Properties

        /// <summary>
        /// Gets or sets the connection password
        /// </summary>
        /// <returns></returns>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the authentication to use.
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.
        /// </summary>
        public bool? Encrypt { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate trust.
        /// </summary>
        public bool? TrustServerCertificate { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool? PersistSecurityInfo { get; set; }

        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.
        /// </summary>
        public int? ConnectTimeout { get; set; }

        /// <summary>
        /// The number of reconnections attempted after identifying that there was an idle connection failure.
        /// </summary>
        public int? ConnectRetryCount { get; set; }

        /// <summary>
        /// Amount of time (in seconds) between each reconnection attempt after identifying that there was an idle connection failure.
        /// </summary>
        public int? ConnectRetryInterval { get; set; }

        /// <summary>
        /// Gets or sets the name of the application associated with the connection string.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the name of the workstation connecting to SQL Server.
        /// </summary>
        public string WorkstationId { get; set; }

        /// <summary>
        /// Declares the application workload type when connecting to a database in an SQL Server Availability Group.
        /// </summary>
        public string ApplicationIntent { get; set; }

        /// <summary>
        /// Gets or sets the SQL Server Language record name.
        /// </summary>
        public string CurrentLanguage { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the connection will be pooled or explicitly opened every time that the connection is requested.
        /// </summary>
        public bool? Pooling { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int? MaxPoolSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int? MinPoolSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum time, in seconds, for the connection to live in the connection pool before being destroyed.
        /// </summary>
        public int? LoadBalanceTimeout { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether replication is supported using the connection.
        /// </summary>
        public bool? Replication { get; set; }

        /// <summary>
        /// Gets or sets a string that contains the name of the primary data file. This includes the full path name of an attachable database.
        /// </summary>
        public string AttachDbFilename { get; set; }

        /// <summary>
        /// Gets or sets the name or address of the partner server to connect to if the primary server is down.
        /// </summary>
        public string FailoverPartner { get; set; }

        /// <summary>
        /// If your application is connecting to an AlwaysOn availability group (AG) on different subnets, setting MultiSubnetFailover=true provides faster detection of and connection to the (currently) active server.
        /// </summary>
        public bool? MultiSubnetFailover { get; set; }

        /// <summary>
        /// When true, an application can maintain multiple active result sets (MARS).
        /// </summary>
        public bool? MultipleActiveResultSets { get; set; }

        /// <summary>
        /// Gets or sets the size in bytes of the network packets used to communicate with an instance of SQL Server.
        /// </summary>
        public int? PacketSize { get; set; }

        /// <summary>
        /// Gets or sets a string value that indicates the type system the application expects.
        /// </summary>
        public string TypeSystemVersion { get; set; }

        #endregion

        /// <summary>
        /// Build a connection string from a connection details instance
        /// </summary>
        public string GetConnectionString()
        {
            SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder
            {
                DataSource = ServerName,
                UserID = UserName,
                Password = Password
            };

            // Check for any optional parameters
            if (!string.IsNullOrEmpty(DatabaseName))
            {
                connectionBuilder.InitialCatalog = DatabaseName;
            }
            if (!string.IsNullOrEmpty(AuthenticationType))
            {
                switch (AuthenticationType)
                {
                    case "Integrated":
                        connectionBuilder.IntegratedSecurity = true;
                        break;
                    case "SqlLogin":
                        break;
                    default:
                        throw new ArgumentException(SR.ConnectionServiceConnStringInvalidAuthType(AuthenticationType));
                }
            }
            if (Encrypt.HasValue)
            {
                connectionBuilder.Encrypt = Encrypt.Value;
            }
            if (TrustServerCertificate.HasValue)
            {
                connectionBuilder.TrustServerCertificate = TrustServerCertificate.Value;
            }
            if (PersistSecurityInfo.HasValue)
            {
                connectionBuilder.PersistSecurityInfo = PersistSecurityInfo.Value;
            }
            if (ConnectTimeout.HasValue)
            {
                connectionBuilder.ConnectTimeout = ConnectTimeout.Value;
            }
            if (ConnectRetryCount.HasValue)
            {
                connectionBuilder.ConnectRetryCount = ConnectRetryCount.Value;
            }
            if (ConnectRetryInterval.HasValue)
            {
                connectionBuilder.ConnectRetryInterval = ConnectRetryInterval.Value;
            }
            if (!string.IsNullOrEmpty(ApplicationName))
            {
                connectionBuilder.ApplicationName = ApplicationName;
            }
            if (!string.IsNullOrEmpty(WorkstationId))
            {
                connectionBuilder.WorkstationID = WorkstationId;
            }
            if (!string.IsNullOrEmpty(ApplicationIntent))
            {
                ApplicationIntent intent;
                switch (ApplicationIntent)
                {
                    case "ReadOnly":
                        intent = System.Data.SqlClient.ApplicationIntent.ReadOnly;
                        break;
                    case "ReadWrite":
                        intent = System.Data.SqlClient.ApplicationIntent.ReadWrite;
                        break;
                    default:
                        throw new ArgumentException(SR.ConnectionServiceConnStringInvalidIntent(ApplicationIntent));
                }
                connectionBuilder.ApplicationIntent = intent;
            }
            if (!string.IsNullOrEmpty(CurrentLanguage))
            {
                connectionBuilder.CurrentLanguage = CurrentLanguage;
            }
            if (Pooling.HasValue)
            {
                connectionBuilder.Pooling = Pooling.Value;
            }
            if (MaxPoolSize.HasValue)
            {
                connectionBuilder.MaxPoolSize = MaxPoolSize.Value;
            }
            if (MinPoolSize.HasValue)
            {
                connectionBuilder.MinPoolSize = MinPoolSize.Value;
            }
            if (LoadBalanceTimeout.HasValue)
            {
                connectionBuilder.LoadBalanceTimeout = LoadBalanceTimeout.Value;
            }
            if (Replication.HasValue)
            {
                connectionBuilder.Replication = Replication.Value;
            }
            if (!string.IsNullOrEmpty(AttachDbFilename))
            {
                connectionBuilder.AttachDBFilename = AttachDbFilename;
            }
            if (!string.IsNullOrEmpty(FailoverPartner))
            {
                connectionBuilder.FailoverPartner = FailoverPartner;
            }
            if (MultiSubnetFailover.HasValue)
            {
                connectionBuilder.MultiSubnetFailover = MultiSubnetFailover.Value;
            }
            if (MultipleActiveResultSets.HasValue)
            {
                connectionBuilder.MultipleActiveResultSets = MultipleActiveResultSets.Value;
            }
            if (PacketSize.HasValue)
            {
                connectionBuilder.PacketSize = PacketSize.Value;
            }
            if (!string.IsNullOrEmpty(TypeSystemVersion))
            {
                connectionBuilder.TypeSystemVersion = TypeSystemVersion;
            }

            return connectionBuilder.ToString();
        }
    }
}

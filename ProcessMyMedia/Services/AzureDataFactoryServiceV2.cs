﻿namespace ProcessMyMedia.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.Azure.Management.DataFactory;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Rest.Azure.Authentication;
    using Microsoft.Azure.Management.DataFactory.Models;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Azure Data Factory client for V2 API
    /// https://docs.microsoft.com/en-us/azure/data-factory/connector-ftp
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Services.Contract.IDataFactoryService" />
    public class AzureDataFactoryServiceV2 : Contract.IDataFactoryService
    {
        private AdfConfiguration configuration;

        private DataFactoryManagementClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDataFactoryServiceV2" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AzureDataFactoryServiceV2(AdfConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Authentications the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task AuthAsync()
        {
            ClientCredential clientCredential =
                new ClientCredential(this.configuration.AadClientId, this.configuration.AadSecret);

            var clientCredentials = await ApplicationTokenProvider.LoginSilentAsync(this.configuration.AadTenantId,
                clientCredential, ActiveDirectoryServiceSettings.Azure);

            this.client = new DataFactoryManagementClient(new Uri(this.configuration.ArmEndpoint), clientCredentials)
            {
                SubscriptionId = this.configuration.SubscriptionId,
            };
        }

        /// <summary>
        /// Adds the linked service.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public async Task AddLinkedServiceAsync(string name, string type, Dictionary<string, object> properties)
        {
            await this.client.LinkedServices.CreateOrUpdateAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                name,
                new LinkedServiceResource(new LinkedService(additionalProperties:properties),
                    type:type));
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
           this.client?.Dispose();
        }
    }
}

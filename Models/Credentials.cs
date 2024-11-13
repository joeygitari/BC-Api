﻿using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net;
using System.ServiceModel;
using Newtonsoft.Json.Linq;

namespace BC_Api.Models
{
    public class Credentials
    {
        //private readonly ILogger<Credentials> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        // Constructor to initialize IConfiguration and HttpClient
        public Credentials(ILogger<Credentials> logger, IConfiguration configuration)
        {
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var handler = new HttpClientHandler
            {
                Credentials = CredentialCache.DefaultCredentials,
                // Ensure proper server certificate validation
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30) // Set a reasonable timeout
            };
        }


        public NAVWS.PortalIntegration_PortClient ObjNav()
        {
            // Configure the binding
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
            {
                Security =
                {
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Windows
                    }
                }
            };

            // Create the endpoint address
            var endpointAddress = new EndpointAddress("http://jo:7047/BC240/WS/CRONUS%20International%20Ltd./Codeunit/PortalIntegration");

            // Instantiate the service client with the binding and endpoint
            return new NAVWS.PortalIntegration_PortClient(binding, endpointAddress);
        }
    }

}

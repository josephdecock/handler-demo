using System.Text.Json.Serialization;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace ParClientWithCustomizedHandler
{
    public class ParEvents : OpenIdConnectEvents
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public override async Task StateDataFormatted(StateDataFormattedContext context)
        {
            var clientId = context.ProtocolMessage.ClientId;
            _httpClient.SetBasicAuthentication(clientId, "secret");

            var requestBody = new FormUrlEncodedContent(context.ProtocolMessage.Parameters);

            var disco = await _httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            if(disco.IsError)
            {
                throw new Exception(disco.Error);
            }
            var parEndpoint = disco.TryGetValue("pushed_authorization_request_endpoint").GetString();
            var response = await _httpClient.PostAsync(parEndpoint, requestBody);
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("PAR failure");
            }
            var par = await response.Content.ReadFromJsonAsync<ParResponse>();

             // Remove all the parameters from the protocol message, and replace with what we got from the PAR response
            context.ProtocolMessage.Parameters.Clear();
            // Then, set client id and request uri as parameters
            context.ProtocolMessage.ClientId = clientId;
            context.ProtocolMessage.RequestUri = par.RequestUri;
        }

        public override Task TokenResponseReceived(TokenResponseReceivedContext context)
        {
            return base.TokenResponseReceived(context);
        }

        private class ParResponse
        {
            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonPropertyName("request_uri")]
            public string RequestUri { get; set; }
        }
    }
}
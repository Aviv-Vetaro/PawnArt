using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;

namespace PawnArt.Web.Client.SignalR
{
    public class HubConnectionFactory
    {
        private readonly NavigationManager navigationManager;
        private readonly IAccessTokenProvider accessTokenProvider;
        private readonly ConcurrentDictionary<Uri, HubConnection> hubConnections;

        public HubConnectionFactory(NavigationManager navigationManager, IAccessTokenProvider accessTokenProvider)
        {
            this.navigationManager = navigationManager;
            this.accessTokenProvider = accessTokenProvider;
            hubConnections = new ConcurrentDictionary<Uri, HubConnection>();
        }
        public async ValueTask<HubConnection> ConnectAsync(string endpoint)
        {
            Uri hubUri = navigationManager.ToAbsoluteUri(endpoint);
            if(hubConnections.TryGetValue(hubUri, out var chachedConnection) && chachedConnection.State is HubConnectionState.Connected)
            {
                return chachedConnection;
            }
            var tokenResult = await accessTokenProvider.RequestAccessToken();

            if(tokenResult.TryGetToken(out var token))
            {
                
                HubConnection connection = new HubConnectionBuilder().WithUrl(hubUri, options =>
            {
                
                options.AccessTokenProvider = () => Task.FromResult(token.Value);
            }).WithAutomaticReconnect()
                    .Build();
                hubConnections[hubUri] = connection;
                await connection.StartAsync();
                return connection;
            }
            throw new InvalidOperationException("Could not get the access token");
                
        }
    }
}

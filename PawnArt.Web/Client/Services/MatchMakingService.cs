using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using PawnArt.Web.Client.SignalR;

namespace PawnArt.Web.Client.Services
{
    public class MatchMakingService
    {
        private readonly HubConnectionFactory hubConnectionFactory;

        public MatchMakingService(HubConnectionFactory hubConnectionFactory)
        {
            this.hubConnectionFactory = hubConnectionFactory;
        }
        public async Task<Guid> FindMatchAsync(int seconds)
        {
            
            HubConnection connection = await hubConnectionFactory.ConnectAsync("/hubs/MatchMaker");
            await connection.SendAsync("EnlistMatchMaking", seconds * 1000);
            var gameId = await GetGameIdAsync(connection);
            await DisconnectAsync(connection);
            return gameId;
        }
        private Task<Guid> GetGameIdAsync(HubConnection connection)
        {
            TaskCompletionSource<Guid> taskCompletionSource = new TaskCompletionSource<Guid>();
            connection.On<Guid>("NotifyGameFound", (id) => taskCompletionSource.TrySetResult(id));
            return taskCompletionSource.Task;
        }

        private async Task DisconnectAsync(HubConnection hubConnection)
        {
            await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
        }
    }               
}

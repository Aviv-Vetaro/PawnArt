using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using PawnArt.Web.Client.SignalR;
using PawnArt.Web.Shared;

namespace PawnArt.Web.Client.Services
{
    public class GameService
    {
        private const string Endpoint = "/hubs/Game";
        private readonly HubConnectionFactory hubConnectionFactory;
        //private HubConnection hubConnection;
        public GameService(HubConnectionFactory hubConnectionFactory)
        {
            this.hubConnectionFactory = hubConnectionFactory;
        }
        public event Action<GameStateDto> OnOpponentMadePly;
        public async Task ConnectAsync(string id)
        {
            HubConnection hubConnection = await hubConnectionFactory.ConnectAsync(Endpoint);
            hubConnection.On("BoardChanged", OnOpponentMadePly);
            await hubConnection.InvokeAsync("GetGameStateByIdAsync", Guid.Parse(id));
        }
        public async Task PlayMoveAsync(MoveDto moveMade, string id)
        {
            HubConnection hubConnection = await hubConnectionFactory.ConnectAsync(Endpoint);
            await hubConnection.InvokeAsync("PlayMoveAsync", moveMade, Guid.Parse(id));
        }
    }
}

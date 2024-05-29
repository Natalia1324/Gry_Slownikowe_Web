//using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class GameHub : Hub
{
    private static readonly List<string> players = new List<string>();
    public async Task JoinLobby(string playerName)
    {
        players.Add(playerName);
        await Clients.AllExcept(Context.ConnectionId).SendAsync("playerJoined", playerName);
    }

    public async Task SendMove(string[] boardState, int points, int playerId)
    {
       
        await Clients.All.SendAsync("ReceiveMove", boardState, points, playerId);
    }

    public async Task StartGame(string player1Name, string player2Name)
    {
    
        players.Remove(player1Name);
        players.Remove(player2Name);
        await Clients.All.SendAsync("GameStarted", player1Name, player2Name);
    }

}
using Minecraft_Server.clientbound.configuration;
using Minecraft_Server.clientbound.play;
using System.Net.Sockets;
using System.Text;
using static Minecraft_Server.clientbound.play.GameEvent;

namespace Minecraft_Server.serverbound.configuration
{
    static class Configuration
    {
        public static void Handle(TcpClient client, byte[] buffer)
        {
            var packetId = buffer[0];

            switch (packetId)
            {
                case 0: // Ignoring
                    KnownPacks.Send(client);
                    break;
                case 2: // Ignoring
                    break;
                case 3:
                    Players.ConnectedSockets[client]["NextState"] = ConnectionStates.Play;
                    Login.Send(client);
                    GameEvent.Send(client, gameEvents.StartWaitingLevelChunks, 0);
                    break;
                case 7: // Ignoring
                    RegistryData.Send(client);
                    FinishConfiguration.send(client);
                    break;
                default:
                    Console.WriteLine($"Unexpected Packet ID {packetId} during Login with data {BitConverter.ToString(buffer)}");
                    break;
            }
        }
    }
}

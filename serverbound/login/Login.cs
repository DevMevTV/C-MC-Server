using Minecraft_Server.clientbound.configuration;
using Minecraft_Server.clientbound.login;
using System.Net.Sockets;

namespace Minecraft_Server.serverbound.login
{
    static class Login
    {
        public static void Handle(TcpClient client, byte[] buffer)
        {
            var packetId = buffer[0];

            switch (packetId)
            {
                case 0:
                    LoginStart.Handle(client, buffer);
                    LoginSuccess.Send(client);
                    break;
                case 3:
                    Players.ConnectedSockets[client]["NextState"] = ConnectionStates.Configuration;
                    break;
                default:
                    Console.WriteLine($"Unexpected Packet ID {packetId} during Login with data {BitConverter.ToString(buffer)}");
                    break;
            }
        }
    }
}

using Minecraft_Server.clientbound.status;
using System.Net.Sockets;

namespace Minecraft_Server.serverbound
{
    internal static class Status
    {
        public static void Handle(TcpClient client, byte[] buffer)
        {
            var packetId = buffer[0];

            switch (packetId)
            {
                case 0:
                    StatusResponse.send(client);
                    break;
                case 1:
                    PingResponse.send(client, buffer);
                    break;
                default:
                    Console.WriteLine($"Unexpected Packet ID {packetId} during Status with data {BitConverter.ToString(buffer)}");
                    break;
            }
        }
    }
}

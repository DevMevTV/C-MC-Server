using Minecraft_Server.clientbound.login;
using Minecraft_Server.serverbound.login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Server.serverbound.play
{
    static class Play
    {
        public static void Handle(TcpClient client, byte[] buffer)
        {
            var packetId = buffer[0];

            switch (packetId)
            {
                default:
                    Console.WriteLine($"Unexpected Packet ID {packetId} during Login with data {BitConverter.ToString(buffer)}");
                    break;
            }
        }
    }
}

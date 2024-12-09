using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Server.clientbound.configuration
{
    static class FinishConfiguration
    {
        public static void send(TcpClient client)
        {
            var stream = client.GetStream();
            stream.Write([0x01, 0x03]);
        }
    }
}

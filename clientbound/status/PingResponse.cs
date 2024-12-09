
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Server.clientbound.status
{
    internal class PingResponse
    {
        public static void send(TcpClient client, byte[] buffer)
        {
            byte[] response = Datatypes.VarInt.Encode(buffer.Length).Concat(buffer).ToArray();
            var stream = client.GetStream();
            stream.Write(response, 0, response.Length);
        }
    }
}

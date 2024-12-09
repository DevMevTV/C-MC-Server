using System.Net.Sockets;
using System.Text;
using static Minecraft_Server.Datatypes;

namespace Minecraft_Server.clientbound.login
{
    static class LoginSuccess
    {
        public static void Send(TcpClient client)
        {
            var clientData = Players.ConnectedSockets[client];
            byte[] responseBuffer = [];
            UUID.Encode(ref responseBuffer, (string)clientData["uuid"]);
            Datatypes.String.Encode(ref responseBuffer, (string)clientData["username"]);
            VarInt.Encode(ref responseBuffer, 0);

            byte[] response = VarInt.Encode(responseBuffer.Length + 1)
                .Concat(new byte[] { 0x02 })
                .Concat(responseBuffer)
                .ToArray();

            var stream = client.GetStream();
            stream.Write(response);
        }
    }
}

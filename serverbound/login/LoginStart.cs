using System.Net.Sockets;

namespace Minecraft_Server.serverbound.login
{
    static class LoginStart
    {
        public static void Handle(TcpClient client, byte[] buffer)
        {
            var offset = 1;
            var username = Datatypes.String.Decode(buffer, ref offset);
            var uuid = Datatypes.UUID.Decode(buffer, ref offset);

            Players.ConnectedSockets[client].Add("username", username);
            Players.ConnectedSockets[client].Add("uuid", uuid);

            Console.WriteLine($"{username} joined with the uuid {uuid}");
        }
    }
}

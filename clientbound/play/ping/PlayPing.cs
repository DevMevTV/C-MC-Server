using System.Net.Sockets;

namespace Minecraft_Server.clientbound.play.ping
{
    static class PlayPing
    {
        public static void Send(TcpClient client)
        {
            int payload = 1234567890;

            byte[] pingData = BitConverter.GetBytes(payload);

            byte[] ping = Datatypes.VarInt.Encode(pingData.Length + 1)
                .Concat(new byte[] { 0x37 })
                .Concat(pingData)
                .ToArray();

            var stream = client.GetStream();
            stream.Write(ping);
        }
    }
}

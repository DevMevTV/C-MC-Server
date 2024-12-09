using System.Net.Sockets;

namespace Minecraft_Server.serverbound
{
    internal static class Handshake
    {
        public static void Handle(TcpClient client, byte[] buffer)
        {
            int offset = 1;
            int protocol_version = Datatypes.VarInt.Decode(buffer, ref offset);
            var (server_address, newOffset) = Datatypes.String.Decode(buffer, offset);
            int server_port = Datatypes.UnsignedShort.Decode(buffer, ref newOffset);
            int next_state = Datatypes.VarInt.Decode(buffer, ref newOffset);

            Players.ConnectedSockets[client]["protocol_version"] = protocol_version;
            Players.ConnectedSockets[client]["server_adress"] = server_address;
            Players.ConnectedSockets[client]["server_port"] = server_port;
            Players.ConnectedSockets[client]["NextState"] = (ConnectionStates)next_state;
        }
    }
}

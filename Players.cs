using System.Net.Sockets;

namespace Minecraft_Server
{
    internal class Players
    {
        public static Dictionary<TcpClient, Dictionary<string, object>> ConnectedSockets = new Dictionary<TcpClient, Dictionary<string, object>>();
    }
}

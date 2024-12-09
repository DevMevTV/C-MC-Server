using System.Net.Sockets;

namespace Minecraft_Server.clientbound.configuration
{
    static class KnownPacks
    {
        public static void Send(TcpClient client)
        {
            var known_packs = new[]
            {
                new
                {
                    Namespace = "minecraft",
                    ID = "core",
                    Version = "1.21.3"
                }
            };

            var responseData = Datatypes.VarInt.Encode(known_packs.Length);
            foreach (var pack in known_packs)
            {
                Datatypes.String.Encode(ref responseData, pack.Namespace);
                Datatypes.String.Encode(ref responseData, pack.ID);
                Datatypes.String.Encode(ref responseData, pack.Version);
            }

            byte[] response = Datatypes.VarInt.Encode(responseData.Length + 1)
                .Concat(new byte[] { 0x0E })
                .Concat(responseData)
                .ToArray();

            var stream = client.GetStream();
            stream.Write(response);
        }
    }
}
